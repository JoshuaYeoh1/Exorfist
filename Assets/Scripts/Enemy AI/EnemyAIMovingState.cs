using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEditor.VersionControl;
using UnityEngine;
using UnityEngine.EventSystems;

public class EnemyAIMovingState : EnemyAIBaseState
{
    //Movement logic, as well as the methods that physically MOVE the enemy, are stored here.

    /*private bool isCooldown;
    private float cooldownDuration;
    private float directionChangeChance;*/

    public override void EnterState(EnemyAIStateMachine enemy)
    {
        Debug.Log("Entering move state");
      /*  isCooldown = false;
        cooldownDuration = 0.5f; */
        enemy.thisEnemy.SetIsMoving(true);
        
    }

    public override void ExitState(EnemyAIStateMachine enemy)
    {
        Debug.Log("Exiting move state");
        enemy.thisEnemy.SetIsMoving(false);
    }

    public override void UpdateState(EnemyAIStateMachine enemy)
    {
        CircleAroundPlayer(enemy);
    }

    void CircleAroundPlayer(EnemyAIStateMachine enemy)
    {
        //Null check
        if (enemy.thisEnemy.playerTransform == null)
        {
            Debug.LogError("Player reference is null");
            return;
        }

        //Rotate the model to be facing the player
        enemy.thisEnemy.transform.LookAt(enemy.thisEnemy.playerTransform);

        //Get the forward direction of the enemy (where it's facing)
        Vector3 forwardDirection = enemy.thisEnemy.transform.forward;

        //Calculate the angle perpendicular to the forward direction
        float angle = Mathf.Atan2(forwardDirection.z, forwardDirection.x) + Mathf.PI / 2f;

        //Calculate the perpendicular direction
        Vector3 perpendicularDirection = new Vector3(Mathf.Cos(angle), 0f, Mathf.Sin(angle));

        //Calculate the target position for the enemy with constant distance (using CircleRadius)
        Vector3 targetPosition = enemy.thisEnemy.playerTransform.position + perpendicularDirection * enemy.thisEnemy.GetCircleRadius();

        float targetPositionDist = Vector3.Distance(enemy.thisEnemy.transform.position, enemy.thisEnemy.playerTransform.position);
        Debug.Log(targetPositionDist);
        //Check if the target player is too far to "circle" around.
        if (targetPositionDist >= enemy.thisEnemy.GetCircleRadius())
        {
            //Debug.Log("Player is too far to circle around. Moving closer instead");
            MoveTowardsPlayer(enemy);
            return;
        }

        enemy.thisEnemy.agent.SetDestination(targetPosition);

        
    }

    void MoveAwayFromPlayer(EnemyAIStateMachine enemy)
    {
        //Wow I wonder what this check is
        if(enemy.thisEnemy.playerTransform == null)
        {
            Debug.LogError("Player reference is null");
            return;
        }

        //Storing forward direction
        Vector3 forwardDirection = enemy.thisEnemy.transform.forward;

        //Find the new distance to move to
    }

    void MoveTowardsPlayer(EnemyAIStateMachine enemy)
    {
        Debug.Log("Moving towards player");
        //add animation for "moving towards player" as well
        enemy.thisEnemy.agent.SetDestination(enemy.thisEnemy.playerTransform.position);
    }

    
}
