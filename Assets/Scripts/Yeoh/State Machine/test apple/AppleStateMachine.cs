using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AppleStateMachine : StateMachine<AppleStateMachine.AppleStates>
{
    public enum AppleStates
    {
        Grow,
        Idle,
        Chewed,
        Rotten,
    }

    [HideInInspector] public Apple apple;

    void Awake()
    {
        apple=GetComponent<Apple>();

        AppleGrowState growState = new AppleGrowState(this);
        AppleIdleState idleState = new AppleIdleState(this);

        States.Add(AppleStates.Grow, growState);
        States.Add(AppleStates.Idle, idleState);

        CurrentState = States[AppleStates.Grow];
    }
}
