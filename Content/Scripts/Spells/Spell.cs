using Godot;
using System;

public class Spell : Entity
{
    private Node2D entities;

    private bool Unlocked = false;
    private Player _Player;
    private string path = "";
    private PackedScene Projectile;
	
	private PackedScene Fireball;
	private PackedScene Waterball;
	
	private int index = 0;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        Waterball = ResourceLoader.Load("res://Content/Scenes/Entities/Projectile/Waterball.tscn") as PackedScene;
        Fireball = ResourceLoader.Load("res://Content/Scenes/Entities/Projectile/Fireball.tscn") as PackedScene;
		
		entities = GetNode("../../../") as Node2D;
        _Player = GetNode("../..") as Player;
		
		projectile();

    }

    public override void _PhysicsProcess(float delta)
    {
        if (Input.IsActionJustPressed("Click"))
        {
           Shoot();
		   updateIndex();
		   projectile();
        }
    }

    public void Shoot()
    {
        var Distance = (this.GlobalPosition - GetGlobalMousePosition() ).Normalized();
        var Angle = Distance.Angle();
        var Ball = ((PackedScene)Projectile).Instance();
        //GD.Print("Distance is " + Distance.ToString() + " - Angle is " + Angle.ToString() + "Ball is: " + Ball.ToString());
        (Ball as Node2D).RotationDegrees = Mathf.Rad2Deg(Angle) - 180;
        (Ball as Node2D).GlobalPosition = _Player.HandPosition.GlobalPosition;
        entities.AddChild(Ball);
    }
	
	//loading which spell as the next projectile
	public void projectile()
	{
		switch(index)
		{
			case 0:
				Projectile = Waterball;
			break;
			case 1:
				Projectile = Fireball;
			break;
		}
	}
	
	//the index must be random to choose the spell instead of just alternating like it is now
	public void updateIndex()
	{
		index++;
		if(index == 2)
		{
			index = 0;
		}
	}
	
	
}
