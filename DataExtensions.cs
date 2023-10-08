using System;
using Godot.Collections;

public static class DataExtensions {

public static string GetString(this Dictionary dict, string fieldName) {
		return dict.Contains(fieldName) ? dict[fieldName].ToString() : "";
	}
	public static int GetInt(this Dictionary dict, string fieldName) {
		return  dict.Contains(fieldName) ? int.Parse(dict[fieldName].ToString()) : -1;
	}
	public static float GetFloat(this Dictionary dict, string fieldName) {
		return  dict.Contains(fieldName) ? float.Parse(dict[fieldName].ToString()) : -1;
	}
	public static bool GetBool(this Dictionary dict, string fieldName) {
		return dict.Contains(fieldName) ? bool.Parse(dict[fieldName].ToString()) : false;
	}
	public static T Get<T>(this Dictionary dict, string fieldName) where T : struct {
		return (dict[fieldName] as T?) ?? throw new Exception($"Failed to retrieve {fieldName}");
	}

	public static T GetRef<T>(this Dictionary dict, string fieldName) where T : class {
		return (dict[fieldName] as T) ?? throw new Exception($"Failed to retrieve {fieldName}");
	}

	public static T? GetNullable<T>(this Dictionary dict, string fieldName) where T : struct {
		return dict[fieldName] as T?;
	}

	public static T GetRefNullable<T>(this Dictionary dict, string fieldName) where T : class {
		return dict[fieldName] as T;
	}

	public static Godot.Collections.Array GetArray(this Dictionary dict, string fieldName) {
		return (dict[fieldName] as Godot.Collections.Array) ?? throw new Exception($"Failed to retrieve {fieldName}");
	}
}
