using Godot;
using System;

public class Camera : Camera2D
{
    const int CameraMaxRangeFromPlayer = 175;

    public bool Shaking = false;

    private Timer _Timer;

    private Player _Player;
    private float ZoomStep = 1;
    private float Amount;
    private RandomNumberGenerator Rng = new RandomNumberGenerator();

    private float MaxZoom = 0.75f;
    private float MinZoom = 0.5f;
    private float ZoomModifier = 0.75f;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        _Player = GetParent() as Player;
        //Shake(1, 5);
    }

    public override void _PhysicsProcess(float delta)
    {
        AimPeeking();

        if (Shaking)
        {
            Rng.Randomize(); // Randomizing seed
            float x = Rng.RandfRange(-Amount, Amount);

            Rng.Randomize(); // Randomizing seed again
            float y = Rng.RandfRange(-Amount, Amount);
            
            Offset += new Vector2(x, y);
        }
        Zoom = new Vector2(ZoomModifier / 2, ZoomModifier / 2);
    }
    public override void _Input(InputEvent @event)
    {
        if (@event.IsActionPressed("ZoomIn"))
            ZoomModifier -= 0.01f;
        else if (@event.IsActionPressed("ZoomOut"))
            ZoomModifier += 0.01f;

        if (ZoomModifier < MinZoom)
            ZoomModifier = MinZoom;
        if (ZoomModifier > MaxZoom)
            ZoomModifier = MaxZoom;

        
    }


    private void AimPeeking()
    {
        var Distance = GetGlobalMousePosition() - _Player.GlobalPosition;

        Distance.x = Mathf.Clamp(Distance.x , -CameraMaxRangeFromPlayer, CameraMaxRangeFromPlayer);
        Distance.y = Mathf.Clamp(Distance.y, -CameraMaxRangeFromPlayer, CameraMaxRangeFromPlayer);

        Offset = Distance / 4;
    }

    /// <summary>
    /// Shake the camera
    /// </summary>
    /// <param name="amount">Force of the shaking</param>
    /// <param name="time">Duration of the shaking</param>
    public void Shake(float amount, float time)
    {
        // Stopping current one if it is already shaking.
        if (Shaking)
            StopShaking();

        // Adding new timer
        var Timer = new Timer();
        AddChild(Timer);

        Timer.WaitTime = time;
        Timer.Connect("timeout", this, "StopShaking");
        Timer.Start();

        Amount = amount;
        _Timer = Timer;
        Shaking = true; // SetShaking loop process

        LimitLeft -= Mathf.CeilToInt(Amount);
        LimitRight += Mathf.CeilToInt(Amount);
        LimitTop -= Mathf.CeilToInt(Amount);
        LimitBottom += Mathf.CeilToInt(Amount);
    }

    /// <summary>
    /// Stop the shaking if the camera is currently shaking
    /// </summary>
    public void StopShaking()
    {
        if (!Shaking)
            return;

        _Timer.Stop();
        _Timer.QueueFree();

        Shaking = false;
        Offset = new Vector2(0, 0);

        LimitLeft += Mathf.CeilToInt(Amount);
        LimitRight -= Mathf.CeilToInt(Amount);
        LimitTop += Mathf.CeilToInt(Amount);
        LimitBottom -= Mathf.CeilToInt(Amount);
    }
}
