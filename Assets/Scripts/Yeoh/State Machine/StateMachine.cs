using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public abstract class StateMachine<EState> : MonoBehaviour where EState : Enum
{
    protected Dictionary<EState, BaseState<EState>> States = new Dictionary<EState, BaseState<EState>>();

    protected BaseState<EState> CurrentState;

    protected bool isTransitioningState;

    void Start()
    {
        CurrentState.EnterState();
    }

    void Update()
    {
        EState NextStateKey = CurrentState.GetNextState();

        if(NextStateKey.Equals(CurrentState.StateKey))
        {
            CurrentState.UpdateState();
        }
        else if(!isTransitioningState)
        {
            TransitionToState(NextStateKey);
        }
    }

    public void TransitionToState(EState StateKey)
    {
        isTransitioningState=true;

        CurrentState.ExitState();

        CurrentState = States[StateKey];

        CurrentState.EnterState();

        isTransitioningState=false;
    }
}
