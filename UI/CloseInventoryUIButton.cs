using Godot;
using System;

public class CloseInventoryUIButton : Button
{
	private void ToggleMenu()
	{
		GD.Print("hello");
		GetNode<GridContainer>("../%Inventory").Visible = !GetNode<GridContainer>("../%Inventory").Visible;
		if(this.Text == "X")
			this.Text = "< Received Buildings";
		else
			this.Text = "X";
	}
}
