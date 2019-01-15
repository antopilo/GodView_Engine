using Godot;
using System;

public class Spell : Entity
{
    private Node2D entities;

    private bool Unlocked = false;
    private Player _Player;
    private string path = "";
    private PackedScene Projectile;

    private PackedScene[] SpellPool;
    private PackedScene Fireball;
	private PackedScene Waterball;
	
	private int index = 1;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        Waterball = ResourceLoader.Load("res://Content/Scenes/Entities/Projectile/Waterball.tscn") as PackedScene;
        Fireball = ResourceLoader.Load("res://Content/Scenes/Entities/Projectile/Fireball.tscn") as PackedScene;
		entities = GetNode("../../../") as Node2D;
        _Player = GetNode("../..") as Player;
		
		projectile();

        // Current equipped spell;
        SpellPool = new PackedScene[1];

        SpellPool[0] = Fireball;
        SpellPool[1] = Waterball;
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
	
	// Rng the current selected spell.
	public void updateIndex(){
        RandomNumberGenerator rng = new RandomNumberGenerator();
        index = rng.RandiRange(0, 1);
	}

    // Change an equipped spell for a new one.
    public void ChangeSpell(int idx, PackedScene spellScene){
        SpellPool[idx] = spellScene;
    }

    // Swaps the two current spells
    public void SwapSpell(){
        PackedScene temp = SpellPool[0];
        SpellPool[0] = SpellPool[1];
        SpellPool[1] = temp;
    }
}
