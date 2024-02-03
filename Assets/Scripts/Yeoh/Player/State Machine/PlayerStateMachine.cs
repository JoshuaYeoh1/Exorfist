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

        PlayerIdleState idleState = new PlayerIdleState(this);
        PlayerMoveState moveState = new PlayerMoveState(this);
        PlayerCombatState combatState = new PlayerCombatState(this);
        PlayerWindUpState windUpState = new PlayerWindUpState(this);
        PlayerAttackState attackState = new PlayerAttackState(this);

        States.Add(PlayerStates.Idle, idleState);
        States.Add(PlayerStates.Move, moveState);
        States.Add(PlayerStates.Combat, combatState);
        States.Add(PlayerStates.WindUp, windUpState);
        States.Add(PlayerStates.Attack, attackState);

        CurrentState = States[PlayerStates.Idle];
    }
}
