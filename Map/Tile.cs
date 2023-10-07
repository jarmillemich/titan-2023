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

	public bool HasRover { get; set; }

	[Signal]
	public delegate void OnScout();
	
	[Signal]
	public delegate void OnBuild();

	[Signal]
	public delegate void OnSelectRover();

	public void _on_ScoutButton_pressed() {
		EmitSignal(nameof(OnScout));
	}

	public void _on_BuildButton_pressed() {
		EmitSignal(nameof(OnBuild));
	}

	public void _on_RoverButton_pressed() {
		EmitSignal(nameof(OnSelectRover));
	}

	private bool _foggy = true;

	private TileType _type = TileType.Plains;

	private Polygon2D background => GetNode<Polygon2D>("Polygon2D");
	private Sprite fog => GetNode<Sprite>("Fog");
	private TextureButton scoutButton => GetNode<TextureButton>("ScoutButton");
	private TextureButton buildButton => GetNode<TextureButton>("BuildButton");

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
		scoutButton.Disabled = gameState.Phase != Phase.Scouting;
		buildButton.Disabled = gameState.Phase != Phase.InTurn;
	}
}
