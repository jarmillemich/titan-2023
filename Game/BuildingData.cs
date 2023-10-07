using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
public class BuildingData : Node
{
    // Declare member variables here. Examples:
    // private int a = 2;
    // private string b = "text";

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
    }
    public Godot.Collections.Dictionary<string, BuildingSpecs> buildings = new Godot.Collections.Dictionary<string, BuildingSpecs>();
    public void loadBuildingSpecs()
    {
        buildings = new Godot.Collections.Dictionary<string, BuildingSpecs>();
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
                        Godot.Collections.Dictionary brd = (Godot.Collections.Dictionary)s[i];// t.Result as Godot.Collections.Array;
                        BuildingRequirements br = new BuildingRequirements(
                            _type: brd.GetString("type"),
                            _tileType: brd.GetString("tileType"),
                            _distance: brd.GetInt("distance"),
                            _negate: brd.GetBool("negate")
                        );
                        buildingRequirements.Add(br);
                    }
                    specs.buildingRequirements = buildingRequirements;

                }
            }
        }
        else
        {
            GD.Print("Missing");
        }

    }
    private List<BuildingRequirements> LoadBuildingRequirements(string buildings)
    {
        List<BuildingRequirements> buildingRequirements = new List<BuildingRequirements>();


        return buildingRequirements;
    }
}
public class BuildingSpecs
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
public class BuildingDesign
{
    public BuildingDesign(string _spritePath,
                            string _spriteScaling)
    {
        spritePath = _spritePath;
        spriteScaling = _spriteScaling;
    }
    public string spritePath { get; set; }
    public string spriteScaling { get; set; }
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
    public override string ToString()
    {
        return "Type: " + type.ToString() +
                "; tileType: " + tileType.ToString() +
                "; distance: " + distance.ToString() +
                "; negate: " + negate.ToString();
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

