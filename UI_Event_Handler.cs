using Godot;
using System;

public class UI_Event_Handler : Node
{
			//Building Types:
		//1:	Farm
		//2:
		//3:
		//...

	private void ScheduleBuilding(int BuildingType, int BuildingLevel)
	{
		
		//Schedule building logic...
		//pull from json
		//check if we have enough tiles to schedule
		//if not, put to the next available schedule?
		//	or, let them select where 

		GD.Print("Button Pressed!");
	}

	private void PreviewBuilding(int BuildingType, int BuildingLevel)
{
	VBoxContainer preview = GetNode<VBoxContainer>("%BuildingPreview");
	preview.Visible = true;
	preview.GetNode<RichTextLabel>("%Description").Text = "Building Type: " + BuildingType + " Building Level: " + BuildingLevel;
}


private void ClosePreview()
{
	GetNode<VBoxContainer>("%BuildingPreview").Visible = false;
}

}
