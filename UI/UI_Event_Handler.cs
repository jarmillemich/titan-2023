using Godot;
using System;
using System.Collections.Generic;
using System.Diagnostics;

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
	List<String> receivedBuildings = new List<String>();
	public override void _Ready()
	{
		for (int i = 0; i < 10; i++)
		{
			Schedule.Add(new List<String>()); //days gotta exist
		}
	}
	public void UpdateGameStateLabel()
	{
		((Label)GetNode("../GameStateLabel")).Text = "Game State: " + GetNode<GameState>("/root/GameState").Phase.ToString();
	}
	private void PreviewBuilding(string buildingName)
	{
		buildings.TryGetValue(buildingName, out BuildingSpecs specs);
		VBoxContainer preview = GetNode<VBoxContainer>("%BuildingPreview");
		preview.Visible = true;
		((RichTextLabel)preview.GetNode("Description")).Text = specs.description;
		RichTextLabel prosume = (RichTextLabel)preview.GetNode("HBoxContainer2/ProsumeLabel");
		prosume.Text = specs.ProsumeToString();

		RichTextLabel capacity = (RichTextLabel)preview.GetNode("HBoxContainer2/CapacityLabel");
		capacity.Text = "Cargo Space: " + specs.cargoSpace;
	}

	private void ClosePreview()
	{
		GetNode<VBoxContainer>("%BuildingPreview").Visible = false;
	}
	private void highlightDay(int day) //this is NOT current day; it is always 0 - 7
	{
		GetNode<Button>("/root/Map/CanvasLayer/Control/CargoUI/CargoContainer/HBoxContainer3/Button" + highlightedDay).Modulate = new Color(1, 1, 1);
		highlightedDay = day;
		GetNode<Button>("/root/Map/CanvasLayer/Control/CargoUI/CargoContainer/HBoxContainer3/Button" + highlightedDay).Modulate = new Color(0, 1, 0);
	}

	public void increaseTurn()
	{
		daysPassed++;
		Schedule.Add(new List<string>());
		for(int i = 0; i < 8; i ++){
			//update each day to be 3077 + (daysPassed * i * 7);
			int newyear = 3070 + (daysPassed * (i + 1) * 7);
			GetNode<Button>("/root/Map/CanvasLayer/Control/CargoUI/CargoContainer/HBoxContainer3/Button" + i).Text = "Y" + newyear;
		}
		//move each day forward by one
			receivedBuildings = new List<string>();
			int tilesRemoved = 0;
			int? tilesExpected = 1;
			while(Schedule[0].Count > 0){
				//grab index 0, remove as many as needed, repeat
				receivedBuildings.Add(Schedule[0][0]);
				tilesRemoved = 0;
				tilesExpected = buildings[Schedule[0][0]].cargoSpace;
				for(int i = 0; i < tilesExpected; i++){
					Schedule[0].RemoveAt(0);
					tilesRemoved++;
				}
			}
			GetNode<InventoryUI>("../InventoryUI").AddNextTurnInventory(receivedBuildings);
			Schedule.RemoveAt(0);
			for(int day = 0; day < 8; day++){
			for (int i = 0; i < 10; i++){
			Button slot = GetNode<Button>("/root/Map/CanvasLayer/Control/CargoUI/CargoContainer/HBoxContainer/VBoxContainer" + day + "/Button" + i);
			if (Schedule[day].Count > i){
				slot.Icon = (Texture)GD.Load(buildings[Schedule[day][i]].buildingDesign.spritePath);
				slot.Modulate = new Color(buildings[Schedule[day][i]].cargoHex);
				slot.Text = "";
			} else {
				slot.Modulate = new Color(1, 1, 1);
				slot.Icon = null;
				slot.Text = "";
			}
		}
		ProgressBar PercentageFilled = GetNode<ProgressBar>("/root/Map/CanvasLayer/Control/CargoUI/CargoContainer/HBoxContainer2/ProgressBar" + day);
		PercentageFilled.Value = Schedule[day].Count * 10.0;
	}
	}

	private bool CheckIfScheduleable()
	{
		return true;
	}

	private void ScheduleBuilding(string building)
	{
		if (cargoMax - Schedule[highlightedDay].Count < buildings[building].cargoSpace)
		{
			return; //if available room < required cargo... no.
		}
		for (int i = 0; i < buildings[building].cargoSpace; i++)
		{
			Button slot = GetNode<Button>("/root/Map/CanvasLayer/Control/CargoUI/CargoContainer/HBoxContainer/VBoxContainer" + highlightedDay + "/Button" + Schedule[highlightedDay].Count);
			Schedule[highlightedDay].Add(building);
			slot.Icon = (Texture)GD.Load(buildings[building].buildingDesign.spritePath);
			slot.Text = "";
			slot.Modulate = new Color(buildings[building].cargoHex);
		}
			ProgressBar PercentageFilled = GetNode<ProgressBar>("/root/Map/CanvasLayer/Control/CargoUI/CargoContainer/HBoxContainer2/ProgressBar" + highlightedDay);
			PercentageFilled.Value = Schedule[highlightedDay].Count * 10.0;
			GD.Print(Schedule[highlightedDay].Count * 10.0);
	}

	private void CancelBuilding(int index)
	{
		if(Schedule[highlightedDay].Count <= index){
			return;
		}

		int tilesRemoved = 0;
		string building = Schedule[highlightedDay][index];
		for (int i = Schedule[highlightedDay].Count - 1; i > -1; i--)
		{
			if (Schedule[highlightedDay][i] == building)
			{
				Schedule[highlightedDay].RemoveAt(i);
				tilesRemoved++;
				if (tilesRemoved == buildings[building].cargoSpace)
				{
					i = -2; //if they have two of the same building, we only want to cancel one
				}
			}
		}
		ProgressBar PercentageFilled = GetNode<ProgressBar>("/root/Map/CanvasLayer/Control/CargoUI/CargoContainer/HBoxContainer2/ProgressBar" + highlightedDay);
		PercentageFilled.Value = Schedule[highlightedDay].Count / 10.0;
		//reset sprites
		for (int i = 0; i < 10; i++){
			Button slot = GetNode<Button>("/root/Map/CanvasLayer/Control/CargoUI/CargoContainer/HBoxContainer/VBoxContainer" + highlightedDay + "/Button" + i);
			if (Schedule[highlightedDay].Count > i){
				slot.Icon = (Texture)GD.Load(buildings[Schedule[highlightedDay][i]].buildingDesign.spritePath);
				slot.Modulate = new Color(buildings[Schedule[highlightedDay][i]].cargoHex);
				slot.Text = "";
			} else {
				slot.Modulate = new Color(1, 1, 1);
				slot.Icon = null;
				slot.Text = "";
			}
		}
	}
}
