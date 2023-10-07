using Godot;
using System;

public partial class PanCamera : Camera2D
{
	private Vector2 Velocity = Vector2.Zero;

	public override void _UnhandledInput(InputEvent evt)
	{
        if (evt is InputEventKey key) {
            int speed = 250;

            // Use WASD to move around
            if (key.IsPressed() && !key.IsEcho()) {
                if (key.Scancode == (int)KeyList.W) {
                    Velocity += new Vector2(0, -speed);
                } else if (key.Scancode == (int)KeyList.A) {
                    Velocity += new Vector2(-speed, 0);
                } else if (key.Scancode == (int)KeyList.S) {
                    Velocity += new Vector2(0, speed);
                } else if (key.Scancode == (int)KeyList.D) {
                    Velocity += new Vector2(speed, 0);
                }
            } else if (!key.IsPressed()) {
                if (key.Scancode == (int)KeyList.W) {
                    Velocity -= new Vector2(0, -speed);
                } else if (key.Scancode == (int)KeyList.A) {
                    Velocity -= new Vector2(-speed, 0);
                } else if (key.Scancode == (int)KeyList.S) {
                    Velocity -= new Vector2(0, speed);
                } else if (key.Scancode == (int)KeyList.D) {
                    Velocity -= new Vector2(speed, 0);
                }
            }
        }

	}

    public override void _Process(float delta)
    {
        Position += Velocity * delta;
    }
}
