using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDeathState : BaseState<PlayerStateMachine.PlayerStates>
{
    PlayerStateMachine stateMachine;

    public PlayerDeathState(PlayerStateMachine stateMachine) : base(PlayerStateMachine.PlayerStates.Death)
    {
        this.stateMachine = stateMachine;
    }

    public override void EnterState()
    {
        Debug.Log("Player state: " + stateMachine.GetCurrentState().StateKey);

        stateMachine.player.isAlive=false;
        stateMachine.player.canAttack=false;
        stateMachine.player.canBlock=false;
        stateMachine.player.canStun=false;
    }

    public override void UpdateState()
    {
        stateMachine.player.move.NoInput();
    }

    public override void ExitState()
    {
        stateMachine.player.isAlive=true;
    }

    public override PlayerStateMachine.PlayerStates GetNextState() // Implement the logic to determine the next state from the this state
    {
        return StateKey;
    }
}
