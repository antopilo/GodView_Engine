using Godot;
using System;

public class WaterBall : Node2D
{

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        
    }

    public override void _PhysicsProcess(float delta)
    {
        MoveLocalX(10 * delta);
    }
}
