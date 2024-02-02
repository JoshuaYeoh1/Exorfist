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

    void OnCollisionEnter(Collision other)
    {
        CurrentState.OnCollisionEnter(other);
    }

    void OnCollisionStay(Collision other)
    {
        CurrentState.OnCollisionStay(other);
    }

    void OnCollisionExit(Collision other)
    {
        CurrentState.OnCollisionExit(other);
    }

    void OnTriggerEnter(Collider other)
    {
        CurrentState.OnTriggerEnter(other);
    }

    void OnTriggerStay(Collider other)
    {
        CurrentState.OnTriggerStay(other);
    }

    void OnTriggerExit(Collider other)
    {
        CurrentState.OnTriggerExit(other);
    }
}
