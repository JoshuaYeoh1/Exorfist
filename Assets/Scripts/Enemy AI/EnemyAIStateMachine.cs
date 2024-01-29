using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//This state machine script will store a bulk of the data that gets passed into the different enemy states.
//So it will hold things like the animator component too.
public class EnemyAIStateMachine : MonoBehaviour
{
    //Basic Declarations
    EnemyAIBaseState currentState;
    public EnemyAI thisEnemy;

    //==Concrete States==//
    //(AKA, only ONE or the other state can be active)
    public EnemyAIInCombatState inCombatState = new EnemyAIInCombatState();
    public EnemyAIOutOfCombatState outOfCombatState = new EnemyAIOutOfCombatState();
    //==Concrete States==//

    //==Sub-states==//
    public EnemyAIIdleState idleState = new EnemyAIIdleState();
    public EnemyAIAttackingState attackingState = new EnemyAIAttackingState();
    public EnemyAIBalanceBrokenState balanceBrokenState = new EnemyAIBalanceBrokenState();
    public EnemyAIHitStunState hitStunState = new EnemyAIHitStunState();
    public EnemyAIMovingState movingState = new EnemyAIMovingState();
    //==Sub-states==//

    private void Awake()
    {
        thisEnemy = GetComponent<EnemyAI>();
    }

    private void Start()
    {
        currentState = idleState;

        currentState.EnterState(this);
    }

    private void Update()
    {
        currentState.UpdateState(this);
    }

    //SwitchState function in order to call proper stuff
    public void SwitchState(EnemyAIBaseState state)
    {
        currentState = state;
        currentState.EnterState(this);
    }
}