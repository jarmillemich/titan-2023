using Godot;
using System;
using System.Collections.Generic;

public class InventoryUI : HBoxContainer
{

    private Godot.Collections.Dictionary<string, int> pendingBuilding => GetNode<BuildingData>("/root/BuildingData").pendingBuilding;
public Godot.Collections.Dictionary<string, BuildingSpecs> buildings => GetNode<BuildingData>("/root/BuildingData").buildings;

    public override void _Ready()
    {
        Button templateButton = (Button)GetNode("Inventory").GetChild(0);
        for(int i = 1; i < pendingBuilding.Count; i ++){
            templateButton.Name = "btnInventory"+i.ToString();
            GetNode("Inventory").AddChild(templateButton.Duplicate());
        }
        foreach (KeyValuePair<string, int> valuePair in pendingBuilding)
        {
            
            buildings.TryGetValue(valuePair.Key,out BuildingSpecs specs);
            Button s = (Button)GetNode("Inventory").GetChild((int)specs.displayOrder-1);
            ((Label)s.GetNode("HBoxContainer/Label")).Text = valuePair.Value.ToString();
            s.Icon =  (Texture)GD.Load(specs.buildingDesign.spritePath);
            s.IconAlign = Button.TextAlign.Center;
            s.ExpandIcon = true;
        }
     
    }

    //  // Called every frame. 'delta' is the elapsed time since the previous frame.
    //  public override void _Process(float delta)
    //  {
    //      
    //  }
}
