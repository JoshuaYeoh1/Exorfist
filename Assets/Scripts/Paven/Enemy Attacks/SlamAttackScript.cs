using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlamAttackScript : MonoBehaviour
{
    private EnemyAI thisEnemy;
    private GameObject warningVFX;

    [SerializeField] float slamRadius;

    //stopRadius dictates how close the enemy has to be to the slam point in order to stop moving
    [SerializeField] float stopRadius;

    private bool destinationReached;

    // Start is called before the first frame update
    void Start()
    {
        thisEnemy = GetComponent<EnemyAI>();
    }

    //navmeshAgent needs to move in this script
    private void LeapToPlayer()
    {
        destinationReached = false;

        thisEnemy.agent.SetDestination(thisEnemy.playerTransform.position);
        StartCoroutine(LeapAccelerate());
        StartCoroutine(PosCheck());
        //Instantiate(warningVFX, thisEnemy.playerTransform.position, thisEnemy.transform.rotation);
    }

    //
    private IEnumerator LeapAccelerate()
    {
        while( destinationReached == false)
        {
            thisEnemy.agent.speed *= 1.1f;
            yield return new WaitForSeconds(0.1f);
        }
        //broke out of while loop, set speed back to normal
        thisEnemy.agent.speed = thisEnemy.GetDefaultSpeed();
    }

    private IEnumerator PosCheck()
    {
        while (Vector3.Distance(thisEnemy.playerTransform.position, thisEnemy.transform.position) >= stopRadius)
        {
            yield return null;
        }
        //broke out of while loop, destination reached;
        destinationReached = true;
    }
}
