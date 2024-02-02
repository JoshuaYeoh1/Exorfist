using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStateMachine : StateMachine<PlayerStateMachine.PlayerStates>
{
    public enum PlayerStates
    {
        Idle,
        Move,
        Combat,
        WindUp,
        Attack,
        Block,
        Parry,
        Stagger,
        Death,
        Pause,
        Cast,
    }

    [HideInInspector] public Player player;

    void Awake()
    {
        player=GetComponent<Player>();

        CurrentState = States[PlayerStates.Idle];
    }
}
