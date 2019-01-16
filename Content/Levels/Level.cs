using Godot;
using System;

public class Level : Node2D
{
    [Export] float DegreePerSecond = 1;

    [Export] 
    private float SunAngle = 0;
    private ShaderMaterial ShadowMaterial;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        ShadowMaterial = ResourceLoader.Load("res://Content/Shaders/Shadow.tres") as ShaderMaterial;
        SpawnPlayer();      
    }

    public override void _Process(float delta)
    {
        SunAngle += Mathf.Deg2Rad(DegreePerSecond / 60);
        ShadowMaterial.SetShaderParam("SunAngle", SunAngle);
    }

    public void SpawnPlayer(){
        if(!Game.InGame)
            return;

        Player Player = Game.PlayerScene.Instance() as Player;
        //Player.GlobalPosition = (Game.Entities.GetNode("Spawn") as Position2D).GlobalPosition;
        GetNode("Layers/Entities").AddChild( Game.PlayerScene.Instance() );
    }
}
