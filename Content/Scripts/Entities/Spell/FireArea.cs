using Godot;
using System;

// This is a Fire Area that hurts the player when on it.
// It make sure to not execute
public class FireArea : Entity
{
    public bool Burning = true;

    [Export] private float TracerDistanceSpacing = 25;
    [Export] private float DamagePerSecond = 5;

    private Particles2D Flames;
    private bool PlayerPresent = false;
    private Player _Player = null;


    public override void _Ready()
    {
        base._Ready();
        Flames = GetNode("Flames") as Particles2D;
    }


    public override void _PhysicsProcess(float delta)
    {
        Flames.Emitting = Burning;
        if(Selected) Flames.Emitting = true;

        if (_Player == null) return; // Check if the player exists
            
        if (PlayerPresent && Burning)
        {
            _Player.HurtPlayer(DamagePerSecond / 60);
            _Player.Hurting = true;
        }
        else
            _Player.Hurting = false;
        
    }


    // Extinguish this FireArea only.
    public void Extinguish()
    {
        (GetNode("Smoke") as Particles2D).Emitting = true;
        Burning = false;
    }


    // Signals when a body enters the entered.
    private void _on_HitZone_body_entered(object body)
    {
        if(body is Player)
        {
            PlayerPresent = true;
            _Player = body as Player;
        }
    }


    // Signals when a body leaves this
    private void _on_HitZone_body_exited(object body)
    {
        if(body is Player)
            PlayerPresent = false;
    }
}