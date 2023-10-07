using Godot;
using System;
using System.Collections.Generic;
[Tool]
public class Building : Node2D
{
	public override void _Ready()
	{

	}
	public BuildingSpecs buildingSpecs {get;set;}
	public void setSprite(string spritePath, string scalingSprite)
	{
		((Sprite)GetNode("Sprite")).Texture = (Texture)GD.Load(spritePath);
		//Build the scaling of the sprite. 
	}
	
}
