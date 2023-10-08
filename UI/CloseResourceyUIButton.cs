using Godot;
using System;

public class CloseResourceyUIButton : Button
{

	public void ToggleMenu()
	{
		GetNode<VBoxContainer>("../../VBoxContainer").Visible = !GetNode<VBoxContainer>("../../VBoxContainer").Visible;
		if (this.Text == "X")
			this.Text = "Resources >";
		else
		{
			this.Text = "X";
		}
	}
}
