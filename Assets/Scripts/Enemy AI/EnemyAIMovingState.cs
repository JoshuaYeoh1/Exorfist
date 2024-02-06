using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    }

    public override void ExitState(EnemyAIStateMachine enemy)
    {
        Debug.Log("Exiting move state");
        enemy.thisEnemy.SetIsMoving(false);
    }

    public override void UpdateState(EnemyAIStateMachine enemy)
    {
        MoveAwayFromPlayer(enemy);
    }

    //circling behaviour should only be called if an enemy is CLOSE to the player.
    void CircleAroundPlayerRight(EnemyAIStateMachine enemy)
    {
        //Null check
        if (enemy.thisEnemy.playerTransform == null)
        {
            Debug.LogError("Player reference is null, doing nothing.");
            return;
        }
        float targetPositionDist = CalcDistanceToPlayer(enemy);
        if (targetPositionDist >= enemy.thisEnemy.GetCircleRadius())
        {
            //Debug.Log("Player is too far to circle around. Moving closer instead");
            MoveTowardsPlayer(enemy);
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

        //Debug.Log(targetPositionDist);
        //Check if the target player is too far to "circle" around.
        if (targetPositionDist >= enemy.thisEnemy.GetCircleRadius())
        {
            //Debug.Log("Player is too far to circle around. Moving closer instead");
            MoveTowardsPlayer(enemy);
            return;
        }

        enemy.thisEnemy.agent.SetDestination(targetPosition);
    }

    void MoveAwayFromPlayerWithLimits(EnemyAIStateMachine enemy)
    {
        Debug.Log("Moving away from player");
        //enemy.thisEnemy.SetIsMoving(true);
        if(enemy.thisEnemy.GetIsMoving() == false)
        {
            //enemy.thisEnemy.SetIsMoving(true);
            //Wow I wonder what this check is
            if (enemy.thisEnemy.playerTransform == null)
            {
                Debug.LogError("Player reference is null");
                enemy.thisEnemy.SetIsMoving(false);
                enemy.SwitchState(enemy.idleState);
            }

            //Rotate the model to be facing the player
            enemy.thisEnemy.transform.LookAt(enemy.thisEnemy.playerTransform);
            float dist = CalcDistanceToPlayer(enemy);

            //initialize variables for calculation and comparison
            float closePlayerRadius = enemy.thisEnemy.GetClosePlayerRadius();
            float farPlayerRadius = enemy.thisEnemy.GetFarPlayerRadius();

            if (dist < closePlayerRadius)
            {
                MoveAwayFromPlayer(enemy);
            }

            if (dist <= farPlayerRadius)
            {
                MoveAwayFromPlayer(enemy);
            }

            if(dist > farPlayerRadius)
            {
                Debug.Log("Enemy is too far, stopping movement");
                enemy.thisEnemy.agent.SetDestination(enemy.thisEnemy.transform.position);
                enemy.thisEnemy.SetIsMoving(false);
                enemy.SwitchState(enemy.inCombatState);
            }            
        } 
    }

    void MoveTowardsPlayer(EnemyAIStateMachine enemy)
    {
        Debug.Log("Moving towards player");
        //add animation for "moving towards player" as well
        enemy.thisEnemy.agent.SetDestination(enemy.thisEnemy.playerTransform.position);
        float dist = CalcDistanceToPlayer(enemy);
        if(dist < enemy.thisEnemy.GetClosePlayerRadius())
        {
            enemy.thisEnemy.agent.SetDestination(enemy.thisEnemy.transform.position);
            enemy.thisEnemy.SetIsMoving(false);
            //stop animation
            return;
        }
    }

    private float CalcDistanceToPlayer(EnemyAIStateMachine enemy)
    {
        float dist = Vector3.Distance(enemy.thisEnemy.transform.position, enemy.thisEnemy.playerTransform.position);
        return dist;
    }

    private void MoveAwayFromPlayer(EnemyAIStateMachine enemy)
    {
        Vector3 targetDirection;
        Vector3 targetPosition;
        if(enemy.thisEnemy.playerTransform == null)
        {
            Debug.Log("No player found in scene");
            return;
        }
        enemy.thisEnemy.transform.LookAt(enemy.thisEnemy.playerTransform.position);

        targetDirection = enemy.thisEnemy.transform.position - enemy.thisEnemy.playerTransform.position;
        targetPosition = enemy.thisEnemy.transform.position + targetDirection.normalized * enemy.thisEnemy.GetMoveAwayDistance();
        enemy.thisEnemy.agent.SetDestination(targetPosition);
    }
}
