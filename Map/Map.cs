using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

[Tool]
public class Map : Node2D
{
	[Export]
	private int Probes = 3;

	[Export(PropertyHint.Range, "0,16,")]
	private byte MapRadius
	{
		get => _mapRadius;
		set
		{
			if (value > 16) value = 16;

			_mapRadius = value;
			GenerateMap();
		}
	}

	private byte _mapRadius = 2;

	[Export]
	private int Seed
	{
		get => _seed;
		set
		{
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
		gameState.Phase = Phase.Scouting;

		// Hook into the UI
		ui.Connect(nameof(UI.OnEndTurn), this, nameof(OnEndTurn));

		// Add a rover for fun
		var rover = ConRover.Instance<Rover>();
		Root.AddChild(rover);
		rover.MapPosition = new HexPoint(0, 0, 0);
		Tiles[rover.MapPosition].HasRover = true;
	}

	private void OnPhaseChanged()
	{
		GD.Print("New phase!");
		switch (gameState.Phase) {
			case Phase.Income:
				resources.CalcResource();
				break;
			case Phase.CargoDrop:
				cargoQueue.Tick();
				break;
			case Phase.MovingRover:
				foreach (var rover in Root.GetChildren().OfType<Rover>()) {
					Tiles[rover.MapPosition].HasRover = false;
					rover.Move();
					Tiles[rover.MapPosition].HasRover = true;
					Reveal(rover.MapPosition);
				}
				break;
		}
	}

	private static readonly PackedScene ConTile = ResourceLoader.Load("res://Map/Tile.tscn") as PackedScene ?? throw new ArgumentNullException("No Tile Scene");
	private static readonly PackedScene ConRover = ResourceLoader.Load("res://Map/Rover.tscn") as PackedScene ?? throw new ArgumentNullException("No Rover Scene");

	private Node2D Root => GetNode<Node2D>("MapRoot");
	private Camera2D Camera => GetNode<Camera2D>("Camera2D");

	private GameState gameState => GetNode<GameState>("/root/GameState");
	private Godot.Collections.Dictionary<string, BuildingSpecs> buildingData => GetNode<BuildingData>("/root/BuildingData").buildings;
	private Resources resources => GetNode<Resources>("/root/Resources");
	private CargoQueue cargoQueue => GetNode<CargoQueue>("/root/CargoQueue");
	private Timer phaseTimer => GetNode<Timer>("PhaseTimer");
	private UI ui => GetNode<UI>("CanvasLayer/Control");

	private const float sideLength = 50f;

	private Godot.Collections.Dictionary<long, Tile> Tiles = new Godot.Collections.Dictionary<long, Tile>();

	private void GenerateMap()
	{
		// Don't generate if we're not ready
		if (!_isReady || Root == null)
		{
			GD.Print("Map but not ready");
			return;
		}

		GD.Print("Map time!");

		var rand = new Random(Seed);

		// Center the map tiles to start
		Root.Position = Camera.Position = GetViewport().GetVisibleRect().Size / 2;

		// Clear existing children
		foreach (var child in Root.GetChildren().OfType<Node2D>())
		{
			child.QueueFree();
		}
		Tiles.Clear();

		// Instantiate a bunch of tiles

		HexPoint zero = new HexPoint(0, 0, 0);


		// Generate tiles
		var zeroTile = AddTile(zero);
		//zeroTile.Color = Color.FromHsv((float)rand.NextDouble(), 0.5f, 0.5f);

		for (int radius = 0; radius <= MapRadius; radius++)
		{
			for (int i = 0; i < 6; i++)
			{
				for (int sideIndex = 0; sideIndex < radius; sideIndex++)
				{
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

	private static IEnumerable<HexPoint> TilesWithin(HexPoint center, int distance)
	{
		// Generate tiles
		yield return center;

		for (int radius = 0; radius <= distance; radius++)
		{
			for (int i = 0; i < 6; i++)
			{
				for (int sideIndex = 0; sideIndex < radius; sideIndex++)
				{
					HexPoint at = center + HexPoint.Directions[i] * radius + HexPoint.Directions[(i + 2) % 6] * sideIndex;
					yield return at;
				}
			}
		}
	}

	private Tile AddTile(HexPoint at)
	{
		var tile = ConTile.Instance<Tile>();

		tile.Position = at.ToVector2(sideLength);

		var col = at.Q + (at.R - (at.R & 1)) / 2;
		var row = at.R;

		//tile.GetNode<Label>("Label").Text = $"{at.Q}, {at.R}, {at.S}\n{col}, {row}";

		Root.AddChild(tile);

		if (Tiles.ContainsKey(at)) throw new InvalidOperationException($"Tile already exists at {at}");
		Tiles.Add(at, tile);
		tile.MapPosition = at;

		tile.Connect(nameof(Tile.OnScout), this, nameof(onScout), new Godot.Collections.Array(at));
		tile.Connect(nameof(Tile.OnBuild), this, nameof(OnStartBuild), new Godot.Collections.Array(at));
		tile.Connect(nameof(Tile.OnSelectRover), this, nameof(OnSelectRover), new Godot.Collections.Array(at));
		tile.Connect(nameof(Tile.OnTargetRover), this, nameof(OnTargetRover), new Godot.Collections.Array(at));

		return tile;
	}

	private void onScout(HexPoint tile)
	{
		GD.Print("Scouting", tile);
		if (Probes > 0)
		{
			Reveal(tile);
			Probes -= 1;
			if (Probes == 0)
			{
				GetNode<GameState>("/root/GameState").Phase = Phase.InTurn;
			}
		}
	}

	private void OnEndTurn() {
		if (gameState.Phase != Phase.InTurn) {
			GD.Print("Probably not the right time to end the turn");
		}

		// 1. Do cargo drop phase
		// 2. Do Income phase
		// 3. Move the rover
		// But it'll all be distributed, just kick off the timer
		gameState.Phase = Phase.CargoDrop;
		phaseTimer.Start();

	}

	private void _on_PhaseTimer_timeout() {
		switch (gameState.Phase) {
			case Phase.CargoDrop:
				gameState.Phase = Phase.Income;
				phaseTimer.Start();
				break;
			case Phase.Income:
				gameState.Phase = Phase.MovingRover;
				phaseTimer.Start();
				break;
			case Phase.MovingRover:
				gameState.Phase = Phase.InTurn;
				break;
		}
	}

	private void OnSelectRover(HexPoint tile) {
		
		if (Tiles[tile].HasRover) {
			GD.Print("Rovinng", tile);
			gameState.RoverStartPoint = tile;
			gameState.Phase = Phase.TargettingRover;
		}
	}

	private void OnTargetRover(HexPoint tile) {
		
		if (gameState.Phase == Phase.TargettingRover) {
			GD.Print("Targeting", tile);
			gameState.Phase = Phase.InTurn;
			GD.Print(Root.GetChildren().OfType<Rover>().ToList()[0].MapPosition, gameState.RoverStartPoint);
			var rover = Root.GetChildren().OfType<Rover>().First(r => r.MapPosition == gameState.RoverStartPoint);
			rover.Destination = tile;
		}
	}

	private void GenerateCluster(TileType type, int size, Random rand)
	{
		// Pick a random plains tile
		var candidates = Tiles.Keys.Where(t => Tiles[t].Type == TileType.Plains).ToList();

		// Shuffle a bit (fisher yates)
		for (int i = candidates.Count - 1; i > 0; i--)
		{
			int j = rand.Next(i + 1);
			(candidates[j], candidates[i]) = (candidates[i], candidates[j]);
		}

		// Try until we find one that works
		foreach (var candidate in candidates)
		{
			//GD.Print("Checking candidate", (HexPoint)candidate);
			// Check if it's surrounded by plains
			var applicable = new List<HexPoint>() {
				(HexPoint)candidate
			};

			for (int neighborIdx = 0; neighborIdx < 6; neighborIdx++)
			{
				var neighbor = (HexPoint)candidate + HexPoint.Directions[neighborIdx];
				if (!Tiles.ContainsKey(neighbor) || Tiles[neighbor].Type != TileType.Plains) continue;

				applicable.Add(neighbor);
			}

			if (applicable.Count >= size)
			{

				// Put it here
				// Shuffle first for fun (fisher yates again)
				for (int i = applicable.Count - 1; i > 0; i--)
				{
					int j = rand.Next(i + 1);
					(applicable[j], applicable[i]) = (applicable[i], applicable[j]);
				}

				// Take the first n
				applicable = applicable.Take(size).ToList();
				foreach (var coord in applicable)
				{
					Tiles[coord].Type = type;
				}

				return;
			}
		}
	}

	public void Reveal(HexPoint center)
	{
		Tiles[center].Foggy = false;

		foreach (var dir in HexPoint.Directions)
		{
			// Off the map or similar
			if (!Tiles.ContainsKey(center + dir)) continue;

			Tiles[center + dir].Foggy = false;
		}
	}

	public List<string> GetBuildingsAvailableForTile(HexPoint tile)
	{
		// TODO get what we have queued up
		var available = buildingData.Keys;

		return buildingData.Keys.Where(k => CanBuild(tile, buildingData[k])).ToList();
	}

	private void OnStartBuild(HexPoint tile) {
		var available = GetBuildingsAvailableForTile(tile);
		gameState.SelectedTile = tile;

		GD.Print("Available", string.Join(", ", available));
		
		ui.OnStartBuilding(available);
	}

	private void _on_Control_OnBuild(string buildingId) {
		GD.Print("Maybe building", buildingId);

		var tile = gameState.SelectedTile;
		var building = buildingData[buildingId];

		if (CanBuild(tile, building)) {
			GD.Print("Building", buildingId, "on", tile);

			if (Tiles[tile].Building.Type != "None") {
				GD.Print("  Replacing", Tiles[tile].Building.Type);
				resources.BuildingBuilt(Tiles[tile].Building.Type, -1);
			}

			Tiles[tile].Building.Type = buildingId;
			resources.BuildingBuilt(buildingId);
		} else {
			GD.Print("Can't build", buildingId, "on", tile);
		}
	}

	private bool CanBuild(HexPoint tile, BuildingSpecs spec)
	{
		// GD.Print("Considering", spec, "on", Tiles[tile].Type);
		return spec.buildingRequirements.All(req => IsRequirementSatisfied(tile, req));
	}

	private bool IsRequirementSatisfied(HexPoint tile, BuildingRequirements req)
	{
		switch (req.type)
		{
			case "tileWithin":
				// Must have the specified tile within the specified distance (or not)
				foreach (var coord in TilesWithin(tile, req.distance))
				{
					if (Tiles.ContainsKey(coord) && Tiles[coord].Type.ToString() == req.targetType)
					{
						// GD.Print("Matched tileWithin", coord, req.targetType, req.negate);
						return !req.negate;
					}
				}
				// GD.Print("  Failed tileWithin-", req.targetType, "-", Tiles[tile].Type.ToString());
				return req.negate;
			case "buildingDistance":
				// Must have the specified building within the specified distance (or not)
				foreach (var coord in TilesWithin(tile, req.distance))
				{
					if (Tiles.ContainsKey(coord) && Tiles[coord].Building != null && Tiles[coord].Building.Type == req.targetType)
					{
						// GD.Print("  Matched buildingDistance", coord, req.targetType, req.negate);
						return !req.negate;
					}
				}
				// GD.Print("  Failed buildingDistance", req.targetType, req.negate);
				return req.negate;
			case "notBuildable":
				// GD.Print("Cannot build");
				return false;
			default:
				throw new Exception($"Unknown requirement type {req.type}");
		}
	}
}
