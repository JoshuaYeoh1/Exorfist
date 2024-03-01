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
        Parry,
        Block,
        Stun,
        Death,
        Pause,
        Casting,
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
        PlayerParryState parryState = new PlayerParryState(this);
        PlayerBlockState blockState = new PlayerBlockState(this);
        PlayerStunState stunState = new PlayerStunState(this);
        PlayerDeathState deathState = new PlayerDeathState(this);
        PlayerPauseState pauseState = new PlayerPauseState(this);
        PlayerCastingState castingState = new PlayerCastingState(this);
        PlayerCastState castState = new PlayerCastState(this);

        States.Add(PlayerStates.Idle, idleState);
        States.Add(PlayerStates.Move, moveState);
        States.Add(PlayerStates.Combat, combatState);
        States.Add(PlayerStates.WindUp, windUpState);
        States.Add(PlayerStates.Attack, attackState);
        States.Add(PlayerStates.Parry, parryState);
        States.Add(PlayerStates.Block, blockState);
        States.Add(PlayerStates.Stun, stunState);
        States.Add(PlayerStates.Death, deathState);
        States.Add(PlayerStates.Pause, pauseState);
        States.Add(PlayerStates.Casting, castingState);
        States.Add(PlayerStates.Cast, castState);

        CurrentState = States[PlayerStates.Idle];
    }
}
