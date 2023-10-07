using Godot;
using System;

public partial class PanCamera : Camera2D
{
	private Vector2 LastMousePosition;
	private bool Dragging = false;

	const float MinZoom = 0.5f;
	const float MaxZoom = 5f;
	const float ZoomFactor = 1.25f;

	public override void _UnhandledInput(InputEvent evt)
	{
		if (evt is InputEventMouseButton mouseEvent) {
			// Left hold to drag
			if (mouseEvent.ButtonIndex == (int)ButtonList.Left) {
				if (mouseEvent.IsPressed()) {
					LastMousePosition = mouseEvent.Position;
					Dragging = true;
				} else {
					Dragging = false;
				}
			}

			// Scroll to zoom
			// if (mouseEvent.IsPressed()) {
			// 	if (mouseEvent.ButtonIndex == (int)ButtonList.WheelDown) {
			// 		Zoom *= ZoomFactor;

			// 		if (Zoom.x > MaxZoom) {
			// 			Zoom = new Vector2(MaxZoom, MaxZoom);
			// 		} 
			// 	} else if (mouseEvent.ButtonIndex == (int)ButtonList.WheelUp) {
			// 		Zoom /= ZoomFactor;

			// 		if (Zoom.x < MinZoom) {
			// 			Zoom = new Vector2(MinZoom, MinZoom);
			// 		}
			// 	}
			// }

			// TODO zoom to cursor, maybe

			
		} else if (Dragging && evt is InputEventMouseMotion motionEvent) {
			Position += (LastMousePosition - motionEvent.Position) * Zoom;
			LastMousePosition = motionEvent.Position;
		}
	}
}
