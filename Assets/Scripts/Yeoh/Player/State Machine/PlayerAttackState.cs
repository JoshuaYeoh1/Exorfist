using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttackState : BaseState<PlayerStateMachine.PlayerStates>
{
    PlayerStateMachine stateMachine;

    public PlayerAttackState(PlayerStateMachine stateMachine) : base(PlayerStateMachine.PlayerStates.Attack)
    {
        this.stateMachine = stateMachine;
    }

    public override void EnterState()
    {
        Debug.Log("Player state: " + stateMachine.GetCurrentState().StateKey);

        stateMachine.player.canMove=false;
        stateMachine.player.canTurn=true;
        stateMachine.player.canAttack=true;
        stateMachine.player.canBlock=false;
        stateMachine.player.canCast=false;
        stateMachine.player.canHurt=true;
        stateMachine.player.canStun=false; 
        stateMachine.player.canTarget=true;
        stateMachine.player.canMeditate=false;
    }

    public override void UpdateState()
    {
        
    }

    public override void FixedUpdateState()
    {

    }

    public override void ExitState()
    {

    }

    public override PlayerStateMachine.PlayerStates GetNextState() // Implement the logic to determine the next state from the this state
    {
        return StateKey;
    }
}
