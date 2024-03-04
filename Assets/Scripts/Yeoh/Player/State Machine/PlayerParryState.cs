using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerParryState : BaseState<PlayerStateMachine.PlayerStates>
{
    PlayerStateMachine stateMachine;

    public PlayerParryState(PlayerStateMachine stateMachine) : base(PlayerStateMachine.PlayerStates.Parry)
    {
        this.stateMachine = stateMachine;
    }

    public override void EnterState()
    {
        Debug.Log("Player state: " + stateMachine.GetCurrentState().StateKey);

        stateMachine.player.canMove=true;
        stateMachine.player.canTurn=true;
        stateMachine.player.canBlock=true;
        stateMachine.player.canCast=false;
        stateMachine.player.canHurt=true;
        stateMachine.player.canStun=true; 
        stateMachine.player.canTarget=true;
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
