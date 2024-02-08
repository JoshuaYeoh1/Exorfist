using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAIIdleState : EnemyAIBaseState
{
    public override void EnterState(EnemyAIStateMachine enemy)
    {
        Debug.Log("Enemy is Idle");
    }

    public override void ExitState(EnemyAIStateMachine enemy)
    {

    }

    public override void UpdateState(EnemyAIStateMachine enemy)
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            enemy.SwitchState(enemy.movingState);
        }

        
    }
}
