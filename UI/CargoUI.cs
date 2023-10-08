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
            Button s = (Button)GetNode(@"BuildingMenu/BuildingList").GetChild((int)specs.displayOrder - 1);
            //disable all buttons
            //s.Disabled = true;
            s.Icon = (Texture)GD.Load(specs.buildingDesign.spritePath);
            s.IconAlign = Button.TextAlign.Center;
            s.ExpandIcon = true;
            s.Connect("mouse_entered", this, "PreviewBuilding", new Godot.Collections.Array { valuePair.Key });
            s.Connect("mouse_exited", this, "ClosePreview");
            s.Connect("button_down", GetNode("../UIEventHandler"), "ScheduleBuilding", new Godot.Collections.Array { valuePair.Key });
        }
    }
    private void PreviewBuilding(string buildingName)
    {
        buildings.TryGetValue(buildingName, out BuildingSpecs specs);
        VBoxContainer preview = GetNode<VBoxContainer>("%BuildingPreview");
        preview.Visible = true;
        ((RichTextLabel)preview.GetNode("Description")).Text = specs.description;
        RichTextLabel prosume = (RichTextLabel)preview.GetNode("HBoxContainer2/ProsumeLabel");
        prosume.Text = specs.ProsumeToString();
        //prosume.RectSize = new Vector2(215f, 75f);
        //prosume.AnchorRight = 0.25f;


        RichTextLabel capacity = (RichTextLabel)preview.GetNode("HBoxContainer2/CapacityLabel");
        capacity.Text = "Cargo Space: " + specs.cargoSpace;
        //capacity.RectSize = new Vector2(215f, 75f);
        //capacity.AnchorLeft = 0.25f;
        //capacity.AnchorRight = 0.5f;
        //	preview.GetNode<RichTextLabel>("%Description").Text = "Building Type: " + BuildingType + " Building Level: " + BuildingLevel;
        //	preview.GetNode<TextureRect>("")

        //	buildings
    }


    private void ClosePreview()
    {
        GetNode<VBoxContainer>("%BuildingPreview").Visible = false;
    }
}
