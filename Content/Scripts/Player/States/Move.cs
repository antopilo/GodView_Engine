using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class Move : IState
{
    public string StateName { get; } = "Move";

    // Physics
    const float ACCELERATION = 35;
    const float DECELERATION = 10;
    const float MAXSPEED = 100;
    const float STOP_TRESHOLD = 10.1f;

    private Vector2 InputDirection = new Vector2();
    private Vector2 Velocity = new Vector2();


    public void Update(ref Player player, float delta)
    {
        GetInputDirection();

        UpdateVelocity();
        player.MoveAndSlide(Velocity);
    }


    // Movement code.
    private void GetInputDirection()
    {
        // Horizontal Inputs.
        if (Input.IsActionPressed("ui_left"))
            InputDirection.x = -1;
        else if (Input.IsActionPressed("ui_right"))
            InputDirection.x = 1;
        else
            InputDirection.x = 0;

        // Vertical Inputs.
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
}

