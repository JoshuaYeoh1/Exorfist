using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlamAttackScript : MonoBehaviour
{
    private EnemyAI thisEnemy;


    [SerializeField] private GameObject warningVFXPrefab;

    private GameObject warningVFXInstance;

    [SerializeField] float slamRadius;

    //this is to offset the final destination of the leap, relative to the orientation of the enemy
    [SerializeField] float leapOffset;
    //stopRadius dictates how close the enemy has to be to the slam point in order to stop moving
    [SerializeField] float stopRadius;

    // Start is called before the first frame update
    void Start()
    {
        thisEnemy = GetComponent<EnemyAI>();
    }

    //navmeshAgent needs to move in this script
    private void LeapToPlayer()
    {
        LeapTween();
    }


    private void LeapTween()
    {
        Vector3 playerPos = thisEnemy.playerTransform.position;
        float defaultAccel = thisEnemy.agent.acceleration;
        thisEnemy.agent.speed = 0;

        //calculating direction from player to enemy
        Vector3 directionToPlayer = (thisEnemy.transform.position - playerPos).normalized;

        Vector3 offsetPos = playerPos + directionToPlayer * leapOffset;
        //Instantiate(warningVFX, offsetPos, thisEnemy.transform.rotation);
        LeanTween.move(thisEnemy.gameObject, offsetPos, 0.3f);
    }

    private void OnHitStun(ControllerColliderHit hit)
    {
        if(warningVFXInstance != null)
        {
            Destroy(warningVFXInstance);
        }
    }
}
