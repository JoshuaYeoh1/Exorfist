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

        stateMachine.player.canMove=true;
<<<<<<< HEAD
=======
        stateMachine.player.canTurn=true;
>>>>>>> main
        stateMachine.player.canAttack=true;
        stateMachine.player.canBlock=true;
        stateMachine.player.canCast=true;
        stateMachine.player.canHurt=true;
        stateMachine.player.canStun=true;
    }

    public override void UpdateState()
    {

    }

    public override void FixedUpdateState()
    {
<<<<<<< HEAD
        stateMachine.player.look.CheckLook();
=======

>>>>>>> main
    }

    public override void ExitState()
    {

    }

    public override PlayerStateMachine.PlayerStates GetNextState() // Implement the logic to determine the next state from the this state
    {
        return StateKey;
    }
}
