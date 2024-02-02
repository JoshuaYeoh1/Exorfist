using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStateMachine : StateMachine<PlayerStateMachine.PlayerState>
{
    public enum PlayerState
    {
        Idle,
        Combat,
        Attack,
        Block,
        Stagger,
        Stun,
        Death,
    }

    void Awake()
    {
        CurrentState = States[PlayerState.Idle];
    }
}
