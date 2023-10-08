using Godot;
using System;
using System.Collections.Generic;

public class InventoryUI : HBoxContainer
{

    private Godot.Collections.Dictionary<string, int> pendingBuilding => GetNode<BuildingData>("/root/BuildingData").pendingBuilding;
    public Godot.Collections.Dictionary<string, BuildingSpecs> buildings => GetNode<BuildingData>("/root/BuildingData").buildings;


    public void EnableButtons(Godot.Collections.Array buildingNames)
    {
        for (int i = 0; i < buildingNames.Count; i++)
        {
            foreach (Button b in GetNode("Inventory").GetChildren())
            {
                if (b.Name.Contains(buildingNames[i].ToString()))
                {
                    b.Disabled = false;
                }
            }

        }
    }
    public override void _Ready()
    {
        Button templateButton = (Button)GetNode("Inventory").GetChild(0);
        int i = 0;
        foreach (KeyValuePair<string, int> keyValue in pendingBuilding)
        {
            if (i == 0)
            {
                templateButton.Name = "btnInventory" + keyValue.Key.ToString();
                i++;
            }
            else
            {
                Button dupButton = (Button)templateButton.Duplicate();
                dupButton.Name = "btnInventory" + keyValue.Key.ToString();
                GetNode("Inventory").AddChild(dupButton);
            }
        }
        LoadInventory(false);
    }
    private void LoadInventory(bool Reload)
    {
        foreach (KeyValuePair<string, int> valuePair in pendingBuilding)
        {

            buildings.TryGetValue(valuePair.Key, out BuildingSpecs specs);
            Button s = (Button)GetNode("Inventory").GetChild((int)specs.displayOrder - 1);
            ((Label)s.GetNode("HBoxContainer/Label")).Text = valuePair.Value.ToString();
            //disable all buttons
            s.Disabled = true;
            if (!Reload)
            {
                s.Icon = (Texture)GD.Load(specs.buildingDesign.spritePath);
                s.IconAlign = Button.TextAlign.Center;
                s.ExpandIcon = true;
                s.Connect("mouse_entered", GetNode("../UIEventHandler"), "PreviewBuilding", new Godot.Collections.Array { valuePair.Key });
                s.Connect("mouse_exited", GetNode("../UIEventHandler"), "ClosePreview");
                s.Connect("pressed", this, "_on_InventoryButton_pressed", new Godot.Collections.Array { valuePair.Key });
            }
        }
    }
    [Signal]
    public delegate void OnInventoryButton(string buildingName);

    public void _on_InventoryButton_pressed(string buildingName)
    {
        GetNode<Resources>("/root/Resources").BuildingBuilt(buildingName);
        LoadInventory(true);
        ((Button)GetNode("GridContainer").GetChild(0)).EmitSignal("button_down");
        EmitSignal(nameof(OnInventoryButton), buildingName);
    }
    //  // Called every frame. 'delta' is the elapsed time since the previous frame.
    //  public override void _Process(float delta)
    //  {
    //      
    //  }
}
