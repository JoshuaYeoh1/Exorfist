using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerIdleState : BaseState<PlayerStateMachine.PlayerStates>
{
    PlayerStateMachine stateMachine;

    public PlayerIdleState(PlayerStateMachine stateMachine) : base(PlayerStateMachine.PlayerStates.Idle)
    {
        this.stateMachine = stateMachine;
    }

    public override void EnterState()
    {
        Debug.Log("Player state: " + stateMachine.GetCurrentState().StateKey);

        stateMachine.player.isAlive=true;
        stateMachine.player.canLook=true;
        stateMachine.player.canAttack=true;
        stateMachine.player.canBlock=true;
        stateMachine.player.canStun=true;
    }

    public override void UpdateState()
    {
        stateMachine.player.move.CheckInput();

        CheckMove();

        CheckCombat();
    }

    public override void ExitState()
    {

    }

    public override PlayerStateMachine.PlayerStates GetNextState() // Implement the logic to determine the next state from the this state
    {
        return StateKey;
    }

    void CheckMove()
    {
        if(stateMachine.player.move.dir.magnitude>0)
        {
            stateMachine.TransitionToState(PlayerStateMachine.PlayerStates.Move);
        }
    }

    void CheckCombat()
    {
        if(stateMachine.player.finder.target)
        {
            stateMachine.TransitionToState(PlayerStateMachine.PlayerStates.Combat);
        }
    }
}
