using Godot;
using System;
using System.Collections.Generic;

public class Player : KinematicBody2D
{
    // Physics
    const float ACCELERATION = 35;
    const float DECELERATION = 10;
    const float MAXSPEED = 100;
    const float STOP_TRESHOLD = 10.1f;
    
    private Vector2 InputDirection = new Vector2();
    private Vector2 Velocity = new Vector2();

    // References
    public Camera2D PlayerCamera;
    public Position2D HandPosition;

    private Sprite SpriteNode;
    private Sprite OutlineSprite;
    private Sprite ShadowSprite;

    public bool Hurting = false;
    private TextureProgress HealthBar;
    private Particles2D HealthParticles;

    // States
    public bool Alive = true;

    private float MaxHealth = 100;
    private float Health;
    [Export] private Color FullHealthTint = new Color("63c74d");
    [Export] private Color MidHealthTint = new Color("f77622");
    [Export] private Color LowHealthTint = new Color("e43b44");
    
    // Logic stuff
	private List<Node2D> InteractableObject = new List<Node2D>(99);

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        SpriteNode = GetNode("Sprite") as Sprite;
        OutlineSprite = GetNode("Outline") as Sprite;
        ShadowSprite = GetNode("Shadow") as Sprite;
        HandPosition = GetNode("Hand") as Position2D;
		PlayerCamera = GetNode("Camera") as Camera2D;
        HealthBar = GetNode("HealthBar") as TextureProgress;
        HealthParticles = HealthBar.GetNode("Particles2D") as Particles2D;

        Health = MaxHealth;
    }

    // Called 60 times per second.
    public override void _PhysicsProcess(float delta)
    {
        GetInputDirection(); // Obtient la Direction des inputs
        UpdateVelocity(); // Calculate acceleration and other stuff.
        MoveAndSlide(Velocity); // Move

        UpdateParticles();
        
        UpdateSprite(); // Adjust the Sprite of the player
		GetInteractable(); // Check for interactable objects
    }

    private void UpdateSprite()
    {
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

        OutlineSprite.FlipH = SpriteNode.FlipH;
        ShadowSprite.FlipH = SpriteNode.FlipH;
    }

    private void UpdateParticles()
    {
        HealthParticles.Emitting = Hurting;
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

        // Zero snapping
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
            var closest = InteractableObject[0];
            var closestDistance = (closest.GlobalPosition - GlobalPosition).Length(); //Distance du joueur

            for (int i = 0; i < InteractableObject.Count; i++)
            {
                var currentDistance = (InteractableObject[i].GlobalPosition - this.GlobalPosition).Length();
                if (currentDistance < closestDistance) // Compare si il y en a un plus proche que le present
                    closest = InteractableObject[i] as Node2D;
            }

            // Swaping the closest with the first of the list
            var temp = InteractableObject[0];
            var idx = InteractableObject.IndexOf(closest);

            InteractableObject[0] = closest;
            InteractableObject[idx] = temp;
        }

		if(Input.IsActionJustPressed("ui_interact") && InteractableObject.Count > 0)
		{
			if( (InteractableObject[0] as Node2D).HasMethod("Interact") )
			    (InteractableObject[0] as Chest).Interact();
		}

        (GetNode("Label") as Label).Text = InteractableObject.Count > 0 ? (InteractableObject[0] as Node2D).Name : "Nothing..";
        if (InteractableObject.Count > 0 && InteractableObject[0] is Chest)
        {
            (GetNode("Label") as Label).Text += " - Opened :" + (InteractableObject[0] as Chest).Opened.ToString();
        } 
    }

    public void HurtPlayer(float amount)
    {
        HealthBar.Value = Health -= amount;
        Health = Mathf.Clamp(Health, 0, MaxHealth);

        // Setting health bar Color
        if (Health < 33)
            HealthBar.SetTintProgress(LowHealthTint);
        else if(Health < 66)
            HealthBar.SetTintProgress(MidHealthTint);
        else
            HealthBar.SetTintProgress(FullHealthTint);

        HealthParticles.Position = new Vector2(Health / 4, 3);

        (HealthParticles.GetProcessMaterial() as ParticlesMaterial).Color = HealthBar.GetTintProgress();
        if (Health <= 0)
            Die();
    }

    private void Die()
    {
        // TODO: Death of player.
        Alive = false;
    }
	
	

	#region Signal
    private void _on_InteractionRange_area_entered(object area)
    {
        InteractableObject.Insert(0, ((area as Area2D).GetParent() as Node2D));
    }

    private void _on_InteractionRange_area_exited(object area)
    {
        InteractableObject.Remove((area as Area2D).GetParent() as Node2D);
    } 
	
	
    #endregion
}
