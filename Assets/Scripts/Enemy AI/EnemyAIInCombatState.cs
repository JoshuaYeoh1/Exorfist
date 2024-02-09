using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAIInCombatState : EnemyAIBaseState
{
    //logic for handling cases when the EnemyAI is in combat alongside it's brethren. This is essentially a "combat idle" state.

    public override void EnterState(EnemyAIStateMachine enemy)
    {
        enemy.thisEnemy.SetIsMoving(false);
        enemy.thisEnemy.animator.SetBool("MovingAwayFromPlayer", false);
    }

    public override void ExitState(EnemyAIStateMachine enemy)
    {
        //do nothing lmao
    }

    public override void UpdateState(EnemyAIStateMachine enemy)
    {
        enemy.thisEnemy.transform.LookAt(enemy.thisEnemy.playerTransform.position);
    }
}
