using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class AIDirector : MonoBehaviour
{
    [SerializeField] private GameObject player;
    public List<GameObject> enemies;
    public List<GameObject> attackingEnemies;

    private List<GameObject> enemiesToRemove;
    private List<GameObject> attackingEnemiesToRemove;

    public static AIDirector instance;

    //Maximum amount of attackers that can be present during a list iteration.
    [SerializeField] private int attackerCountMax;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        else if(instance != null)
        {
            instance = this;
        }
    }


    void Start()
    {
        //this assumes there is only one gameObject with the tag of Player
        player = GameObject.FindGameObjectWithTag("Player");

        //this method apparently repeats a function based on a time parameter that gets passed into the third parameter slot.
        //the second parameter slot is the INITIAL delay, AKA for the first call of the function. Neat stuff
        InvokeRepeating("AIDirectorCycle", 1f, 0.5f);
    }

    
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.K))
        {
            AIDirectorCycle();
        }

        CheckIfEnemiesAreAttacking();
    }

    //This function will essentially act as the "tick" function for our class.
    private void AIDirectorCycle() 
    {
        //CheckIfEnemiesAreAttacking();
        foreach (GameObject enemy in enemies)
        {
            //store references to the gameObject script components
            EnemyAIAttackTimer thisTimer = enemy.GetComponent<EnemyAIAttackTimer>();
            EnemyBehaviourManager behaviours = enemy.GetComponent<EnemyBehaviourManager>();
            EnemyAI thisEnemy = enemy.GetComponent<EnemyAI>();

            if(enemy == null)
            {
                enemiesToRemove.Add(enemy);
            }
            if(enemy != null)
            {
                if (attackingEnemies.Count <= attackerCountMax)
                {
                    if (thisEnemy?.GetIsHitStun() == true && behaviours?.currentCoroutine == null)
                    {
                        continue; //continue to next item in the Foreach loop
                    }
                    if (enemy.GetComponent<EnemyAIAttackTimer>() != null)
                    {
                        if(thisEnemy?.GetPreparedAttack() == true)
                        {
                            continue; //skip element as it's already preparing an attack.
                        }
                        if (thisTimer.GetAtkCooldownBool() == false)
                        {

                            if (behaviours != null)
                            {
                                behaviours.StartMoveTowardsThenAttack(); //this part of my code needs to be modular, I'll find a solution later -Paven
                                attackingEnemies.Add(enemy);
                            }
                        }
                        else
                        {
                            Debug.Log("Attacks are not off cooldown. Executing movement coroutine instead");
                            //Ask enemy to go through their movement options instead, based on several factors (too many enemies near player? back up a little bit) (these factors are defined in a separate enemy script instead)
                            behaviours.StartMaintainDistanceWithPlayerForShortDuration();
                        }
                    }
                }
                else
                {
                    if (thisEnemy?.GetIsHitStun() == true && behaviours?.currentCoroutine == null)
                    {
                        continue; //continue to next item in the Foreach loop
                    }
                    else
                    {
                        Debug.Log("Attacker count max reached, every other Unit is executing a movement function instead.");
                        //Ask enemy to go through their movement options instead, based on several factors (too many enemies near player? back up a little bit) (these factors are defined in a separate enemy script instead)
                        behaviours.StartMaintainDistanceWithPlayerForShortDuration();
                    }
                }
                
            }
            else
            {
                Debug.Log("This enemy does not have an attack timer script attached | Enemy : " + enemy);
            }
        }
        //reset attackerCount so that the next list iteration can take it into consideration
    }

    private void CheckIfEnemiesAreAttacking()
    {
        if (attackingEnemies.Count == 0)
        {
            return; //exit method early if there are no attacking enemies;
        }
        for (int i = attackingEnemies.Count - 1; i >= 0; i--)
        {
            GameObject enemy = attackingEnemies[i];
            //the enemy can become dead very quickly. hence why this check is here.
            if(enemy == null)
            {
                attackingEnemiesToRemove.Add(enemy);
            }
            if (enemy != null)
            {
                EnemyAI currentEnemy = enemy.GetComponent<EnemyAI>();

                if (currentEnemy != null)
                {
                    if (currentEnemy.GetPreparedAttack() != true)
                    {
                        attackingEnemies.RemoveAt(i);
                    }
                }
                else
                {
                    // Handle case where the enemy component is null
                    Debug.LogWarning("EnemyAI component not found on attacking enemy.");
                    // Optionally, remove the enemy from the list if it's null
                    attackingEnemies.RemoveAt(i);
                }
            }
        }
    }
}
