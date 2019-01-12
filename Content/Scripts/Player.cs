using Godot;
using System;
using System.Collections.Generic;

public class Player : KinematicBody2D
{
    // Contrainte
    const float ACCELERATION = 35;
    const float DECELERATION = 10;
    const float MAXSPEED = 100;
    const float STOP_TRESHOLD = 10.1f;
     
	public Camera2D PlayerCamera;
    private Sprite SpriteNode;
    private Position2D HandPosition;
    
	private List<Node2D> InteractableObject = new List<Node2D>(99);
	
    // Direction de nos Inputs
    private Vector2 InputDirection = new Vector2();

    // Vitesse de notre joueur
    private Vector2 Velocity = new Vector2();

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        SpriteNode = GetNode("Sprite") as Sprite;
        HandPosition = GetNode("HandPosition") as Position2D;
		PlayerCamera = GetNode("Camera2D") as Camera2D;
    }

    public override void _PhysicsProcess(float delta)
    {
        GetInputDirection(); // Obtient la Direction des inputs
        UpdateVelocity();
        MoveAndSlide(Velocity);
		GetInteractable();
        if (Input.IsActionJustPressed("mouse1"))
        {
            SpriteNode.Centered = !SpriteNode.Centered;
        }

        if (GetGlobalMousePosition().x < this.GlobalPosition.x)
        {
            SpriteNode.FlipH = true;
            HandPosition.Position = new Vector2(-6, -6);
        }
        else
        {
            SpriteNode.FlipH = false;
            HandPosition.Position = new Vector2(6, -6);
        }

        Update();
        
    }

    public override void _Draw()
    {
        DrawLine(GetGlobalMousePosition() - this.GlobalPosition, HandPosition.Position, Color.ColorN("blue"));
    }


    private void GetInputDirection()
    {
        if (Input.IsActionPressed("ui_left"))
            InputDirection.x = -1;
        else if (Input.IsActionPressed("ui_right"))
            InputDirection.x = 1;
        else
            InputDirection.x = 0;

        if (Input.IsActionPressed("ui_up"))
            InputDirection.y = -1;
        else if (Input.IsActionPressed("ui_down"))
            InputDirection.y = 1;
        else
            InputDirection.y = 0;
    }

    private void UpdateVelocity()
    {
        // Acceleration
        Velocity.x += InputDirection.x * ACCELERATION;
        Velocity.y += InputDirection.y * ACCELERATION;

        // Max Speed
        if (Mathf.Abs(Velocity.x) > MAXSPEED)
            Velocity.x = MAXSPEED * Mathf.Sign(Velocity.x);
        if (Mathf.Abs(Velocity.y) > MAXSPEED)
            Velocity.y = MAXSPEED * Mathf.Sign(Velocity.y);

        // Deceleration
        if (InputDirection.x == 0)
            Velocity.x -= DECELERATION * Mathf.Sign(Velocity.x);
        if (InputDirection.y == 0)
            Velocity.y -= DECELERATION * Mathf.Sign(Velocity.y);

        // Zero snaping
        if (Mathf.Abs(Velocity.x) < STOP_TRESHOLD && InputDirection.x == 0)
            Velocity.x = 0;
        if (Mathf.Abs(Velocity.y) < STOP_TRESHOLD && InputDirection.y == 0)
            Velocity.y = 0;
    }
	
	private void GetInteractable()
	{
        // Ordering them
        if (InteractableObject.Count > 0)
        {
            Node2D closest = InteractableObject[0];
            var closestDistance = (closest.GlobalPosition - this.GlobalPosition).Length();
            for (int i = 0; i < InteractableObject.Count; i++)
            {
                var currentDistance = (InteractableObject[i].GlobalPosition - this.GlobalPosition).Length();
                if (currentDistance < closestDistance)
                    closest = InteractableObject[i] as Node2D;
            }

            // Swaping the closest with the first of the list
            var temp = InteractableObject[0];
            var idx = InteractableObject.IndexOf(closest);
            InteractableObject[0] = closest;
            InteractableObject[idx] = temp;
        }
		if(Input.IsActionJustPressed("ui_interact") && InteractableObject != null)
		{
			if((InteractableObject[0] as Node2D).HasMethod("Interact"))
			    (InteractableObject[0] as Chest).Interact();
			
		}
        (GetNode("Label") as Label).Text = InteractableObject.Count > 0 ? (InteractableObject[0] as Node2D).Name : "Nothing..";
        if (InteractableObject.Count > 0 && InteractableObject[0] is Chest)
        {
            (GetNode("Label") as Label).Text += " - Opened :" + (InteractableObject[0] as Chest).Opened.ToString();
        } 

    }
	// Signals
	private void _on_InteractionRange_area_entered(object area)
	{
        InteractableObject.Insert(0, ((area as Area2D).GetParent() as Node2D));
	}
	
	private void _on_InteractionRange_area_exited(object area)
	{
        InteractableObject.Remove((area as Area2D).GetParent() as Node2D);
    }
}
