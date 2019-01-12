using Godot;
using System;

public class FireArea : Node2D
{
    private bool PlayerPresent = false;
    [Export] float DamagePerSecond = 5;
    private Player _Player = null;

    public override void _PhysicsProcess(float delta)
    {
        if (_Player == null)
            return;
        if (PlayerPresent)
        {
            _Player.HurtPlayer(DamagePerSecond / 60);
            _Player.Hurting = true;
        }
        else
            _Player.Hurting = false;
    }

    private void _on_HitZone_body_entered(object body)
    {
        if(body is Player)
        {
            PlayerPresent = true;
            _Player = body as Player;
        }
    }

    private void _on_HitZone_body_exited(object body)
    {
        if(body is Player)
        {
            PlayerPresent = false;
        }
    }
}



