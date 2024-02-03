using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTemplateState : BaseState<PlayerStateMachine.PlayerStates>
{
    PlayerStateMachine stateMachine;

    public PlayerTemplateState(PlayerStateMachine stateMachine) : base(PlayerStateMachine.PlayerStates.Idle)
    {
        this.stateMachine = stateMachine;
    }

    public override void EnterState()
    {
        Debug.Log(stateMachine.GetCurrentState().StateKey);
    }

    public override void UpdateState()
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
