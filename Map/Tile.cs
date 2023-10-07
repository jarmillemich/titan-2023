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

	[Signal]
	public delegate void OnScout();

	public void _on_ScoutButton_pressed() {
		EmitSignal(nameof(OnScout));
	}

	private bool _foggy = true;

	private TileType _type = TileType.Plains;

	private Polygon2D background => GetNode<Polygon2D>("Polygon2D");
	private Sprite fog => GetNode<Sprite>("Fog");
	private TextureButton scoutButton => GetNode<TextureButton>("ScoutButton");

	private void UpdateSprite() {
		switch (Type) {
            case TileType.Plains:
                background.Texture = ResourceLoader.Load("res://Graphics/Tiles/ice_plains.png") as Texture;
                break;
            case TileType.Mountain:
                background.Texture = ResourceLoader.Load("res://Graphics/Tiles/mountains.png") as Texture;
                break;
            case TileType.Lake:
                background.Texture = ResourceLoader.Load("res://Graphics/Tiles/methane lakes 2.png") as Texture;
                break;
            case TileType.Crater:
                background.Texture = ResourceLoader.Load("res://Graphics/Tiles/craters.png") as Texture;
                break;
            case TileType.Cryovolcano:
                background.Texture = ResourceLoader.Load("res://Graphics/Tiles/cryovolcano.png") as Texture;
                break;
        }

		scoutButton.Visible = fog.Visible = _foggy;
		
	}

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_isReady = true;
		UpdateSprite();
	}

	private bool _isReady = false;
}
