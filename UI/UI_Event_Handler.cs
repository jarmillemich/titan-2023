using Godot;
using System;

public class UI_Event_Handler : Node
{
			//Building Types:
		//1:	Farm
		//2:
		//3:
		//...
	private Godot.Collections.Dictionary<string, int> pendingBuilding => GetNode<BuildingData>("/root/BuildingData").pendingBuilding;
	private Godot.Collections.Dictionary<string, BuildingSpecs> buildings => GetNode<BuildingData>("/root/BuildingData").buildings;
  

	private void ScheduleBuilding(string building)
	{
		
		//Schedule building logic...
		//pull from json
		//check if we have enough tiles to schedule
		//if not, put to the next available schedule?
		//	or, let them select where 

		GD.Print("Button Pressed!");
	}

	private void PreviewBuilding(string building)
{
	VBoxContainer preview = GetNode<VBoxContainer>("%BuildingPreview");
	preview.Visible = true;
//	preview.GetNode<RichTextLabel>("%Description").Text = "Building Type: " + BuildingType + " Building Level: " + BuildingLevel;
//	preview.GetNode<TextureRect>("")

//	buildings
}


private void ClosePreview()
{
	GetNode<VBoxContainer>("%BuildingPreview").Visible = false;
}

}
