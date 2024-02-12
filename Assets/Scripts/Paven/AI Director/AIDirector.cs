using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class AIDirector : MonoBehaviour
{
    [SerializeField] private GameObject player;
    public List<GameObject> enemies;

    public static AIDirector instance;

    private int attackerCount;
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
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.K))
        {
            AICycle();
        }
    }

    //IT WORKS!!!! :))
    private void AICycle() 
    {
        foreach(GameObject enemy in enemies)
        {
            //store references to the gameObject script components
            EnemyAIAttackTimer thisTimer = enemy.GetComponent<EnemyAIAttackTimer>();
            EnemyBehaviourManager behaviours = enemy.GetComponent<EnemyBehaviourManager>();
            EnemyAI thisEnemy = enemy.GetComponent<EnemyAI>();

            
            if(enemy != null)
            {
                if (thisEnemy?.GetIsHitStun() == true && behaviours?.currentCoroutine == null)
                {
                    continue; //continue to next item in the Foreach loop
                }
                if (enemy.GetComponent<EnemyAIAttackTimer>() != null)
                {

                    if (thisTimer.GetAtkCooldownBool() == false)
                    {

                        if (behaviours != null)
                        {
                            behaviours.StartMoveTowardsThenAttack();
                            attackerCount++;
                            thisTimer.StartAtkCooldown();
                        }
                    }
                    else
                    {
                        Debug.Log("Attacks are not off cooldown. Executing movement coroutine instead");
                        //Ask enemy to go through their movement options instead, based on several factors (too many enemies near player? back up a little bit) (these factors are defined in a separate enemy script instead)
                        behaviours.StartCirclePlayerForDuration();
                    }
                }
            }
            else
            {
                Debug.Log("This enemy does not have an attack timer script attached | Enemy : " + enemy);
            }
        }
    }
}
