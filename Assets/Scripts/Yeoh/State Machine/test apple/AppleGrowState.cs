using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AppleGrowState : BaseState<AppleStateMachine.AppleStates>
{
    AppleStateMachine stateMachine;

    public AppleGrowState(AppleStateMachine stateMachine) : base(AppleStateMachine.AppleStates.Grow)
    {
        this.stateMachine = stateMachine;
    }

    public override void EnterState()
    {
        stateMachine.transform.localScale = stateMachine.apple.startAppleSize;
    }

    public override void UpdateState()
    {
        if(stateMachine.transform.localScale.x < .3)
        {
            stateMachine.transform.localScale += stateMachine.apple.growAppleScalar * Time.deltaTime;
        }
        else
        {
            stateMachine.TransitionToState(AppleStateMachine.AppleStates.Idle);
        }
    }

    public override void ExitState()
    {

    }

    public override AppleStateMachine.AppleStates GetNextState() // Implement the logic to determine the next state from the Grow state
    {
        // return AppleStateMachine.AppleStates.Idle;
        return StateKey;
    }
}
