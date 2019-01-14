using Godot;
using System;

public class Editor : Node2D
{
    // Refs
    public static Viewport Viewport;
    public static Level CurrentLevel;
    public static YSort Entities;
    public static Node2D Nodes;
    public static EditorCamera Camera;
    public static PopupMenu MainMenu;

    public override void _Ready()
    {
        if (!GetTree().GetRoot().HasNode("Editor"))
            return;

        Camera = GetTree().GetRoot().GetNode("Editor/ViewportContainer/Viewport/Camera") as EditorCamera;
        Viewport = GetTree().GetRoot().GetNode("Editor/ViewportContainer/Viewport") as Viewport;
        Entities = GetTree().GetRoot().GetNode("Editor/ViewportContainer/Viewport/CurrentLevel/Layers/Entities") as YSort;
        Nodes = GetTree().GetRoot().GetNode("Editor/ViewportContainer/Viewport/CurrentLevel/Nodes") as Node2D;
    }
}
