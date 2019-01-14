using Godot;
using System;

public class EditorCamera : Camera2D
{
    private bool Panning = false;
    
    private Vector2 InitialPos = new Vector2();
    private Vector2 CamMovement = new Vector2();
    
    public override void _Input(InputEvent @event)
    {
        if (@event.IsActionPressed("MiddleMouse"))
            Panning = true;
        else if(@event.IsActionReleased("MiddleMouse"))
            Panning = false;

        if (@event.IsActionReleased("ZoomIn"))
            this.Zoom /= new Vector2(1.1f, 1.1f);
        else if (@event.IsActionReleased("ZoomOut"))
            this.Zoom *= new Vector2(1.1f, 1.1f);
    }
    public override void _Process(float delta)
    {
        if (Panning)
            CamMovement = InitialPos - GetViewport().GetMousePosition();

        this.GlobalPosition = this.GlobalPosition + CamMovement * this.Zoom.x;
        CamMovement = Vector2.Zero;

        InitialPos = GetViewport().GetMousePosition();
    }
}
