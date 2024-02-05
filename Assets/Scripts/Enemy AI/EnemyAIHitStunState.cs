using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAIHitStunState : EnemyAIBaseState
{

    //So, a bit of context, why is the hitStunDuration set every time this "EnterState" function is called in regards to the hitStunState?
    //This is because we want the hitStun duration to REFRESH when the enemy receives damage.
    //Ideally this would cause the enemy to also be interrupted mid-attack animation, which we need to program as well.

    public override void EnterState(EnemyAIStateMachine enemy)
    {
        Debug.Log("HitStun state achieved");
        //change animations to "hitStun" animation
        enemy.thisEnemy.transform.LookAt(enemy.thisEnemy.playerTransform.position);
        enemy.thisEnemy.animator.Play("Hit Stun", -1, 0f);
        
        
    }

    public override void ExitState(EnemyAIStateMachine enemy)
    {
        Debug.Log("Exiting hit stun");
    }

    public override void UpdateState(EnemyAIStateMachine enemy)
    {
        if (enemy.thisEnemy.GetIsHitStun())
        {
            return;
        }
        else
        {
            enemy.SwitchState(enemy.inCombatState);
        }
    }
}