using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Abstract class that acts as a template to setup every other state the EnemyAI has
[System.Serializable]
public abstract class EnemyAIBaseState
{
    //this function runs when the enemy ENTERS a specific state
    public abstract void EnterState(EnemyAIStateMachine enemy);

    //this function runs when the enemy is STILL in the specific state (think of it as a localized update function)
    public abstract void UpdateState(EnemyAIStateMachine enemy);

    //This function runs when the enemy EXITS a specific state
    public abstract void ExitState(EnemyAIStateMachine enemy);
}
