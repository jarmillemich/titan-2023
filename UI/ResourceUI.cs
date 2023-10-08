using Godot;
using System;
using System.Collections.Generic;

public class ResourceUI : HBoxContainer
{
    // Declare member variables here. Examples:
    // private int a = 2;
    // private string b = "text";
    List<string> MaterialColumns = new List<string> { "Produce", "Consume", "Storage", };
    List<string> MaterialRows = new List<string> { "Water", "Energy", "Food" };
    // List<string> AvailabilityColumns = new List<string> { "Amount", "Assigned", "Available" };
    // List<string> AvailabilityRows = new List<string> { "Workers", "Rovers", "Mountain Samples", "Lake Samples", "Volcano Samples", "Crater Samples" };

    // Called when the node enters the scene tree for the first time.
    private void LoadGrid(string templatePath, string containerPath, List<string> resourceRow, List<string> resourceColumn)
    {
        Label templateNode = (Label)GetNode(templatePath).GetChild(0);
        for (int x = -1; x < resourceRow.Count; x++)
        {
            for (int y = -1; y < resourceColumn.Count; y++)
            {
                if (x == -1 && y == -1)
                {
                    templateNode.Name = "Blank";
                    templateNode.Text = "";

                }
                else
                {
                    Label dupTemplate = (Label)templateNode.Duplicate();
                    if (x == -1)
                    {
                        dupTemplate.Name = "ColumnHeader_" + resourceColumn[y].Replace(" ", "_");
                        dupTemplate.Text = resourceColumn[y];
                    }
                    else if (y == -1)
                    {
                        dupTemplate.Name = "RowHeader_" + resourceRow[x].Replace(" ", "_");
                        dupTemplate.Text = resourceRow[x];
                    }
                    else
                    {
                        dupTemplate.Name = resourceRow[x].Replace(" ", "_") + "_" + resourceColumn[y].Replace(" ", "_");
                        dupTemplate.Align = Label.AlignEnum.Center;
                        dupTemplate.Text = "0";
                    }
                    GetNode(containerPath).AddChild(dupTemplate);
                }
            }
        }
    }
    public override void _Ready()
    {
LoadGrid("VBoxContainer/MaterialResourcePanel/MaterialResourceContainer","VBoxContainer/MaterialResourcePanel/MaterialResourceContainer",MaterialRows,MaterialColumns);
// LoadGrid("VBoxContainer/AvailabilityResourcePanel/AvailabilityResourceContainer","VBoxContainer/AvailabilityResourcePanel/AvailabilityResourceContainer",AvailabilityRows,AvailabilityColumns);

    }

    public void ReloadGridValues()
    {

    }
    //  // Called every frame. 'delta' is the elapsed time since the previous frame.
    //  public override void _Process(float delta)
    //  {
    //      
    //  }
}
