using Godot;
using System;

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

    private void _on_EndTurnButton_pressed() {
        EmitSignal(nameof(OnEndTurn));
    }

    public void OnPhaseChange() {
        // TODO open/close various UI elements appropriately
        endTurnButton.Visible = gameState.Phase == Phase.InTurn;
    }

//  // Called every frame. 'delta' is the elapsed time since the previous frame.
//  public override void _Process(float delta)
//  {
//      
//  }
}
