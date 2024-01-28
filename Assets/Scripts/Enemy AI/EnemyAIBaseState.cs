using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Abstract class that acts as a template to setup every other state the EnemyAI has
public abstract class EnemyAIBaseState
{
    public abstract void EnterState(EnemyAIStateMachine enemy);

    public abstract void ExitState(EnemyAIStateMachine enemy);
}
