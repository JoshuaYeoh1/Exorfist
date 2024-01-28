using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//This state machine script will store a bulk of the data that gets passed into the different enemy states.
//So it will hold things like the animator component, as well as any data related to that
public class EnemyAIStateMachine : MonoBehaviour
{
    EnemyAIBaseState currentState;
    public EnemyAI thisEnemy;
}
