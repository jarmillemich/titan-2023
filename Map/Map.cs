using Godot;
using System;
using System.Collections.Generic;
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

		gameState.Connect(nameof(GameState.OnPhaseChanged), this, nameof(OnPhaseChanged));
		gameState.Phase = Phase.Income;
	}

	private void OnPhaseChanged() {
		GD.Print("New phase!");
	}

	private static readonly PackedScene ConTile = ResourceLoader.Load("res://Map/Tile.tscn") as PackedScene ?? throw new ArgumentNullException("No Tile Scene");

	private Node2D Root => GetNode<Node2D>("MapRoot");
	private Camera2D Camera => GetNode<Camera2D>("Camera2D");

	private GameState gameState => GetNode<GameState>("/root/GameState");

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
					tile.Type = TileType.Plains;
					tile.Foggy = true;
				}
			}
		}

		// Generate tiles
		for (int i = 0; i < 4; i++) GenerateCluster(TileType.Mountain, 4, rand);
		for (int i = 0; i < 4; i++) GenerateCluster(TileType.Lake, 4, rand);
		for (int i = 0; i < 3; i++) GenerateCluster(TileType.Crater, 4, rand);
		for (int i = 0; i < 2; i++) GenerateCluster(TileType.Cryovolcano, 2, rand);
	}

	private Tile AddTile(HexPoint at) {
		var tile = ConTile.Instance<Tile>();
					
		tile.Position = at.ToVector2(sideLength);

		var col = at.Q + (at.R - (at.R & 1)) / 2;
		var row = at.R;

		//tile.GetNode<Label>("Label").Text = $"{at.Q}, {at.R}, {at.S}\n{col}, {row}";

		Root.AddChild(tile);

		// GD.Print("Adding tile at", at, Tiles.ContainsKey(at));
		if (Tiles.ContainsKey(at)) throw new InvalidOperationException($"Tile already exists at {at}");
		Tiles.Add(at, tile);

		// GD.Print("Adding tile at", at, Tiles.ContainsKey(at));

		tile.Connect(nameof(Tile.OnScout), this, nameof(onScout), new Godot.Collections.Array(at));

		return tile;
	}

	private void onScout(HexPoint tile) {
		GD.Print("Scouting", tile);
		Reveal(tile);
	}

	private void GenerateCluster(TileType type, int size, Random rand) {
		// Pick a random plains tile
		var candidates = Tiles.Keys.Where(t => Tiles[t].Type == TileType.Plains).ToList();

		// Shuffle a bit (fisher yates)
		// TODO vet this
		for (int i = candidates.Count - 1; i > 0; i--) {
			int j = rand.Next(i + 1);
            (candidates[j], candidates[i]) = (candidates[i], candidates[j]);
        }

        // Try until we find one that works
        foreach (var candidate in candidates) {
			//GD.Print("Checking candidate", (HexPoint)candidate);
			// Check if it's surrounded by plains
			var applicable = new List<HexPoint>() {
				(HexPoint)candidate
			};
			for (int neighborIdx = 0; neighborIdx < 6; neighborIdx++) {
				var neighbor = (HexPoint)candidate + HexPoint.Directions[neighborIdx];
				if (!Tiles.ContainsKey(neighbor) || Tiles[neighbor].Type != TileType.Plains) continue;

				applicable.Add(neighbor);
			}

			//GD.Print("  Got ", applicable.Count);

			if (applicable.Count >= size) {
				//GD.Print("  That was it");

				// Put it here
				// Shuffle first for fun (fisher yates again)
				for (int i = applicable.Count - 1; i > 0; i--) {
					int j = rand.Next(i + 1);
                    (applicable[j], applicable[i]) = (applicable[i], applicable[j]);
                }

                // Take the first n
                applicable = applicable.Take(size).ToList();
				foreach (var coord in applicable) {
					Tiles[coord].Type = type;
				}

				return;
			}
		}
	}

	public void Reveal(HexPoint center) {
		Tiles[center].Foggy = false;

		foreach (var dir in HexPoint.Directions) {
			Tiles[center + dir].Foggy = false;
		}
	}
}
