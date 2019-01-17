using Godot;
using System;

// Chest entity that can be opened if the player Interacts with it.
// TODO: Make the Chest unlock spells when openeing.
public class Chest : Entity
{
    [Export] public bool Interactable = true;
	public bool Opened = false;
    
    private Spell[] AllSpell;

	private AnimatedSprite _Sprite;
	
    private PackedScene DroppedS;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        base._Ready(); // Execute the Ready function from the Entity class.
        _Sprite = GetNode("AnimatedSprite") as AnimatedSprite; // Get sprite node
        DroppedS = ResourceLoader.Load("res://Content/Scenes/Entities/Spells/Dropped/Drop.tscn") as PackedScene;
    }
	
	public override void _Process(float Delta)
	{
        base._Process(Delta);
        
        _Sprite.Animation = Opened ? "Open" : "Close";
	}

	public void Interact()
	{
		Opened = true;
        var drop = DroppedS.Instance() as Dropped;
        drop.GlobalPosition = this.GlobalPosition + new Vector2(0, 16);
        Game.Entities.AddChild(drop);
	}
}
