using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCombatState : BaseState<PlayerStateMachine.PlayerStates>
{
    PlayerStateMachine stateMachine;

    public PlayerCombatState(PlayerStateMachine stateMachine) : base(PlayerStateMachine.PlayerStates.Combat)
    {
        this.stateMachine = stateMachine;
    }

    public override void EnterState()
    {
        Debug.Log("Player state: " + stateMachine.GetCurrentState().StateKey);

        stateMachine.player.canAttack=true;
    }

    public override void UpdateState()
    {
        stateMachine.player.move.CheckInput();

        CheckNoCombat();
    }

    public override void ExitState()
    {

    }

    public override PlayerStateMachine.PlayerStates GetNextState() // Implement the logic to determine the next state from the this state
    {
        return StateKey;
    }

    void CheckNoCombat()
    {
        if(!stateMachine.player.finder.target)
        {
            stateMachine.TransitionToState(PlayerStateMachine.PlayerStates.Idle);
        }
    }
}
