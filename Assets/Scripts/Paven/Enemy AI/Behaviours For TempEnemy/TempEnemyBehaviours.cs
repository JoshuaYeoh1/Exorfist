using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
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
        if(self.GetIsHitStun() == true)
        {
            StopActiveCoroutine();
            self.SetPreparedAttack(false);
            self.SetIsAttacking(false);
        }
        if(Input.GetKeyDown(KeyCode.V))
        {
            Debug.Log("V pressed");
            if(currentCoroutine == null)
            {
                currentCoroutine = StartCoroutine(MoveTowardsThenAttack());
            }
        }

        if (Input.GetKeyDown(KeyCode.B))
        {
            Debug.Log("B pressed");
            self.animator.SetBool("inCombat", true);
            self.animator.SetBool("MovingAwayFromPlayer", true);
            self.SetIsMoving(true);
            sm.movingState.MoveAwayFromPlayerWithLimits(sm);
            sm.SwitchState(sm.movingState);
        }
    }

    //Move then attack coroutine//
    private IEnumerator MoveTowardsThenAttack()
    {
        Debug.Log("Executing moveTowardsThenAttack");
        sm.SwitchState(sm.movingState);
        
        sm.movingState.MoveTowardsPlayerWithLimits(sm);
        sm.thisEnemy.SetPreparedAttack(true);
        Debug.Log(sm.thisEnemy.GetPreparedAttack());
        float dist = Vector3.Distance(self.transform.position, self.playerTransform.position);

        if(dist <= self.GetClosePlayerRadius())
        {
            sm.SwitchState(sm.attackingState);
            //sm.thisEnemy.SetPreparingAttack(false);
            sm.attackingState.PunchPlayer(sm);
            StopActiveCoroutine();            
        }

        while(self.GetIsMoving() != false)
        {
            //Don't execute next block until player stops moving
            yield return null;
        }

        //So, why is this check here? This is because the PreparingAttack bool can be flipped by other components if needed, and coroutines don't always stop either.
        if(sm.thisEnemy.GetPreparedAttack() != true)
        {
            //Debug.Log("Stopping MoveTowardsThenAttack");
            StopActiveCoroutine();
            sm.SwitchState(sm.inCombatState);
        }
        else
        {
            sm.SwitchState(sm.attackingState);
            //sm.thisEnemy.SetPreparingAttack(false);
            sm.attackingState.PunchPlayer(sm);
            StopActiveCoroutine();
        }
    }

    public void StopActiveCoroutine()
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
