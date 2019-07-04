using System;

public class Idle : IState 
{
    public string StateName { get; } = "Idle";

    public void Update(ref Player host, float delta)
    {
        

    }

    public void Ready() { }
}