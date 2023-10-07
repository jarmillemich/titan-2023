using Godot;
using System;
using System.Collections.Generic;
[Tool]
public class Building : Node2D
{
	public override void _Ready()
	{
		_isReady = true;
		UpdateSprite();
	}
	
	[Export]
	public string Type {
		get => _type;
		set {
			_type = value;
			if (_isReady) UpdateSprite();
		}
	}

	private string _type = "None";

	private Godot.Collections.Dictionary<string, BuildingSpecs> buildingData => GetNode<BuildingData>("/root/BuildingData").buildings;

	public void UpdateSprite()
	{
		if (!buildingData.ContainsKey(_type)) return;

		GD.Print("Loading building proto for ", _type);

		var spec = buildingData[_type];

		var texture = (Texture)GD.Load(spec.buildingDesign.spritePath);
		float factor = texture.GetSize().Length();
		var sprite = (Sprite)GetNode("Sprite");
		sprite.Texture = texture;
		//sprite.Scale = 120 * Vector2.One / factor;
		GD.Print(texture.GetSize(), sprite.Scale);

		//Build the scaling of the sprite. 
	}
	

	private bool _isReady = false;
}
