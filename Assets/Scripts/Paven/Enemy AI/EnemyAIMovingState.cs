using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAIMovingState : EnemyAIBaseState
{
    //Movement logic, as well as the methods that physically MOVE the enemy, are stored here.

    /*private bool isCooldown;
    private float cooldownDuration;
    private float directionChangeChance;*/
    private int movementIndex; //dictates the specific movement code that should execute;

    public override void EnterState(EnemyAIStateMachine enemy)
    {
        enemy.thisEnemy.SetIsMoving(true);
        Debug.Log("Entering move state");
        /*  isCooldown = false;
          cooldownDuration = 0.5f; */
    }

    public override void ExitState(EnemyAIStateMachine enemy)
    {
        //Debug.Log("Exiting move state");
        enemy.thisEnemy.SetIsMoving(false);
    }

    public override void UpdateState(EnemyAIStateMachine enemy)
    {
        switch (movementIndex)
        {
            case 0:

                break;
            case 1:
                MoveTowardsPlayer(enemy);
                break;
            case 2:
                MoveAwayFromPlayerWithLimits(enemy);
                break;
            case 3:
                CircleAroundPlayerRight(enemy);
                break;
            default:
                ForceBreakOutOfMoveState(enemy);
                break;

        }
    }

    //circling behaviour should only be called if an enemy is CLOSE to the player.
    void CircleAroundPlayerRight(EnemyAIStateMachine enemy)
    {
        movementIndex = 3;
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

        /*
        if (targetPositionDist >= enemy.thisEnemy.GetCircleRadius())
        {
            //Debug.Log("Player is too far to circle around. Moving closer instead");
            movementIndex = 1;
            MoveTowardsPlayer(enemy);
            return;
        }
        */

        enemy.thisEnemy.agent.SetDestination(targetPosition);
    }

    public void MoveAwayFromPlayerWithLimits(EnemyAIStateMachine enemy)
    {
        //Debug.Log("Moving away from player");
        movementIndex = 2;
        //enemy.thisEnemy.SetIsMoving(true);
        if (enemy.thisEnemy.GetIsMoving() == true)
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
            enemy.thisEnemy.transform.LookAt(enemy.thisEnemy.playerTransform.position);
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

            if (dist > farPlayerRadius)
            {
                Debug.Log("Enemy is too far, stopping movement");
                enemy.thisEnemy.agent.SetDestination(enemy.thisEnemy.transform.position);
                enemy.thisEnemy.SetIsMoving(false);
                enemy.SwitchState(enemy.inCombatState);
            }
        }
        else
        {
            Debug.LogError("IsMoving is false, swapping to combat state");
            enemy.SwitchState(enemy.inCombatState);
        }
    }

    public void MoveTowardsPlayer(EnemyAIStateMachine enemy)
    {
        float dist = CalcDistanceToPlayer(enemy);
        //Debug.Log(dist);
        if (dist < enemy.thisEnemy.GetClosePlayerRadius())
        {
            //Debug.Log("executing if code");
            movementIndex = 0;
            enemy.thisEnemy.transform.LookAt(enemy.thisEnemy.playerTransform.position);
            enemy.thisEnemy.animator.SetBool("MovingTowardsPlayer", false);
            enemy.thisEnemy.agent.SetDestination(enemy.thisEnemy.transform.position);
            enemy.thisEnemy.SetIsMoving(false);
            //stop animation
            return;
        }
        else
        {
            //add animation for "moving towards player" as well
            //Debug.Log("Executing else code");
            movementIndex = 1;
            enemy.thisEnemy.transform.LookAt(enemy.thisEnemy.playerTransform.position);
            enemy.thisEnemy.agent.SetDestination(enemy.thisEnemy.playerTransform.position);
            enemy.thisEnemy.animator.SetBool("inCombat", true);
            enemy.thisEnemy.animator.SetBool("MovingTowardsPlayer", true);
        }


    }

    public float CalcDistanceToPlayer(EnemyAIStateMachine enemy)
    {
        float dist = Vector3.Distance(enemy.thisEnemy.transform.position, enemy.thisEnemy.playerTransform.position);
        return dist;
    }

    public void MoveAwayFromPlayer(EnemyAIStateMachine enemy)
    {
        Vector3 targetDirection;
        Vector3 targetPosition;
        movementIndex = 2;

        if (enemy.thisEnemy.playerTransform == null)
        {
            Debug.Log("No player found in scene");
            return;
        }
        enemy.thisEnemy.transform.LookAt(enemy.thisEnemy.playerTransform.position);
        enemy.thisEnemy.agent.updateRotation = false; //stop navmesh agent from rotating based on location direction.
        targetDirection = enemy.thisEnemy.transform.position - enemy.thisEnemy.playerTransform.position;
        targetPosition = enemy.thisEnemy.transform.position + targetDirection.normalized * enemy.thisEnemy.GetMoveAwayDistance();
        enemy.thisEnemy.agent.SetDestination(targetPosition);
    }

    //this function assumes the player is already detected
    public void MoveTowardsPlayerWithLimits(EnemyAIStateMachine enemy)
    {
        float dist = CalcDistanceToPlayer(enemy);

        if (dist <= enemy.thisEnemy.GetClosePlayerRadius())
        {
            StopMoving(enemy);
            enemy.SwitchState(enemy.inCombatState);
        }
        else
        {
            MoveTowardsPlayer(enemy);
        }
    }

    private void StopMoving(EnemyAIStateMachine enemy)
    {
        enemy.thisEnemy.agent.SetDestination(enemy.thisEnemy.playerTransform.position);
    }

    public void ForceBreakOutOfMoveState(EnemyAIStateMachine enemy)
    {
        enemy.thisEnemy.SetIsMoving(false);
        enemy.thisEnemy.animator.SetBool("isMoving", false);
        enemy.SwitchState(enemy.inCombatState);
    }
}