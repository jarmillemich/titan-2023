using Godot;
using System;
using System.Collections.Generic;

public class UI : Control
{
    // Declare member variables here. Examples:
    // private int a = 2;
    // private string b = "text";

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        gameState.Connect(nameof(GameState.OnPhaseChanged), this, nameof(OnPhaseChange));
    }

    private GameState gameState => GetNode<GameState>("/root/GameState");

    private Button endTurnButton => GetNode<Button>("EndTurnButton");

    [Signal]
    public delegate void OnEndTurn();

    [Signal]
    public delegate void OnBuild(string buildingId);

    public void OnStartBuilding(List<string> available)
    {
        // TODO Alex & Nick
        Godot.Collections.Array buildinglist = GetNode("InventoryUI/Inventory").GetChildren();
        GD.Print(available.Count);
        if (available.Count > 0)
        {
            for (int j = 0; j < available.Count; j++)
            {
                for (int i = 0; i < buildinglist.Count; i++)
                {
                    GD.Print(((Button)buildinglist[i]).Name);
                    if (((Button)buildinglist[i]).Name.Contains(available[j]))
                    {
                        ((Button)buildinglist[i]).Disabled = false;
                    }
                }
            }
            ((HBoxContainer)GetNode("InventoryUI")).Visible = true;
        }
    }

    private void _on_EndTurnButton_pressed()
    {
        EmitSignal(nameof(OnEndTurn));
    }

    public void OnPhaseChange()
    {
        // TODO open/close various UI elements appropriately
        endTurnButton.Visible = gameState.Phase == Phase.InTurn;
    }

    public void _on_AboutButton_pressed()
    {
        GetNode<Control>("InfoBox").Visible = true;
    }

    public void _on_InfoCloseButton_pressed()
    {
        GetNode<Control>("InfoBox").Visible = false;
    }

    //  // Called every frame. 'delta' is the elapsed time since the previous frame.
    //  public override void _Process(float delta)
    //  {
    //      
    //  }
}
