using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
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
            TempEnemyBehaviours behaviours = enemy.GetComponent<TempEnemyBehaviours>();

            if(enemy != null)
            {
                if (enemy.GetComponent<EnemyAIAttackTimer>() != null)
                {

                    if (thisTimer.GetAtkCooldownBool() == false)
                    {

                        if (behaviours != null)
                        {
                            behaviours.StartMoveTowardsThenAttack();
                            thisTimer.StartAtkCooldown();
                        }
                    }
                    else
                    {
                        Debug.Log("Attacks are not off cooldown");
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
