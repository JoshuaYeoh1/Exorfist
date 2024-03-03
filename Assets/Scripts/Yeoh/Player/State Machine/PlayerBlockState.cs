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

        stateMachine.player.canMove=true;
<<<<<<< HEAD
=======
        stateMachine.player.canTurn=true;
>>>>>>> main
        stateMachine.player.canAttack=false;
        stateMachine.player.canBlock=true;
        stateMachine.player.canCast=false;
        stateMachine.player.canHurt=true;
        stateMachine.player.canStun=true;

        stateMachine.player.anim.SetBool("isBlocking", true);
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
        stateMachine.player.anim.SetBool("isBlocking", false);
    }

    public override PlayerStateMachine.PlayerStates GetNextState() // Implement the logic to determine the next state from the this state
    {
        return StateKey;
    }
}
