using System.Collections.Generic;
using UnityEngine;

public class AIDirector : MonoBehaviour
{

    /*  !!HEADS UP IMPORTANT NOTE!! 
        If you wish to edit the script to debug it, by removing [HideInInspector] from the public lists, do keep in mind that Unity has a null reference bug in regards to the inspector window
        
        I'm not sure why it happens, but I figure it's because the objects are being removed ~too quickly~ for unity's inspector to handle. We may need to make a custom debug script in order to keep up
        
        Overall, this current iteration of the script works pretty well. Enemies rarely bug out and when they do, it's hardly noticeable.
        !!HEADS UP IMPORTANT NOTE!!

        |--Purpose of this script--|
        The purpose of the AI director is to limit the amount of enemies that can attack the players all at once. I plan to add a "shuffle list" feature to change up which enemies attack the player.
        Furthermore, the AI Director can be disabled to allow the enemies to just be punching bags. Or, instead, to allow for aspects where we want the AI director to stop running.

        Although enemies depend on the Director for commands in regards to how many of them can attack, the logic for the attacks themselves are within the enemy's relevant scripts.
        So yes, you can ask an enemy to attack without using the director (if you go through the trouble of making a function), although using the director is simply easier.
        |--Purpose of this script--|
        
     */
    
    [SerializeField] private GameObject player;
    [HideInInspector] public List<GameObject> enemies;
    [HideInInspector] public List<GameObject> attackingEnemies;

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
        /*
        if (Input.GetKeyDown(KeyCode.K))
        {
            AIDirectorCycle();
        }
        */
    }

    //This function will essentially act as the "tick" function for our class.
    private void AIDirectorCycle() 
    {
        
        RemoveDeadEnemies();
        CheckIfEnemiesAreAttacking();
        Shuffle(enemies);
        foreach (GameObject enemy in enemies)
        {
            //store references to the gameObject script components
            EnemyAIAttackTimer thisTimer = enemy.GetComponent<EnemyAIAttackTimer>();
            EnemyBehaviourManager behaviours = enemy.GetComponent<EnemyBehaviourManager>();
            EnemyAI thisEnemy = enemy.GetComponent<EnemyAI>();

            if(enemy == null)
            {
                continue;
                
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
                        if(thisEnemy?.GetPreparedAttack() == true || thisEnemy?.GetIsAttacking() == true)
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
                            //Debug.Log("Attacks are not off cooldown. Executing movement coroutine instead");
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
                        //Debug.Log("Attacker count max reached, every other Unit is executing a movement function instead.");
                        //Ask enemy to go through their movement options instead, based on several factors (too many enemies near player? back up a little bit) (these factors are defined in a separate enemy script instead)
                        behaviours.StartMaintainDistanceWithPlayerForShortDuration();
                    }
                }
                
            }
            else
            {
                //Debug.Log("This enemy does not have an attack timer script attached | Enemy : " + enemy);
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
            //the enemy can become dead very quickly. hence why this check is here. this will be fixed later to implement a "isDead" boolean
            
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
                    //Debug.LogWarning("EnemyAI component not found on attacking enemy.")
                    attackingEnemies.RemoveAt(i);
                }
            }
        }
    }

    private void RemoveDeadEnemies()
    {
        //Iterate backwards to avoid issues with index shifting
        for (int i = enemies.Count - 1; i >= 0; i--)
        {
            if (enemies[i] == null)
            {
                enemies.RemoveAt(i);
            }
        }

        //Also remove dead enemies from the attackingEnemies list if applicable
        for (int i = attackingEnemies.Count - 1; i >= 0; i--)
        {
            if (attackingEnemies[i] == null)
            {
                attackingEnemies.RemoveAt(i);
            }
        }
    }

    //Fisher-yate shuffle algorithm wow epic. I didn't copy it from stack overflow I swear
    private void Shuffle(List<GameObject> list)
    {
        int n = list.Count;
        while (n > 1)
        {
            n--;
            int k = Random.Range(0, n + 1);
            GameObject temp = list[k];
            list[k] = list[n];
            list[n] = temp;
        }
    }

}
