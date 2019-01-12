using Godot;
using System;

public class Spell : Node2D
{
    private Node2D entities;

    private bool Unlocked = false;

    private string path = "";

    private PackedScene Projectile;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        Projectile = ResourceLoader.Load("res://Content/Scenes/Entities/Projectile/Waterball.tscn") as PackedScene;
        entities = GetNode("../../../") as Node2D;
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
        var Ball = Projectile.Instance() as KinematicBody2D;
        Ball.Rotation = 0.8;
        entities.AddChild(Ball);
    }
}
