using Godot;
using System;

public class WorldNode : Control
{
    private bool Grabbed = false;

    public override void _Process(float delta)
    {
        if (Grabbed)
            this.RectGlobalPosition = Editor.Camera.GetGlobalMousePosition();
    }

    private void MoveHandle_button_down()
    {
        Grabbed = true;
    }


    private void MoveHandle_button_up()
    {
        Grabbed = false;
    }


    private void Body_pressed()
    {
        // TODO : Faire apparaitre un pop up menu pour les settings d'une World Node.
    }
}


