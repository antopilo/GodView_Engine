using Godot;
using System;

public class WaterBall : Node2D
{

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        //GD.Print("Hey I spawned !");
    }

    public override void _PhysicsProcess(float delta)
    {
        MoveLocalX(100 * delta);
		//GD.Print("Heres my position faggot: " + this.GlobalPosition.ToString());
    }
}
