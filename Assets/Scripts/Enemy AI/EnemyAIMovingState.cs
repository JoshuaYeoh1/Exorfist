using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAIMovingState : EnemyAIBaseState
{
    public override void EnterState(EnemyAIStateMachine enemy)
    {
        enemy.thisEnemy.setIsMoving(true);
        
    }

    public override void ExitState(EnemyAIStateMachine enemy)
    {
        enemy.thisEnemy.setIsMoving(false);
    }

    public override void UpdateState(EnemyAIStateMachine enemy)
    {
        
    }
}
