using Godot;
using System;
using System.Collections.Generic;

public class UI_Event_Handler : Node
{
			//Building Types:
		//1:	Farm
		//2:
		//3:
		//...
	private Godot.Collections.Dictionary<string, int> pendingBuilding => GetNode<BuildingData>("/root/BuildingData").pendingBuilding;
	private Godot.Collections.Dictionary<string, BuildingSpecs> buildings => GetNode<BuildingData>("/root/BuildingData").buildings;
	int cargoMax = 10; //The max slots available per day
	int highlightedDay = 0; //This is set from selecting a day in the UI.
	int daysPassed = 0; //this could also be called turn count. Modified externally
	List<List<String>> Schedule = new List<List<String>>();
	//The schedule: outer list selects the day, inner list selects the cargo on that day, named the building it is related to
	//ex: access the 4th day's 6th cargo slot: Schedule[4][6]
	public override void _Ready()
	{
	 for(int i = 0; i < 10; i++){
		Schedule.Add(new List<String>()); //days gotta exist
	 }   
	}
	private void highlightDay(int day){
		highlightedDay = day;
	}

	private void increaseTurn(){
		daysPassed++;
		Schedule.Add(new List<string>());
	}

	private bool CheckIfScheduleable(){
		return true;
	}

	private void ScheduleBuilding(string building)
	{
		if(cargoMax - Schedule[highlightedDay].Count < buildings[building].cargoSpace){
			return; //if available room < required cargo... no.
		}
		int tilesAdded = 0;
		for(int i = 0; i < buildings[building].cargoSpace; i++){
			Button slot = GetNode<Button>("/root/Map/CanvasLayer/Control/CargoUI/CargoContainer/HBoxContainer/VBoxContainer" + highlightedDay + "/Button" + Schedule[highlightedDay].Count); 
			Schedule[highlightedDay].Add(building);
			slot.Icon = (Texture)GD.Load(buildings[building].buildingDesign.spritePath);
			slot.Text = "TEST";
			//slot.Modulate = Color(buildings[building].cargoHex);
		}
	}

	private void CancelBuilding(int day, string building){
		int tilesRemoved = 0;
		for(int i = Schedule[day].Count - 1; i > -1; i--){
			if(Schedule[day][i] == building){
				//change the sprites n stuff here!
				Schedule[day].RemoveAt(i);
				tilesRemoved++;
				if(tilesRemoved == buildings[building].cargoSpace){
					return; //if they have two of the same building, we only want to cancel one
				}
			}
		}
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
