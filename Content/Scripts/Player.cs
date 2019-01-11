using Godot;
using System;

public class Player : KinematicBody2D
{
    // Contrainte
    const float ACCELERATION = 35;
    const float DECELERATION = 10;
    const float MAXSPEED = 100;
    const float STOP_TRESHOLD = 10.1f;
     
    private Sprite SpriteNode;
    private Position2D HandPosition;
    

    // Direction de nos Inputs
    private Vector2 InputDirection = new Vector2();

    // Vitesse de notre joueur
    private Vector2 Velocity = new Vector2();

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        SpriteNode = GetNode("Sprite") as Sprite;
        HandPosition = GetNode("Position2D") as Position2D;
    }

    public override void _PhysicsProcess(float delta)
    {
        GetInputDirection(); // Obtient la Direction des inputs
        UpdateVelocity();
        MoveAndSlide(Velocity);

        if (Input.IsActionJustPressed("mouse1"))
        {
            SpriteNode.Centered = !SpriteNode.Centered;
        }

        if (GetGlobalMousePosition().x < this.GlobalPosition.x)
        {
            SpriteNode.FlipH = true;
            HandPosition.Position = new Vector2(-11, -11);
        }
        else
        {
            SpriteNode.FlipH = false;
            HandPosition.Position = new Vector2(11, -11);
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
}
