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
        
        Debug.Log("Executing moveTowardsThenAttack");
        sm.SwitchState(sm.movingState);
        
        sm.movingState.MoveTowardsPlayerWithLimits(sm);
        sm.thisEnemy.SetPreparedAttack(true);
        //Debug.Log(sm.thisEnemy.GetPreparedAttack());
        float dist = Vector3.Distance(self.transform.position, self.playerTransform.position);
        Debug.Log(dist);
        //if close enough to player, attack them
        if(dist <= self.GetClosePlayerRadius())
        {
            sm.SwitchState(sm.attackingState);
            //sm.thisEnemy.SetPreparingAttack(false);
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
