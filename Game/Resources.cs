using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

[Tool]
public class MaterialResource : Node
{
	[Export]
	public int Amount { get; set; } = 0;

	[Export]
	public int Capacity { get; set; } = 0;

	[Export]
	public int Income { get; set; } = 0;
}

[Tool]
public class AvailabilityResource : Node
{
	[Export]
	public int Amount { get; set; } = 0;

	[Export]
	public int Assigned { get; set; } = 0;

	public int Available => Amount - Assigned;
}

public class Resources : Node
{
	private Godot.Collections.Dictionary<string, int> pendingBuilding => GetNode<BuildingData>("/root/BuildingData").pendingBuilding;
	private Godot.Collections.Dictionary<string, int> builtBuilding => GetNode<BuildingData>("/root/BuildingData").builtBuilding;

	private Godot.Collections.Dictionary<string, BuildingSpecs> buildings => GetNode<BuildingData>("/root/BuildingData").buildings;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{

	}

	public MaterialResource GetResource(string name) {
		switch (name)
		{
			case "Energy":
				return Energy;
			case "Water":
				return Water;
			case "Food":
				return Food;
			default:
				return null;
		}
	}

	[Signal]
	public delegate void ResourceChanged();

	public void CalcResource()
	{
		//Add resources
		Energy.Amount += Energy.Income;
		Water.Amount += Water.Income;
		Food.Amount += Food.Income;

		
		//If resources are above capacity, set to capacity
		if (Energy.Amount > Energy.Capacity)
		{
			Energy.Amount = Energy.Capacity;
		}
		if (Water.Amount > Water.Capacity)
		{
			Water.Amount = Water.Capacity;
		}
		if (Food.Amount > Food.Capacity)
		{
			Food.Amount = Food.Capacity;
		}

		EmitSignal(nameof(ResourceChanged));
	}

	#region DictionaryAccess
	public void AddPendingBuild(string buildingName)
	{
		if (pendingBuilding.ContainsKey(buildingName))
		{
			pendingBuilding[buildingName] = pendingBuilding[buildingName] + 1;
		}
		else
		{
			GD.Print(buildingName, " does not exist in list");
		}
	}
	public void BuildingBuilt(string buildingName, int amount = 1)
	{
		if (builtBuilding.ContainsKey(buildingName))
		{
			builtBuilding[buildingName] = builtBuilding[buildingName] += amount;
			if (amount > 0) {
				pendingBuilding[buildingName] = pendingBuilding[buildingName] - 1;
			}

			//Adjust capacity, storage, produce
			List<BuildingProsume> prosume = buildings[buildingName].buildingProsume;
			for (int i = 0; i < prosume.Count(); i++)
			{
				GD.Print("Update production ", prosume[i].type, " ", prosume[i].function, " ", prosume[i].amount);
				switch (prosume[i].type)
				{
					case "Food":
						switch (prosume[i].function)
						{
							case "produce":
								Food.Income += amount * prosume[i].amount;
								break;
							case "provideStorage":
								Food.Capacity += amount * prosume[i].amount;
								break;
						}
						break;
					case "Water":
						switch (prosume[i].function)
						{
							case "produce":
								Water.Income += amount * prosume[i].amount;
								break;
							case "provideStorage":
								Water.Capacity += amount * prosume[i].amount;
								break;
						}
						break;
					case "Energy":
						switch (prosume[i].function)
						{
							case "produce":
								Energy.Income += amount * prosume[i].amount;
								break;
							case "provideStorage":
								Energy.Capacity += amount * prosume[i].amount;
								break;
						}
						break;

					// case "Workers":
					// 	switch (prosume[i].function)
					// 	{
					// 		case "provideCapacity":
					// 			Workers.Amount += amount * prosume[i].amount;
					// 			break;
					// 	}
					// 	break;

					// case "Rovers":
					// 	switch (prosume[i].function)
					// 	{
					// 		case "provideCapacity":
					// 			Rovers.Amount += amount * prosume[i].amount;
					// 			break;
					// 	}
					// 	break;
				}
			}
		}
		else
		{
			GD.Print(buildingName, " does not exist in list");
		}

		EmitSignal(nameof(ResourceChanged));
	}
	#endregion
	public MaterialResource Energy { get; set; } = new MaterialResource();
	public MaterialResource Water { get; set; } = new MaterialResource();
	public MaterialResource Food { get; set; } = new MaterialResource();

	// public AvailabilityResource Workers { get; set; } = new AvailabilityResource();
	// public AvailabilityResource Rovers { get; set; } = new AvailabilityResource();
	// public AvailabilityResource MountainSamples { get; set; } = new AvailabilityResource();
	// public AvailabilityResource LakeSamples { get; set; } = new AvailabilityResource();
	// public AvailabilityResource VolcanoSamples { get; set; } = new AvailabilityResource();
	// public AvailabilityResource CraterSamples { get; set; } = new AvailabilityResource();

}
