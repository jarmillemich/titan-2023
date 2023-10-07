using Godot;
using System;

[Tool]
public class MaterialResource : Node {
    [Export]
    public int Amount { get; set; } = 0;
    
    [Export]
    public int Capacity { get; set; } = 0;

    [Export]
    public int Income { get; set; } = 0;
}

[Tool]
public class AvailabilityResource : Node {
    [Export]
    public int Amount { get; set; } = 0;

    [Export]
    public int Assigned { get; set; } = 0;

    public int Available => Amount - Assigned;
}

public class Resources : Node
{

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        
    }

    public MaterialResource Energy { get; set; } = new MaterialResource();
    public MaterialResource Water { get; set; } = new MaterialResource();
    public MaterialResource Food { get; set; } = new MaterialResource();

    public AvailabilityResource Workers { get; set; } = new AvailabilityResource();
    public AvailabilityResource MountainSamples { get; set; } = new AvailabilityResource();
    public AvailabilityResource LakeSamples { get; set; } = new AvailabilityResource();
    public AvailabilityResource VolcanoSamples { get; set; } = new AvailabilityResource();
    public AvailabilityResource CraterSamples { get; set; } = new AvailabilityResource();
    
}
