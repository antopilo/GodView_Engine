using Godot;
using System;

public class FPScounter : Label
{
    override public void _Process(float delta)
    {
        Text = Engine.GetFramesPerSecond().ToString();
    }
}
