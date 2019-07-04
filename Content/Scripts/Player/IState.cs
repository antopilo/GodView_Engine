using System;

public interface IState
{
    string StateName 
    {
        get;
    }

    /// <summary>
    /// What should the player do in this state.
    /// </summary>
    /// <param name="host">The player in this state.</param>
    /// <param name="delta">Time spent between this and the previous frame.</param>
    void Update(ref Player host, float delta);
}