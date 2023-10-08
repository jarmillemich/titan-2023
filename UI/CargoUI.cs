using Godot;
using System;
using System.Collections.Generic;

public class CargoUI : HBoxContainer
{
	private Godot.Collections.Dictionary<string, int> pendingBuilding => GetNode<BuildingData>("/root/BuildingData").pendingBuilding;
	public Godot.Collections.Dictionary<string, BuildingSpecs> buildings => GetNode<BuildingData>("/root/BuildingData").buildings;

	public override void _Ready()
	{
		Button templateButton = (Button)GetNode(@"BuildingMenu/BuildingList").GetChild(0);
		int i = 0;
		foreach (KeyValuePair<string, int> keyValue in pendingBuilding)
		{
			if (i == 0)
			{
				templateButton.Name = "btnBuilding" + keyValue.Key.ToString();
				i++;
			}
			else
			{
				Button dupButton = (Button)templateButton.Duplicate();
				dupButton.Name = "btnBuilding" + keyValue.Key.ToString();
				GetNode(@"BuildingMenu/BuildingList").AddChild(dupButton);
			}
		}
		LoadBuildings();

	}
	private void LoadBuildings()
	{
		foreach (KeyValuePair<string, int> valuePair in pendingBuilding)
		{

            buildings.TryGetValue(valuePair.Key, out BuildingSpecs specs);
            // var infobutton = GetNode<Button>(@"BuildingMenu/BuildingList/Button");
            // infobutton.Connect("button_down",GetNode("../InfoBox"), "PreviewBuilding", new Godot.Collections.Array { valuePair.Key });

            Button s = (Button)GetNode(@"BuildingMenu/BuildingList").GetChild((int)specs.displayOrder - 1);
            //disable all buttons
            //s.Disabled = true;
            s.Icon = (Texture)GD.Load(specs.buildingDesign.spritePath);
            s.IconAlign = Button.TextAlign.Center;
            s.ExpandIcon = true;
            s.Connect("mouse_entered", GetNode("../UIEventHandler"), "PreviewBuilding", new Godot.Collections.Array { valuePair.Key });
            s.Connect("mouse_exited", GetNode("../UIEventHandler"), "ClosePreview");
            s.Connect("button_down", GetNode("../UIEventHandler"), "ScheduleBuilding", new Godot.Collections.Array { valuePair.Key });

                    }

    }
    
}
