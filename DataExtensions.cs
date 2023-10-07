using System;
using Godot.Collections;

public static class DataExtensions {

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