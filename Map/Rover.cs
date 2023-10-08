using Godot;
using System;

[Tool]
public class Rover : Node2D
{
    public HexPoint MapPosition {
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

    public void Move() {
        GD.Print("Moving rover", _position, _destination);
        MapPosition = _destination;
        UpdateSprite();
    }

    private bool _isReady = false;

    private void UpdateSprite()
    {
        Position = MapPosition.ToVector2(50f);

        bool moving = _destination.DistanceTo(_position) > 0;
        GetNode<Sprite>("Arrow").Visible = moving;

        if (moving) {
            Rotation = Vector2.Right.AngleTo((_destination - _position).ToVector2(50f));
            GD.Print("Moving rover", _position, _destination, Rotation);
        }

    }

}
