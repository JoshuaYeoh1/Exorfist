using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStunState : BaseState<PlayerStateMachine.PlayerStates>
{
    PlayerStateMachine stateMachine;

    public PlayerStunState(PlayerStateMachine stateMachine) : base(PlayerStateMachine.PlayerStates.Stun)
    {
        this.stateMachine = stateMachine;
    }

    public override void EnterState()
    {
        Debug.Log("Player state: " + stateMachine.GetCurrentState().StateKey);

        stateMachine.player.canMove=true;
<<<<<<< HEAD
=======
        stateMachine.player.canTurn=false;
>>>>>>> main
        stateMachine.player.canAttack=false;
        stateMachine.player.canBlock=true;
        stateMachine.player.canCast=false;
        stateMachine.player.canHurt=true;
        stateMachine.player.canStun=true; 
    }

    public override void UpdateState()
    {

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
