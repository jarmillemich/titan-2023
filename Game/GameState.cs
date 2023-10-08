using Godot;

public enum Phase {
    Scouting,
    
    CargoDrop,
    Income,
    MovingRover,

    InTurn,
    DecidingOnBuildings,
    TargettingRover,

}

public class GameState : Node
{
    public Phase Phase {
        get => _phase;
        set {
            _phase = value;
            GetNode<UI_Event_Handler>("/root/Map/CanvasLayer/Control/UIEventHandler").UpdateGameStateLabel();
            EmitSignal(nameof(OnPhaseChanged));
        }
    }

    [Signal]
    public delegate void OnPhaseChanged();

    private Phase _phase = Phase.Scouting;

    public int RemainingProbes { get; set; } = 3;

    /// Current rover we want to move
    public HexPoint RoverStartPoint { get; set; }

    /// Tile we have selected for building
    public HexPoint SelectedTile { get; set; }

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        
    }
}
