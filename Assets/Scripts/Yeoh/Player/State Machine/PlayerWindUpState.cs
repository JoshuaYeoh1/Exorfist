using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWindUpState : BaseState<PlayerStateMachine.PlayerStates>
{
    PlayerStateMachine stateMachine;

    public PlayerWindUpState(PlayerStateMachine stateMachine) : base(PlayerStateMachine.PlayerStates.WindUp)
    {
        this.stateMachine = stateMachine;
    }

    public override void EnterState()
    {
        Debug.Log("Player state: " + stateMachine.GetCurrentState().StateKey);

        stateMachine.player.canLook=true;
        stateMachine.player.canAttack=true;
        stateMachine.player.canBlock=true;
        stateMachine.player.canStun=true;
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
