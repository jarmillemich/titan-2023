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
	public void CalcResource()
	{
		//Add resources
		Energy.Amount += Energy.Income;
		Water.Amount += Water.Income;
		Food.Amount += Food.Income;
		//Subtract resources
		Godot.Collections.Dictionary<string, BuildingSpecs> buildings2 =
	 (Godot.Collections.Dictionary<string, BuildingSpecs>)from built in builtBuilding
														  join builds in buildings on built.Key.ToString() equals buildings.Keys.ToString()
														  where built.Value > 0
														  select builds;

		foreach (BuildingSpecs builds in buildings2.Values)
		{
			List<BuildingProsume> prosume = builds.buildingProsume;
			for (int i = 0; i < prosume.Count(); i++)
			{
				switch (prosume[i].type)
				{
					case "Food":
						if (prosume[i].function == "consume")
							Food.Amount -= prosume[i].amount;
						break;
					case "Water":
						if (prosume[i].function == "consume")
							Water.Amount -= prosume[i].amount;
						break;
					case "Energy":
						if (prosume[i].function == "consume")
							Energy.Amount -= prosume[i].amount;
						break;
				}
			}
		}
		//If resources are negative then set to 0
		if (Energy.Amount < 0)
		{
			Energy.Amount = 0;
		}
		if (Water.Amount < 0)
		{
			Water.Amount = 0;
		}
		if (Food.Amount < 0)
		{
			Food.Amount = 0;
		}
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
	public void BuildingBuilt(string buildingName)
	{
		if (builtBuilding.ContainsKey(buildingName))
		{
			builtBuilding[buildingName] = builtBuilding[buildingName] + 1;
			pendingBuilding[buildingName] = pendingBuilding[buildingName] - 1;

			//Adjust capacity, storage, produce
			List<BuildingProsume> prosume = buildings[buildingName].buildingProsume;
			for (int i = 0; i < prosume.Count(); i++)
			{
				switch (prosume[i].type)
				{
					case "Food":
						switch (prosume[i].function)
						{
							case "produce":
								Food.Income += prosume[i].amount;
								break;
							case "provideStorage":
								Food.Capacity += prosume[i].amount;
								break;
						}
						break;
					case "Water":
						switch (prosume[i].function)
						{
							case "produce":
								Water.Income += prosume[i].amount;
								break;
							case "provideStorage":
								Water.Capacity += prosume[i].amount;
								break;
						}
						break;
					case "Energy":
						switch (prosume[i].function)
						{
							case "produce":
								Energy.Income += prosume[i].amount;
								break;
							case "provideStorage":
								Energy.Capacity += prosume[i].amount;
								break;
						}
						break;

					case "Workers":
						switch (prosume[i].function)
						{
							case "provideCapacity":
								Workers.Amount += prosume[i].amount;
								break;
						}
						break;

					case "Rovers":
						switch (prosume[i].function)
						{
							case "provideCapacity":
								Rovers.Amount += prosume[i].amount;
								break;
						}
						break;
				}
			}
		}
		else
		{
			GD.Print(buildingName, " does not exist in list");
		}
	}
	#endregion
	public MaterialResource Energy { get; set; } = new MaterialResource();
	public MaterialResource Water { get; set; } = new MaterialResource();
	public MaterialResource Food { get; set; } = new MaterialResource();

	public AvailabilityResource Workers { get; set; } = new AvailabilityResource();
	public AvailabilityResource Rovers { get; set; } = new AvailabilityResource();
	public AvailabilityResource MountainSamples { get; set; } = new AvailabilityResource();
	public AvailabilityResource LakeSamples { get; set; } = new AvailabilityResource();
	public AvailabilityResource VolcanoSamples { get; set; } = new AvailabilityResource();
	public AvailabilityResource CraterSamples { get; set; } = new AvailabilityResource();

}
