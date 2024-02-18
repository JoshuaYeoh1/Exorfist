using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPauseState : BaseState<PlayerStateMachine.PlayerStates>
{
    PlayerStateMachine stateMachine;

    public PlayerPauseState(PlayerStateMachine stateMachine) : base(PlayerStateMachine.PlayerStates.Pause)
    {
        this.stateMachine = stateMachine;
    }

    public override void EnterState()
    {
        Debug.Log("Player state: " + stateMachine.GetCurrentState().StateKey);

        stateMachine.player.canAttack=false;
        stateMachine.player.canBlock=false;
        stateMachine.player.canCast=false;
        stateMachine.player.canHurt=false;
        stateMachine.player.canStun=false; 
    }

    public override void UpdateState()
    {
        stateMachine.player.move.NoInput();
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
