using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlamAttackScript : MonoBehaviour
{
    private EnemyAI thisEnemy;
    private float initialY;

    [SerializeField] private GameObject warningVFXPrefab;

    private GameObject warningVFXInstance;
    private Vector3 warningVFXScale;
    [SerializeField] float slamRadius;

    //this is to offset the final destination of the leap, relative to the orientation of the enemy
    [SerializeField] float leapOffset;
    //stopRadius dictates how close the enemy has to be to the slam point in order to stop moving
    [SerializeField] float stopRadius;

    Vector3 offsetPos;
    // Start is called before the first frame update
    void Start()
    {
        thisEnemy = GetComponent<EnemyAI>();
        warningVFXScale = warningVFXPrefab.transform.localScale;
        initialY = thisEnemy.transform.localPosition.y;
    }

    private void OnDestroy()
    {
        DestroyVFXInstance();
    }

    private void LeapTween()
    {
        //set navmesh agent speed to 0 to allow LeanTween to move the gameObject instead
        thisEnemy.agent.speed = 0;
        LeanTween.move(thisEnemy.gameObject, offsetPos, 0.3f).setEaseInOutSine();
        thisEnemy.agent.SetDestination(offsetPos);
    }

    private void CalculateOffsetPos()
    {
        Vector3 playerPos = thisEnemy.playerTransform.position;
        warningVFXPrefab.transform.localScale = Vector3.zero;

        //calculating direction from player to enemy
        Vector3 directionToPlayer = (thisEnemy.transform.position - playerPos).normalized;

        offsetPos = playerPos + directionToPlayer * leapOffset;
        Vector3 offsetPosVFX = new Vector3(offsetPos.x, offsetPos.y + 0.25f, offsetPos.z);
        warningVFXInstance = Instantiate(warningVFXPrefab, offsetPosVFX, thisEnemy.transform.rotation);
        offsetPos.y = initialY;
        LeanTween.scale(warningVFXInstance, warningVFXScale, 0.2f);
        warningVFXPrefab.transform.localScale = warningVFXScale;

        offsetPos = playerPos + directionToPlayer * leapOffset;

    }
    private void OnHitStun(ControllerColliderHit hit)
    {
        Destroy(warningVFXInstance);
    }

    private void DestroyVFXInstance()
    {
        if(warningVFXInstance != null)
        {
            Destroy(warningVFXInstance);
        }
    }
}
