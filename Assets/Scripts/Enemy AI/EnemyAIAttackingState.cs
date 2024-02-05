using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAIAttackingState : EnemyAIBaseState
{
    //This is the state that handles all the logic callback for when the attack is finished, and thus it will transition into the next state after the enemy has finished their attack animation.

    //variable to hold attack animation
    public override void EnterState(EnemyAIStateMachine enemy)
    {
        enemy.thisEnemy.SetPreparingAttack(true);
        enemy.thisEnemy.animator.SetBool("inCombat", true);
        PunchPlayer(enemy);
    }

    public override void ExitState(EnemyAIStateMachine enemy)
    {
        enemy.thisEnemy.SetPreparingAttack(false);
        enemy.thisEnemy.SetIsAttacking(false);
    }

    public override void UpdateState(EnemyAIStateMachine enemy)
    {
        
    }

    //isAttacking boolean is handled by animation events
    public void PunchPlayer(EnemyAIStateMachine enemy)
    {
        enemy.thisEnemy.transform.LookAt(enemy.thisEnemy.playerTransform.position);
        enemy.thisEnemy.animator.SetBool("Punching", true);
    }
}
