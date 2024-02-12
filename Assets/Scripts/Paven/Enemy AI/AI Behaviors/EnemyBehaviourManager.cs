using System.Collections;
using UnityEngine;

public class EnemyBehaviourManager : MonoBehaviour
{
    private EnemyAIStateMachine sm;
    private EnemyAI self;
    public Coroutine currentCoroutine;

    
    [SerializeField] private string currentAnimName;
    [SerializeField] private bool currentAnimBool;

    private void Start()
    {
        sm = GetComponent<EnemyAIStateMachine>();
        self = sm.GetComponent<EnemyAI>();
    }

    //Move then attack coroutine//
    private IEnumerator MoveTowardsThenAttack()
    {
       
        sm.SwitchState(sm.movingState);
        
        sm.movingState.MoveTowardsPlayerWithLimits(sm);
        sm.thisEnemy.SetPreparedAttack(true);
        
        float dist = Vector3.Distance(self.transform.position, self.playerTransform.position);
        
        //if close enough to player, attack them
        if(dist <= self.GetClosePlayerRadius())
        {
            sm.SwitchState(sm.attackingState);
            sm.attackingState.PunchPlayer(sm);

            //stop coroutine execution early, Coroutine gets set back to "null" once the punch animation ends, or if the player enters the HitStun state.
            //the coroutine being set back to null is using an animation event WITHIN the animator component itself. Remember to set it there or else the EnemyAI will bug out!
            yield return null;
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
            sm.SwitchState(sm.inCombatState);
            StopActiveCoroutine();
        }
        else
        {
            //Once player is in range of the ClosePlayerRadius, stop moving and attack
            sm.SwitchState(sm.attackingState);
            //sm.thisEnemy.SetPreparingAttack(false);
            sm.attackingState.PunchPlayer(sm);
            //
        }
    }
    public void StartMoveTowardsThenAttack()
    {
        if (currentCoroutine == null)
        {
            currentCoroutine = StartCoroutine(MoveTowardsThenAttack());
        }
    }

    private IEnumerator CirclePlayerForShortDuration()
    {
        //StopActiveCoroutine();
        Debug.Log("Executing CirclePlayer");
        sm.SwitchState(sm.movingState);
        sm.movingState.CircleAroundPlayerRight(sm);
        self.SetIsMoving(true);
        self.SetNavMeshSpeed(self.GetCircleSpeed());
        self.animator.SetBool("CirclingPlayer", true);

        yield return new WaitForSeconds(4f);

        sm.movingState.StopMoving(sm);
        self.SetIsMoving(false);
        sm.SwitchState(sm.inCombatState);
        self.animator.SetBool("CirclingPlayer", false);
        self.SetNavMeshSpeed(self.GetDefaultSpeed());
        StopActiveCoroutine();
    }
    public void StartCirclePlayerForDuration()
    {
        if (currentCoroutine == null)
        {
            currentCoroutine = StartCoroutine(CirclePlayerForShortDuration());
        }
    }

    private IEnumerator MoveAwayFromPlayerWithLimits()
    {
        float dist = Vector3.Distance(self.transform.position, self.playerTransform.position);

        if (dist >= self.GetFarPlayerRadius())
        {
            self.animator.SetBool("MovingAwayFromPlayer", false);
            yield return null;
            StopActiveCoroutine();
        }
        else
        {
            Debug.Log("Executing MoveAway from player");
            sm.SwitchState(sm.movingState);
            sm.movingState.MoveAwayFromPlayerWithLimits(sm);
            self.SetIsMoving(true);
            while (self.GetIsMoving() != false)
            {
                yield return null;
            }
            Debug.Log("Coroutine ended");
            self.animator.SetBool("MovingAwayFromPlayer", false);
            StopActiveCoroutine();
        } //stop coroutine at end and set current coroutine back to null
    }
    public void StartMoveAwayFromPlayerWithLimits()
    {
        if(currentCoroutine == null)
        {
            currentCoroutine = StartCoroutine(MoveAwayFromPlayerWithLimits());
        }
    }

    private IEnumerator MaintainDistanceWithPlayerForShortDuration()
    {
        sm.movingState.MaintainDistanceWithPlayer(sm);
        sm.SwitchState(sm.movingState);
        self.animator.SetBool("MovingTowardsPlayer", false);
        yield return new WaitForSeconds(4f);
        Debug.Log("Coroutine ended");
        sm.SwitchState(sm.inCombatState);
        StopActiveCoroutine();
    }
    public void StartMaintainDistanceWithPlayerForShortDuration()
    {
        if(currentCoroutine == null)
        {
            currentCoroutine = StartCoroutine(MaintainDistanceWithPlayerForShortDuration());
        }
    }
    public void StopActiveCoroutine()
    {
        if(currentCoroutine != null)
        {
            StopCoroutine(currentCoroutine);
            currentCoroutine = null;
        }
        else
        {
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
