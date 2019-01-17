using Godot;
using System;

// Chest entity that can be opened if the player Interacts with it.
// TODO: Make the Chest unlock spells when openeing.
public class Chest : Entity
{
    [Export] public bool Interactable = true;
	public bool Opened = false;

	private AnimatedSprite _Sprite;
	
    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        base._Ready(); // Execute the Ready function from the Entity class.
        _Sprite = GetNode("AnimatedSprite") as AnimatedSprite; // Get sprite node
    }
	
	public override void _Process(float Delta)
	{
        base._Process(Delta);
        
        _Sprite.Animation = Opened ? "Open" : "Close";
	}

	public void Interact()
	{
		Opened = true;
	}
}
