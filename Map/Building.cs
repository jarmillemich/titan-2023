using Godot;
using System;
using System.Collections.Generic;
[Tool]
public class Building : Node2D
{
	public override void _Ready()
	{

	}
	public BuildingSpecs buildingSpecs{get;set;}
	public void setSprite(string spritePath, string scalingSprite)
	{
		((Sprite)GetNode("Sprite")).Texture = (Texture)GD.Load(spritePath);
		//Build the scaling of the sprite. 
	}
	
}
public class BuildingSpecs
{
	public BuildingSpecs(int _level,
							string _type,
							string _placementType,
							int _cargoSpace,
							string _cargoHex,
							int _maxLimit,
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
	public int level { get; set; }
	public string type { get; set; }
	public string placementType { get; set; }
	public int cargoSpace { get; set; }
	public string cargoHex { get; set; }
	public int maxLimit { get; set; }
	public string upgradeOf { get; set; }
	public List<BuildingRequirements> buildingRequirements { get; set; }
	public List<BuildingProsume> buildingProsume { get; set; }
}
public class BuildingRequirements
{
	public BuildingRequirements(string _type,
							string _tileType,
							int _distance,
							bool _negate)
	{
		type = _type;
		tileType = _tileType;
		distance = _distance;
		negate = _negate;
	}
	public string type { get; set; }
	public string tileType { get; set; }
	public int distance { get; set; }
	public bool negate { get; set; }
}
public class BuildingProsume
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
