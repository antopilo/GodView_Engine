using Godot;
using System;

public class Shadow : Node2D
{
    // Declare member variables here. Examples:
    // private int a = 2;
    // private string b = "text";

    private ShaderMaterial ShadowMaterial;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        ShadowMaterial = ResourceLoader.Load("res://Content/Shaders/Shadow.tres") as ShaderMaterial;
        ShadowMaterial.SetShaderParam("height", (this.GetNode("Sprite") as Sprite).Texture.GetSize().y);
        ShadowMaterial.SetShaderParam("height", (this.GetNode("Sprite") as Sprite).Texture.GetSize().x);
    }

}
