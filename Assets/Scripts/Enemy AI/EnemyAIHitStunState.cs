using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAIHitStunState : EnemyAIBaseState
{
    
    private float hitStunDuration;
    private float elapsedTime;
    private Coroutine hitStunTimerCoroutine;

    //So, a bit of context, why is the hitStunDuration set every time this "EnterState" function is called in regards to the hitStunState?
    //This is because we want the hitStun duration to REFRESH when the enemy receives damage.
    //Ideally this would cause the enemy to also be interrupted mid-attack animation, which we need to program as well.

    public override void EnterState(EnemyAIStateMachine enemy)
    {
        //change animations to "hitStun" animation
        hitStunDuration = enemy.thisEnemy.hitStunDuration;
        elapsedTime = 0f;
        
    }

    public override void ExitState(EnemyAIStateMachine enemy)
    {

    }

    public override void UpdateState(EnemyAIStateMachine enemy)
    {

    }

    private IEnumerator startHitStunTimer(EnemyAIStateMachine enemy)
    {
        while (elapsedTime < hitStunDuration)
        {
            if (Time.timeScale > 0f)
            {
                elapsedTime += Time.deltaTime;
            }
            yield return null;
        }

        Debug.Log("hitStun finished");
        enemy.SwitchState(enemy.inCombatState);
    }
}
