using Godot;
using System;

public class Level : Node2D
{
    [Export] float SunAngle = 0;
    private ShaderMaterial ShadowMaterial;
    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        ShadowMaterial = ResourceLoader.Load("res://Content/Shaders/Shadow.tres") as ShaderMaterial;
    }

    public override void _Process(float delta)
    {
        if (SunAngle >= 90)
            SunAngle = -90;
        SunAngle += 0.1f;
        ShadowMaterial.SetShaderParam("SunAngle", SunAngle);
    }
}
