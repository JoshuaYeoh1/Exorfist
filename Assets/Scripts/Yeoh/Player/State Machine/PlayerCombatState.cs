using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCombatState : BaseState<PlayerStateMachine.PlayerStates>
{
    PlayerStateMachine stateMachine;

    public PlayerCombatState(PlayerStateMachine stateMachine) : base(PlayerStateMachine.PlayerStates.Combat)
    {
        this.stateMachine = stateMachine;
    }

    public override void EnterState()
    {
        Debug.Log("Player state: " + stateMachine.GetCurrentState().StateKey);

        stateMachine.player.canMove=true;
        stateMachine.player.canTurn=true;
        stateMachine.player.canAttack=true;
        stateMachine.player.canBlock=true;
        stateMachine.player.canCast=true;
        stateMachine.player.canHurt=true;
        stateMachine.player.canStun=true; 
        stateMachine.player.canTarget=true;
    }

    public override void UpdateState()
    {
        CheckNoCombat();
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

    void CheckNoCombat()
    {
        if(!stateMachine.player.target)
        {
            stateMachine.TransitionToState(PlayerStateMachine.PlayerStates.Idle);
        }
    }
}
