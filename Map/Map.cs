using Godot;
using System;
using System.Linq;

[Tool]
public class Map : Node2D
{
[Export(PropertyHint.Range, "0,16,")]
	private byte MapRadius {
		get => _mapRadius;
		set {
			if (value > 16) value = 16;

			_mapRadius = value;
			GenerateMap();
		}
	}

	private byte _mapRadius = 2;

	[Export]
	private int Seed {
		get => _seed;
		set {
			_seed = value;
			GenerateMap();
		}
	}

	private int _seed = 0;

	private bool _isReady = false;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_isReady = true;
		GenerateMap();

		GD.Print("Map ready");
	}

	private static readonly PackedScene Tile = ResourceLoader.Load("res://Map/Tile.tscn") as PackedScene ?? throw new ArgumentNullException("No Tile Scene");

	private Node2D Root => GetNode<Node2D>("MapRoot");
	private Camera2D Camera => GetNode<Camera2D>("Camera2D");

private const float sideLength = 50f;

	private Godot.Collections.Dictionary<long, Tile> Tiles = new Godot.Collections.Dictionary<long, Tile>();

	private void GenerateMap() {
		// Don't generate if we're not ready
		// if (!IsNodeReady() || Root == null) {
		if (!_isReady || Root == null) {
			GD.Print("Map but not ready");
			return;
		}

		GD.Print("Map time!");

		var rand = new Random(Seed);

		// Center the map tiles to start
		Root.Position = Camera.Position = GetViewport().GetVisibleRect().Size / 2;
		
		// Clear existing children
		foreach (var child in Root.GetChildren().OfType<Node2D>()) {
			child.QueueFree();
		}
		Tiles.Clear();
		
		// Instantiate a bunch of tiles
		
		HexPoint zero = new HexPoint(0, 0, 0);


		// Generate tiles
		var zeroTile = AddTile(zero);
		//zeroTile.Color = Color.FromHsv((float)rand.NextDouble(), 0.5f, 0.5f);

		for (int radius = 0; radius <= MapRadius; radius++) {
			for (int i = 0; i < 6; i++) {
				for (int sideIndex = 0; sideIndex < radius; sideIndex++) {
					HexPoint at = zero + HexPoint.Directions[i] * radius + HexPoint.Directions[(i + 2) % 6] * sideIndex;
					var tile = AddTile(at);

					//tile.Color = Color.FromHsv((float)rand.NextDouble(), 0.5f, 0.5f);
				}
			}
		}
	}

	private Tile AddTile(HexPoint at) {
		var tile = Tile.Instance<Tile>();
					
		tile.Position = at.ToVector2(sideLength);

		var col = at.Q + (at.R - (at.R & 1)) / 2;
		var row = at.R;

		//tile.GetNode<Label>("Label").Text = $"{at.Q}, {at.R}, {at.S}\n{col}, {row}";

		Root.AddChild(tile);

		// GD.Print("Adding tile at", at, Tiles.ContainsKey(at));
		if (Tiles.ContainsKey(at)) throw new InvalidOperationException($"Tile already exists at {at}");
		Tiles.Add(at, tile);

		// GD.Print("Adding tile at", at, Tiles.ContainsKey(at));

		return tile;
	}
}