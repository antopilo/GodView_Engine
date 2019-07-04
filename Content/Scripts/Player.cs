using Godot;
using System;
using System.Collections.Generic;

public class Player : KinematicBody2D
{
    // References
    public Camera2D PlayerCamera;
    public Position2D HandPosition;
    private Sprite SpriteNode;
    private Sprite OutlineSprite;
    private Sprite ShadowSprite;

    private StateMachine StateMachine; 

    // Health
    [Export] private Color FullHealthTint = new Color("63c74d");
    [Export] private Color MidHealthTint = new Color("f77622");
    [Export] private Color LowHealthTint = new Color("e43b44");

    public bool Alive = true;
    public bool Hurting = false;
    private float MaxHealth = 100;
    private float Health;
    private TextureProgress HealthBar;
    private Particles2D HealthParticles;

    // Interaction
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

        // Create state machine with player as parameter.
        StateMachine = new StateMachine(this);

        // Adds possible states.
        StateMachine.AddState(new Idle());
        StateMachine.AddState(new Move());
        StateMachine.AddState(new Jump());

        // Set the state.
        StateMachine.SetState("Move");
    }

    // Called 60 times per second.
    public override void _PhysicsProcess(float delta)
    {
        StateMachine.Update(delta);

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
			    InteractableObject[0].Call("Interact");
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
        var parent = (area as Area2D).GetParent() as Node2D;
        if(parent.IsInGroup("Interact"))
            InteractableObject.Insert(0, ((area as Area2D).GetParent() as Node2D));
    }

    private void _on_InteractionRange_area_exited(object area)
    {
        var parent = (area as Area2D).GetParent() as Node2D;
        if(parent.IsInGroup("Interact"))
            InteractableObject.Remove((area as Area2D).GetParent() as Node2D);
    } 
	
	
    #endregion
}
