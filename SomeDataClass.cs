using Godot.Collections;
using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
public class SomeOtherClass
{
    // public void aMethod(string Path)
    // {
    //     var file = new File();

    //     if (file.FileExists(path))
    //     {
    //         file.Open(path, File.ModeFlags.Read);
    //         JSONParseResult p = JSON.Parse(file.GetAsText());
    //         using (Godot.Collections.Dictionary t = (Godot.Collections.Dictionary)p.Result)
    //         {
    //             Godot.Collections.Dictionary<string, buildingtemplate> gameEntities = new Godot.Collections.Dictionary<string, buildingtemplate>();
    //             foreach (string s in t.Keys)
    //             {
    //                 buildingtemplate b = (buildingtemplate)building_scene.Instance();

    //                 // JSONParseResult parseElement = JSON.Parse(JSON.Print(s.ToString()));
    //                 // // Godot.Collections.Dictionary<string,string> elementdata = (Godot.Collections.Dictionary<string,string>)parseElement.Result;
    //                 // GD.Print(JSON.Print(s.ToString()));
    //                 string sp = (t[s] as Godot.Collections.Dictionary)["sprite"].ToString();
    //                 b.setSprite(sp);
    //                 string[] pos = (t[s] as Godot.Collections.Dictionary)["position"].ToString().Split(",");

    //                 b.Position = new Vector2(float.Parse(pos[0]), float.Parse(pos[1]));

    //                 GetNode("Node").AddChild(b);
    //             }
    //         }
    //     }
    //     else
    //     {
    //         GD.Print("Missing");
    //     }
    // }
}
public class SomeDataClass
{
    public bool SomeBool { get; set; }
    public int SomeInt { get; set; }
    public string SomeString { get; set; }
    public List<SomeOtherClass> Things { get; set; }

    public static SomeDataClass FromJson(Dictionary src)
    {
        return new SomeDataClass()
        {
            SomeBool = src.Get<bool>("SomeBool"),
            SomeInt = src.Get<int>("SomeInt"),
            SomeString = src.GetRef<string>("SomeString"),
            //Things = src.GetArray("Things").Select(SomeOtherClass.FromJson).ToList(),
        };
    }
}