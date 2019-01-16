using Godot;
using System;

public class Spell : Entity
{
    public bool Unlocked = false;
    public bool Equipped = false;

    private Hand _Hand;
    private Node2D Entities;
    private Player _Player;
    
    [Export] private PackedScene Projectile;

    public override void _Ready()
    {
		Entities = GetNode("../../../") as Node2D;
        _Hand = GetParent() as Hand;
        _Player = GetNode("../..") as Player;
    }

    public override void _PhysicsProcess(float delta)
    {
        if( !Unlocked || !Equipped)
            return;

        if (Input.IsActionJustPressed("Click"))
           Shoot();
    }

    public void Shoot()
    {
        var Distance = (this.GlobalPosition - GetGlobalMousePosition() ).Normalized();
        var Angle = Distance.Angle();
        var Ball = ((PackedScene)Projectile).Instance();
        
        (Ball as Node2D).RotationDegrees = Mathf.Rad2Deg(Angle) - 180;
        (Ball as Node2D).GlobalPosition = _Player.HandPosition.GlobalPosition;

        Entities.AddChild(Ball);
    }
}
