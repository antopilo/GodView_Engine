using Godot;
using System;

// This is for all of the camera movement in the editor view.
// Mainly for Moving the camera and zooming. Thats all it does really.
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

        if (@event.IsActionReleased("ZoomIn") && !Input.IsActionPressed("Shift") )
            this.Zoom /= new Vector2(1.1f, 1.1f);
        if (@event.IsActionReleased("ZoomOut") && !Input.IsActionPressed("Shift") )
            this.Zoom *= new Vector2(1.1f, 1.1f);
    }


    public override void _Process(float delta)
    {
        if (Panning)
            CamMovement = InitialPos - GetViewport().GetMousePosition();
        
        GlobalPosition = GlobalPosition + CamMovement * this.Zoom.x;
        CamMovement = Vector2.Zero;
        
        InitialPos = GetViewport().GetMousePosition();
    }

}
