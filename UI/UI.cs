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

	private Button CargoBuildingMenu => GetNode<Button>("/root/Map/CanvasLayer/Control/CargoUI/GridContainer/Button");
	
	private Button Inventory => GetNode<Button>("/root/Map/CanvasLayer/Control/InventoryUI/GridContainer/CloseMenuButton");
	
	private Button ResourceUI => GetNode<Button>("/root/Map/CanvasLayer/Control/ResourceUI/GridContainer/Button");
	private Node UIEventHandler => GetNode<Node>("/root/Map/CanvasLayer/Control/UIEventHandler");

	[Signal]
	public delegate void OnEndTurn();

	[Signal]
	public delegate void OnBuild(string buildingId);

    public void OnStartBuilding(List<string> available)
    {
        // TODO Alex & Nick
        Godot.Collections.Array buildinglist = GetNode("InventoryUI/Inventory").GetChildren();
        if (available.Count > 0)
        {
            for (int j = 0; j < available.Count; j++)
            {
                for (int i = 0; i < buildinglist.Count; i++)
                {
                    if (((Button)buildinglist[i]).Name.Contains(available[j]))
                    {
                        ((Button)buildinglist[i]).Disabled = false;
                    }
                }
            }
            ((GridContainer)GetNode("InventoryUI/Inventory")).Visible = true;
        }
    }
private void _on_InventoryUI_OnInventoryButton(string buildingName){
    EmitSignal(nameof(OnBuild),buildingName);
}
    private void _on_EndTurnButton_pressed()
    {
        UIEventHandler.Call("increaseTurn");
        EmitSignal(nameof(OnEndTurn));
    }

	public void OnPhaseChange() {
		// TODO open/close various UI elements appropriately
		endTurnButton.Visible = gameState.Phase == Phase.InTurn;

		if (gameState.Phase == Phase.CargoDrop) {
			if(CargoBuildingMenu.Text != "X")
				CargoBuildingMenu.Call("ToggleMenu");
			if(Inventory.Text == "X")
				Inventory.Call("ToggleMenu");
			if(ResourceUI.Text == "X")
				ResourceUI.Call("ToggleMenu");
		}

		if (gameState.Phase == Phase.DecidingOnBuildings) {
			if(CargoBuildingMenu.Text == "X")
				CargoBuildingMenu.Call("ToggleMenu");
			if(Inventory.Text != "X")
				Inventory.Call("ToggleMenu");
			if(ResourceUI.Text == "X")
				ResourceUI.Call("ToggleMenu");
		}

		if (gameState.Phase == Phase.Income) {
			if(CargoBuildingMenu.Text == "X")
				CargoBuildingMenu.Call("ToggleMenu");
			if(Inventory.Text == "X")
				Inventory.Call("ToggleMenu");
			if(ResourceUI.Text != "X")
				ResourceUI.Call("ToggleMenu");
			System.Threading.Thread.Sleep(3000);
		}

		if (gameState.Phase == Phase.InTurn) {
			if(CargoBuildingMenu.Text == "X")
				CargoBuildingMenu.Call("ToggleMenu");
			if(Inventory.Text == "X")
				Inventory.Call("ToggleMenu");
			if(ResourceUI.Text == "X")
				ResourceUI.Call("ToggleMenu");
		}

		if (gameState.Phase == Phase.MovingRover) {
			if(CargoBuildingMenu.Text == "X")
				CargoBuildingMenu.Call("ToggleMenu");
			if(Inventory.Text == "X")
				Inventory.Call("ToggleMenu");
			if(ResourceUI.Text == "X")
				ResourceUI.Call("ToggleMenu");
			System.Threading.Thread.Sleep(3000);
		}

		if (gameState.Phase == Phase.Scouting) {
			if(CargoBuildingMenu.Text == "X")
				CargoBuildingMenu.Call("ToggleMenu");
			if(Inventory.Text == "X")
				Inventory.Call("ToggleMenu");
			if(ResourceUI.Text == "X")
				ResourceUI.Call("ToggleMenu");
		}

		if (gameState.Phase == Phase.TargettingRover) {
			if(CargoBuildingMenu.Text == "X")
				CargoBuildingMenu.Call("ToggleMenu");
			if(Inventory.Text == "X")
				Inventory.Call("ToggleMenu");
			if(ResourceUI.Text == "X")
				ResourceUI.Call("ToggleMenu");
		}
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
