using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//This class is entirely dedicated to managing the HitStun time variable in the EnemyAI script
//because coroutines can only be used in a script that derives from MonoBehaviour :(

public class EnemyAIHitStunTimer : MonoBehaviour
{
    private EnemyAIStateMachine thisSM;
    private EnemyAI thisEnemy;

    private void Awake()
    {
        thisSM = gameObject.GetComponent<EnemyAIStateMachine>();
        if(thisSM != null)
        {
            thisEnemy = thisSM.thisEnemy;
        }
        else
        {
            Debug.LogError("thisSM is null, hitstun cannot be applied or managed");
        }
    }
    
}
