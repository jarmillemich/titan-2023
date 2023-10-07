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

	private TileType _type = TileType.Plains;

	private Polygon2D Sprite => GetNode<Polygon2D>("Polygon2D");

	private void UpdateSprite() {
		// TODO
	}

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_isReady = true;
		UpdateSprite();
	}

	private bool _isReady = false;
}
