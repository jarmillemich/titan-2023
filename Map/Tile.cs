using Godot;
using System;

public enum TileType {
	Plains,
	Mountain,
	Lake,
	Crater,
	Cryovolcano
}

[Tool]
public class Tile : Node2D
{
	[Export]
	public TileType Type {
		get => _type;
		set {
			_type = value;
			if (_isReady) UpdateSprite();
		}
	}

	[Export]
	public bool Foggy {
		get => _foggy;
		set {
			_foggy = value;
			if (_isReady) UpdateSprite();
		}
	}

	public bool HasRover { get => _hasRover; set {
		_hasRover = value;
		// XXX
		OnPhaseChanged();
	} }
	private bool _hasRover = false;
	public HexPoint MapPosition { get; set; }

	[Signal]
	public delegate void OnScout();
	
	[Signal]
	public delegate void OnBuild();

	[Signal]
	public delegate void OnSelectRover();

	[Signal]
	public delegate void OnTargetRover();

	public void _on_ScoutButton_pressed() {
		EmitSignal(nameof(OnScout));
	}

	public void _on_BuildButton_pressed() {
		EmitSignal(nameof(OnBuild));
	}

	public void _on_RoverButton_pressed() {
		GD.Print("Button");
		EmitSignal(nameof(OnSelectRover));
	}

	public void _on_RoverTargetButton_pressed() {
		EmitSignal(nameof(OnTargetRover));
		gameState.Phase = Phase.InTurn;
	}

	private bool _foggy = true;

	private TileType _type = TileType.Plains;

	private Polygon2D background => GetNode<Polygon2D>("Polygon2D");
	private Sprite fog => GetNode<Sprite>("Fog");
	private TextureButton scoutButton => GetNode<TextureButton>("Buttons/ScoutButton");
	private TextureButton buildButton => GetNode<TextureButton>("Buttons/BuildButton");
	private TextureButton roverButton => GetNode<TextureButton>("Buttons/RoverButton");
	private TextureButton roverTargetButton => GetNode<TextureButton>("Buttons/RoverTargetButton");

	public Building Building => GetNode<Building>("Building");

	private void UpdateSprite() {
		switch (Type) {
            case TileType.Plains:
                background.Texture = ResourceLoader.Load("res://Graphics/Tiles/ice_plains - low.png") as Texture;
                break;
            case TileType.Mountain:
                background.Texture = ResourceLoader.Load("res://Graphics/Tiles/mountains - low.png") as Texture;
                break;
            case TileType.Lake:
                background.Texture = ResourceLoader.Load("res://Graphics/Tiles/methane lakes 2 - low.png") as Texture;
                break;
            case TileType.Crater:
                background.Texture = ResourceLoader.Load("res://Graphics/Tiles/craters - low.png") as Texture;
                break;
            case TileType.Cryovolcano:
                background.Texture = ResourceLoader.Load("res://Graphics/Tiles/cryovolcano - low.png") as Texture;
                break;
        }

		scoutButton.Visible = fog.Visible = _foggy;
		
	}

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_isReady = true;
		UpdateSprite();
		
		gameState?.Connect(nameof(GameState.OnPhaseChanged), this, nameof(OnPhaseChanged));
	}

	private bool _isReady = false;

	private GameState gameState => GetNode<GameState>("/root/GameState");

	private void OnPhaseChanged() {
		scoutButton.Visible = gameState.Phase == Phase.Scouting;
		
		// Can build in the "turn" phase, or move a rover if we have one
		buildButton.Visible = gameState.Phase == Phase.InTurn && !HasRover && !Foggy;
		roverButton.Visible = gameState.Phase == Phase.InTurn && HasRover;

		// Can move a rover here in the "move rover" pseudo-phase
		// And we are 1 tile from the source rover
		if (gameState.Phase == Phase.TargettingRover) {
			roverTargetButton.Visible = gameState.RoverStartPoint.DistanceTo(MapPosition) == 1 && !HasRover;
		} else {
			roverTargetButton.Visible = false;
		}
	}
}
