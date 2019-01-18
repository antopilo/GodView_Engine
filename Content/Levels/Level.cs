using Godot;
using System;

// This is the script that handles all of th level logics.
// This could be spawns, weather, lighing, Quest, npcs etc.
public class Level : Node2D
{
    [Export] float SunDegreePerSecond = 1;
    [Export] private float SunAngle = 0;
    private ShaderMaterial ShadowMaterial;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        ShadowMaterial = (ShaderMaterial)ResourceLoader.Load("res://Content/Shaders/Shadow.tres");
    }

    public override void _PhysicsProcess(float delta)
    {
        SunAngle += Mathf.Deg2Rad(SunDegreePerSecond / 60);
        ShadowMaterial.SetShaderParam("SunAngle", SunAngle);
    }


    public void SpawnPlayer()
    {
        if (!Game.InGameMode)
        {   
            throw new Exception("Can't spawn the player because not in game mode");
        }
            
        if(Game.Entities.HasNode("Player"))
        {
            GD.PushWarning("[Level] Tried spawning the player, but player already spawned.");
            return;
        }

        // Making the player and if there is a Spawn. Spawn the player there
        // If not spawn is found, the player will be at X:0 Y:0
        Player Player = Game.PlayerScene.Instance() as Player;
        Player.Name = "Player"; // Naming the player.

        if(Game.Entities.HasNode("Spawn"))
            Player.GlobalPosition = (Game.Entities.GetNode("Spawn") as Entity).GlobalPosition;
        
        Game.Entities.AddChild(Player, true);
    }
}
