using System;
using System.Collections.Generic;

/// <summary>
/// Created on 2019-06-29 
/// By Antoine Pilote.
/// </summary>
public class StateMachine : Godot.Object
{
    // List of possible states.
    private Dictionary<string, IState> m_states = new Dictionary<string, IState>();

    // Current state.
    private IState m_currentState;
    private Player m_player;


    public StateMachine(Player player)
    {
        m_player = player;
    }


    public void Update(float delta)
    {
        if (m_currentState == null)
            return;

        m_currentState.Update(ref m_player, delta);
    }


    /// <summary>
    /// Adds a state to the state machine. You must add
    /// the state before you can use it.
    /// </summary>
    /// <param name="state"></param>
    public void AddState(IState state)
    {
        m_states.Add(state.StateName, state);
    }


    /// <summary>
    /// Sets the current state
    /// </summary>
    /// <param name="stateName">desired state.</param>
    public void SetState(string stateName)
    {
        if (!m_states.ContainsKey(stateName))
            throw new Exception($"StateMachine doesn't have {stateName}.");

        m_currentState = m_states[stateName];
    }


    /// <summary>
    /// The player that this state machine is affecting.
    /// </summary>
    /// <param name="player"></param>
    public void SetPlayer(Player player)
    {
        m_player = player;
    }
}