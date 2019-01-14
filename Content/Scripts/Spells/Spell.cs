using Godot;
using System;

public class Spell : Entity
{
    private Node2D entities;

    private bool Unlocked = false;
    private Player _Player;
    private string path = "";

    private PackedScene Projectile;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        Projectile = ResourceLoader.Load("res://Content/Scenes/Entities/Projectile/Waterball.tscn") as PackedScene;
        entities = GetNode("../../../") as Node2D;
        _Player = GetNode("../..") as Player;

    }

    public override void _PhysicsProcess(float delta)
    {
        if (Input.IsActionJustPressed("Click"))
        {
           Shoot();
        }
    }

    public void Shoot()
    {
        var Distance = (this.GlobalPosition - GetGlobalMousePosition()).Normalized();
        var Angle = Distance.Angle();
        var Ball = ((PackedScene)Projectile).Instance();
        //GD.Print("Distance is " + Distance.ToString() + " - Angle is " + Angle.ToString() + "Ball is: " + Ball.ToString());
        (Ball as Node2D).RotationDegrees = Mathf.Rad2Deg(Angle) - 180;
        (Ball as Node2D).GlobalPosition = _Player.HandPosition.GlobalPosition;
        entities.AddChild(Ball);
    }
}
