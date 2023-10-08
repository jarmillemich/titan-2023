using Godot;
using System;

public class CloseInventoryUIButton : Button
{
	public void ToggleMenu()
	{
		GetNode<GridContainer>("../%Inventory").Visible = !GetNode<GridContainer>("../%Inventory").Visible;
		if(this.Text == "X")
			this.Text = "< Received Buildings";
		else
			this.Text = "X";
	}
}
