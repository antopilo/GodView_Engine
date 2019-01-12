using Godot;
using System;

public class Chest : Node2D
{
    [Export] public bool Interactable = true;
	
	private AnimatedSprite _Sprite;
	
	public bool Opened = false;
	
    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        _Sprite = GetNode("Sprite") as AnimatedSprite;
    }
	
	public override void _Process(float Delta)
	{
		_Sprite.Animation = Opened ? "Open" : "Close";
	}

	public void Interact()
	{
		Opened = true;
	}
}
