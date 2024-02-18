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
        Debug.Log("Player state: " + stateMachine.GetCurrentState().StateKey);

        stateMachine.player.canAttack=true;
        stateMachine.player.canBlock=true;
        stateMachine.player.canCast=true;
        stateMachine.player.canHurt=true;
        stateMachine.player.canStun=true;
    }

    public override void UpdateState()
    {
        stateMachine.player.move.CheckInput();
    }

    public override void FixedUpdateState()
    {
        stateMachine.player.look.CheckLook();
    }

    public override void ExitState()
    {

    }

    public override PlayerStateMachine.PlayerStates GetNextState() // Implement the logic to determine the next state from the this state
    {
        return StateKey;
    }
}
