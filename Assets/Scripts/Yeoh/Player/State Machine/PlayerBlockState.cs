using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBlockState : BaseState<PlayerStateMachine.PlayerStates>
{
    PlayerStateMachine stateMachine;

    public PlayerBlockState(PlayerStateMachine stateMachine) : base(PlayerStateMachine.PlayerStates.Block)
    {
        this.stateMachine = stateMachine;
    }

    public override void EnterState()
    {
        Debug.Log("Player state: " + stateMachine.GetCurrentState().StateKey);

        stateMachine.player.canAttack=false;
        stateMachine.player.canBlock=false;
        stateMachine.player.canStun=false;
    }

    public override void UpdateState()
    {
        stateMachine.player.move.CheckInput();
    }

    public override void ExitState()
    {

    }

    public override PlayerStateMachine.PlayerStates GetNextState() // Implement the logic to determine the next state from the this state
    {
        return StateKey;
    }
}
