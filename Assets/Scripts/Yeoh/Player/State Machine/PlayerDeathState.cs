using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDeathState : BaseState<PlayerStateMachine.PlayerStates>
{
    PlayerStateMachine stateMachine;

    public PlayerDeathState(PlayerStateMachine stateMachine) : base(PlayerStateMachine.PlayerStates.Death)
    {
        this.stateMachine = stateMachine;

        stateMachine.player.isAlive=false;
    }

    public override void EnterState()
    {
        Debug.Log("Player state: " + stateMachine.GetCurrentState().StateKey);

        stateMachine.player.canAttack=false;
        stateMachine.player.canBlock=false;
    }

    public override void UpdateState()
    {
        stateMachine.player.move.NoInput();
    }

    public override void ExitState()
    {

    }

    public override PlayerStateMachine.PlayerStates GetNextState() // Implement the logic to determine the next state from the this state
    {
        return StateKey;
    }
}
