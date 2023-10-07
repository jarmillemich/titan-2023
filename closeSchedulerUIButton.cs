using Godot;
using System;
using System.Diagnostics;

public class closeSchedulerUIButton : Button
{
	private void ToggleMenu()
	{
		GD.Print("hello");
		GetNode<VBoxContainer>("%CargoContainer").Visible = !GetNode<VBoxContainer>("%CargoContainer").Visible;
		GetNode<VBoxContainer>("%BuildingMenu").Visible = !GetNode<VBoxContainer>("%BuildingMenu").Visible;
		if(this.Text == "X")
			this.Text = "Cargo Scheduler >";
		else
			this.Text = "X";
	}
}
