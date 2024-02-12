using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//This state machine script will store a bulk of the data that gets passed into the different enemy states.
//So it will hold things like the animator component too.
public class EnemyAIStateMachine : MonoBehaviour
{
    //Basic Declarations
    EnemyAIBaseState currentState;
    [HideInInspector] public EnemyAI thisEnemy;
    [HideInInspector] public EnemyBehaviourManager bm;

    //==States==//
    public EnemyAIInCombatState inCombatState = new EnemyAIInCombatState();
    public EnemyAIIdleState idleState = new EnemyAIIdleState();
    public EnemyAIAttackingState attackingState = new EnemyAIAttackingState();
    public EnemyAIHitStunState hitStunState = new EnemyAIHitStunState();
    public EnemyAIMovingState movingState = new EnemyAIMovingState();
    //==States==//

    private void Awake()
    {
        thisEnemy = GetComponent<EnemyAI>();
        bm = GetComponent<EnemyBehaviourManager>();
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
        currentState.ExitState(this);
        currentState = state;
        currentState.EnterState(this);
    }

    //So you might be wondering why THIS is here
    //This is because the enemy, at any point, can switchi nto the HitStunState
    //This is so that the player can interrupt their movement as well as their attacks :)
    public void HitStunSwitchState(EnemyAIBaseState state)
    {
        
        currentState = hitStunState;
        SwitchState(hitStunState);
        bm.currentCoroutine = null;
        thisEnemy.SetIsMoving(false);
        thisEnemy.agent.SetDestination(thisEnemy.transform.position); //stops enemy from moving        
    }
}
