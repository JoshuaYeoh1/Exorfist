using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TempEnemyBehaviours : MonoBehaviour
{
    private EnemyAIStateMachine sm;
    private EnemyAI self;
    Coroutine currentCoroutine;

    
    [SerializeField] private string currentAnimName;
    [SerializeField] private bool currentAnimBool;

    private void Start()
    {
        sm = GetComponent<EnemyAIStateMachine>();
        self = sm.GetComponent<EnemyAI>();
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.V))
        {
            Debug.Log("V pressed");
            if(currentCoroutine == null)
            {
                currentCoroutine = StartCoroutine(MoveTowardsThenAttack());
            }
        }

         
    }

    //Move then attack coroutine//
    private IEnumerator MoveTowardsThenAttack()
    {
        Debug.Log("Executing moveTowardsThenAttack");
        sm.SwitchState(sm.movingState);
        
        sm.movingState.MoveTowardsPlayer(sm);
        sm.thisEnemy.SetPreparedAttack(true);
        Debug.Log(sm.thisEnemy.GetPreparedAttack());

        while(self.GetIsMoving() != false)
        {
            //Don't execute next block until player stops moving
            yield return null;
        }

        //So, why is this check here? This is because the PreparingAttack bool can be flipped by other components if needed, and coroutines don't always stop either.
        if(sm.thisEnemy.GetPreparedAttack() != true)
        {
            //Debug.Log("Stopping MoveTowardsThenAttack");
            StopMoveTowardsThenAttack();
            sm.SwitchState(sm.inCombatState);
        }
        else
        {
            sm.SwitchState(sm.attackingState);
            //sm.thisEnemy.SetPreparingAttack(false);
            sm.attackingState.PunchPlayer(sm);
            StopMoveTowardsThenAttack();
        }
    }

    public void StopMoveTowardsThenAttack()
    {
        if(currentCoroutine != null)
        {
            StopCoroutine(currentCoroutine);
            currentCoroutine = null;
        }
    }


    //Called in animation events
    public void SetCurrentAnimatorBoolState()
    {
        sm.thisEnemy.animator.SetBool(currentAnimName, currentAnimBool);
    }

    public void SetCurrentAnimName(string name)
    {
        currentAnimName = name;
    }

    public void SetCurrentAnimBool(bool input)
    {
        currentAnimBool = input;
    }
}
