using Godot;
using System;

[Tool]
public class Rover : Node2D
{
    public HexPoint Position {
        get => _position;
        set {
            _position = value;
            if (_isReady) UpdateSprite();
        }
    }

    private HexPoint _position = new HexPoint(0, 0, 0);

    public HexPoint Destination {
        get => _destination;
        set {
            _destination = value;
            if (_isReady) UpdateSprite();
        }
    }

    private HexPoint _destination = new HexPoint(0, 0, 0);

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        _isReady = true;
        UpdateSprite();
    }

    private bool _isReady = false;

    private void UpdateSprite()
    {
        bool moving = _destination.DistanceTo(_position) > 0;
        GetNode<Line2D>("Arrow").Visible = moving;

        if (moving) {
            Rotation = _position.ToVector2(50f).AngleTo(_destination.ToVector2(50f));
        }

    }

}
