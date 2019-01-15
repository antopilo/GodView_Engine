using Godot;
using System;

public class HealArea : Entity
{
    private bool PlayerPresent = false;
    [Export] float HealPerSecond = 5;
    private Player _Player = null;

    public override void _PhysicsProcess(float delta)
    {
        if (_Player == null)
            return;

        if (PlayerPresent)
        {
            _Player.HurtPlayer( HealPerSecond / 60 * -1);
        }
    }

    private void _on_HitZone_body_entered(object body)
    {
        if (body is Player)
        {
            PlayerPresent = true;
            _Player = body as Player;
        }
    }

    private void _on_HitZone_body_exited(object body)
    {
        if (body is Player)
        {
            PlayerPresent = false;
        }
    }
}