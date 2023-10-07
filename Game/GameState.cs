using Godot;
using System;

public enum Phase {
    Scouting,
    
    CargoDrop,
    Income,
    InTurn,
    MovingRover,

}

public class GameState : Node
{
    public Phase Phase {
        get => _phase;
        set {
            _phase = value;
            EmitSignal(nameof(OnPhaseChanged));
        }
    }
    [Signal]
    public delegate void OnPhaseChanged();

    private Phase _phase = Phase.Scouting;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        
    }

//  // Called every frame. 'delta' is the elapsed time since the previous frame.
//  public override void _Process(float delta)
//  {
//      
//  }
}
