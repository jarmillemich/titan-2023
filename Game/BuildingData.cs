using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

public class BuildingData : Node
{

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		LoadBuildingSpecs();
		LoadBuiltPendinglist();
	}
	public Godot.Collections.Dictionary<string, BuildingSpecs> buildings = new Godot.Collections.Dictionary<string, BuildingSpecs>();

	public Godot.Collections.Dictionary<string, int> builtBuilding = new Godot.Collections.Dictionary<string, int>();
	public Godot.Collections.Dictionary<string, int> pendingBuilding = new Godot.Collections.Dictionary<string, int>();

	
	#region  LoadDictionaries
	public void LoadBuiltPendinglist()
	{
		foreach (var key in buildings.Keys)
		{
			builtBuilding.Add(key, 0);
			pendingBuilding.Add(key, 0);
		}
	}
	public void LoadBuildingSpecs()
	{
		string path = "Entities.json";
		var file = new File();

		if (file.FileExists(path))
		{
			GD.Print("got path");
			file.Open(path, File.ModeFlags.Read);
			JSONParseResult parsedFile = JSON.Parse(file.GetAsText());
			using (Godot.Collections.Dictionary dictFile = (Godot.Collections.Dictionary)parsedFile.Result)
			{

				foreach (var key in dictFile.Keys)
				{

					Godot.Collections.Dictionary keyValues = dictFile[key] as Godot.Collections.Dictionary;
					BuildingSpecs specs = new BuildingSpecs(
						keyValues.GetInt("level"),
						keyValues.GetString("type"),
						keyValues.GetString("placementType"),
						keyValues.GetInt("cargoSpace"),
						keyValues.GetString("cargoHex"),
						keyValues.GetInt("maxLimit")
					);

					//Requirements
					List<BuildingRequirements> buildingRequirements = new List<BuildingRequirements>();
					Godot.Collections.Array s = keyValues["buildingReq"] as Godot.Collections.Array;
					for (int i = 0; i < s.Count; i++)
					{
						Godot.Collections.Dictionary brd = (Godot.Collections.Dictionary)s[i];
						BuildingRequirements br = new BuildingRequirements(
							_type: brd.GetString("type"),
							_targetType: brd.GetString("tileType"),
							_distance: brd.GetInt("distance"),
							_negate: brd.GetBool("negate")
						);
						buildingRequirements.Add(br);
						brd.Dispose();
					}
					s.Dispose();
					specs.buildingRequirements = buildingRequirements;

					//Design
					specs.buildingDesign = new BuildingDesign(_spritePath: keyValues.GetString("spritePath"),
																_spriteScaling: keyValues.GetFloat("spriteScaling"));
					//BuildingProsume
					List<BuildingProsume> buildingProsumes = new List<BuildingProsume>();
					s = keyValues["buildingReq"] as Godot.Collections.Array;
					for (int i = 0; i < s.Count; i++)
					{
						Godot.Collections.Dictionary brd = (Godot.Collections.Dictionary)s[i];
						BuildingProsume buildingProsume = new BuildingProsume(_type: brd.GetString("type"),
							 _function: brd.GetString("function"),
							 _amount: brd.GetInt("amount"));
						buildingProsumes.Add(buildingProsume);
						brd.Dispose();

					}
					s.Dispose();
					specs.buildingProsume = buildingProsumes;
					//keyValues.Dispose();

					// Save it out for others
					buildings.Add(key.ToString(), specs);
				}

			}

		}
		else
		{
			GD.Print("Missing");
		}

	}
	#endregion

}
#region Classes
public class BuildingSpecs : Node
{
	public BuildingSpecs(int? _level,
							string _type,
							string _placementType,
							int? _cargoSpace,
							string _cargoHex,
							int? _maxLimit,
							string _upgradeOf = "")
	{
		level = _level;
		type = _type;
		placementType = _placementType;
		cargoSpace = _cargoSpace;
		cargoHex = _cargoHex;
		maxLimit = _maxLimit;
		upgradeOf = _upgradeOf;
	}
	public override string ToString()
	{
		return "Level: " + level.ToString() +
				"; type: " + level.ToString() +
				"; placementType: " + placementType.ToString() +
				"; cargoSpace: " + cargoSpace.ToString() +
				"; cargoHex: " + cargoHex.ToString() +
				"; maxLimit: " + maxLimit.ToString() +
				"; upgradeOf: " + upgradeOf.ToString();
	}
	public int? level { get; set; }
	public string type { get; set; }
	public string placementType { get; set; }
	public int? cargoSpace { get; set; }
	public string cargoHex { get; set; }
	public int? maxLimit { get; set; }
	public string upgradeOf { get; set; }
	public BuildingDesign buildingDesign { get; set; }
	public List<BuildingRequirements> buildingRequirements { get; set; }
	public List<BuildingProsume> buildingProsume { get; set; }
}
public class BuildingDesign : Node
{
	public BuildingDesign(string _spritePath,
							float _spriteScaling)
	{
		spritePath = _spritePath;
		spriteScaling = _spriteScaling;
	}
	public string spritePath { get; set; }
	public float spriteScaling { get; set; }
}
public class BuildingRequirements : Node
{
	public BuildingRequirements(string _type,
							string _targetType,
							int _distance,
							bool _negate)
	{
		type = _type;
		targetType = _targetType;
		distance = _distance;
		negate = _negate;
	}
	public override string ToString()
	{
		return "Type: " + type.ToString() +
				"; tileType: " + targetType.ToString() +
				"; distance: " + distance.ToString() +
				"; negate: " + negate.ToString();
	}
	public string type { get; set; }
	public string targetType { get; set; }
	public int distance { get; set; }
	public bool negate { get; set; }
}
public class BuildingProsume : Node
{
	public BuildingProsume(string _type,
							string _function,
							int _amount)
	{
		type = _type;
		function = _function;
		amount = _amount;
	}

	public string type { get; set; }
	public string function { get; set; }
	public int amount { get; set; }
}
#endregion
