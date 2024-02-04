using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AppleIdleState : BaseState<AppleStateMachine.AppleStates>
{
    AppleStateMachine stateMachine;

    public AppleIdleState(AppleStateMachine stateMachine) : base(AppleStateMachine.AppleStates.Idle)
    {
        this.stateMachine = stateMachine;
    }

    public override void EnterState()
    {
        stateMachine.GetComponent<Rigidbody>().isKinematic=false;
    }

    public override void UpdateState()
    {

    }

    public override void ExitState()
    {

    }

    public override AppleStateMachine.AppleStates GetNextState() // Implement the logic to determine the next state from the Grow state
    {
        return StateKey;
    }
}
