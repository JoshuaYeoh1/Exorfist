using System.Collections;
using UnityEngine;
using UnityEngine.AI;

//This holds things like the stats and booleans of our EnemyAI. As well as the location vector of the player.
//Also holds attack animation data and hurt animation data.
public class EnemyAI : MonoBehaviour
{
    //Declarations for respective things
    
    [HideInInspector] public Animator animator;
    [HideInInspector] public Rigidbody body;
    [HideInInspector] public NavMeshAgent agent;
    [HideInInspector] public EnemyAIStateMachine sm;
    [HideInInspector] public EnemyAIAttackTimer atkTimer;

    public LayerMask whatIsGround, whatIsPlayer;

    [Header("Stats")]
    public float healthMax;
    private float currentHealth;

    public float balanceMax;
    private float currentBalance;

    bool iframe;
    public float iframeTime=.1f;

    public float chiDrop;

    public float hitStunDuration;
    public Transform playerTransform;

    [Header("States")] //Serializing these fields so that we can inspect them when debugging.
    [SerializeField] private bool preparedAttack; //startup frames of an attack, or when the enemy is moving towards the player to attack.
    [SerializeField] private bool isAttacking;
    [SerializeField] private bool isMoving;
    [SerializeField] private bool isStunned;
    [SerializeField] private bool isHitStun;
    [SerializeField] private bool isInCombat;
    [SerializeField] private bool isIdle;
    [SerializeField] private bool isBlocking;
    [SerializeField] private bool isDead;

    [Header("Behaviours")]
    public bool canBlock;
    public bool circlesPlayer;

    [Header("Navigation Numbers")]
    //==Navigation Numbers==//
    [SerializeField]
    [Range(0, 5)]
    private float circleRadius; //determines radius of CircularMovement

     //radius for enemy to be considered "near" the player. For melee attack purposes, can be adjusted.
    [SerializeField]
    [Range(0, 5)]
    private float closePlayerRadius;

     //Radius for enemy to be considered "far" from the player. To initiate the "moveCloserTo" function. Or to use for ranged enemies later down the line.
    [SerializeField]
    [Range(0,10)]
    private float farPlayerRadius;

    
    [SerializeField]
    [Range(0, 10)]
    private float moveAwayDistance; //float value to determine how far away the enemy should move.
    //==Navigation Numbers==//

    [Header("Movement Speeds")]
    [SerializeField] private float defaultMovementSpeed;
    [SerializeField] private float circlingMovementSpeed;

    OffsetMeshColor offsetColor;
    HPManager hp;

    private void Awake()
    {
        //code to set things like event subscriptions, etc.
        currentHealth = healthMax;
        currentBalance = balanceMax;

        animator = GetComponent<Animator>();
        body = GetComponent<Rigidbody>();
        playerTransform = GameObject.Find("Player").transform;
        agent = GetComponent<NavMeshAgent>();
        sm = GetComponent<EnemyAIStateMachine>();
        offsetColor = GetComponent<OffsetMeshColor>();
        hp = GetComponent<HPManager>();
    }

    private void Start()
    {
        if (AIDirector.instance != null)
        {
            AIDirector.instance.enemies.Add(gameObject); //add this EnemyAI gameObject to the AIDirector script
        }
        
    }

    // private void OnDestroy()
    // {
    //     OnEnemyDeath(gameObject);
    // }

    private void Update()
    {
        //Debug.Log(preparingAttack);
        UpdateHPManager();
    }

    //Taking damage algorithm
    private void OnTriggerEnter(Collider other)
    {
        if (other != null)
        {
            if (other.gameObject.GetComponent<PlayerHitbox>() != null)
            {
                PlayerHitbox thisWep = other.gameObject.GetComponent<PlayerHitbox>();
                //pseudocode notes for balance mechanic and blocking

                //if(isBlocking == true) { blockAttack(), reduceBalance() }

                //use thisWep.hitStun duration etc, as values to be passed into the "takingDamage" function" | Should be added later
                isHitStun = true;
                LoseHealth(thisWep.damage, thisWep.damage, thisWep.knockback, thisWep.contactPoint);
            }
            else
            {
                return;
            }
        }
    }

    //this is for a future event system implementation
    private void OnHitByPlayer(PlayerHitbox thisWep)
    {
        if(isBlocking == true)
        {
            //passing thisWep.damage into the "balanceDamage" field for now
            LoseBalance(thisWep.damage, thisWep.damage);
        }
        else
        {
            
        }
    }

    private void LoseHealth(float healthdamage, float balancedamage, float kbForce, Vector3 contactPoint, float speedDebuffMult=.3f, float stunTime=.2f)
    {
        if(currentHealth > 0)
        {
            if(!iframe)
            {
                DoIFraming(iframeTime);

                Knockback(body, kbForce, contactPoint);

                Singleton.instance.SpawnPopUpText(contactPoint, healthdamage.ToString(), Color.white);
                
                currentHealth -= healthdamage;

                if(currentHealth <= 0)
                {
                    OnEnemyDeath();
                    return;
                }
                else
                {
                    //switch EnemyAIStateMachine to "HitStun" state, stop all coroutines and play hurt animation, play sound effect, etc.
                    //we can use an event system to call sfx and hurt animations if we need to :P
                    isHitStun = true;
                    sm.HitStunSwitchState(sm.hitStunState);
                    
                    return;
                }
            }
        }
        else
        {
            //switch EnemyAIStateMachine to "dying" state, stop all coroutines as needed (if we're using coroutines that is)
        }
    }

    void UpdateHPManager()
    {
        hp.hpMax=healthMax;
        hp.hp=currentHealth;
    }

    public void DoIFraming(float t)
    {
        StartCoroutine(IFraming(t));
    }
    IEnumerator IFraming(float t)
    {
        iframe=true;

        StartIFrameFlicker();

        yield return new WaitForSeconds(t);

        iframe=false;

        StopIFrameFlicker();
    }

    void StartIFrameFlicker()
    {
        if(iFrameFlickeringRt!=null) StopCoroutine(iFrameFlickeringRt);
        iFrameFlickeringRt = StartCoroutine(IFrameFlickering());
    }
    void StopIFrameFlicker()
    {
        if(iFrameFlickeringRt!=null) StopCoroutine(iFrameFlickeringRt);
        offsetColor.OffsetColor();
    }

    Coroutine iFrameFlickeringRt;
    IEnumerator IFrameFlickering()
    {
        while(true)
        {
            offsetColor.OffsetColor(.5f, -.5f, -.5f);
            yield return new WaitForSecondsRealtime(.05f);
            offsetColor.OffsetColor();
            yield return new WaitForSecondsRealtime(.05f);
        }
    }

    public void Knockback(Rigidbody rb, float force, Vector3 contactPoint)
    {
        if(force>0)
        {
            Vector3 kbVector = rb.transform.position - contactPoint;
            kbVector.y = 0;

            rb.velocity = new Vector3(0, rb.velocity.y, 0);
            rb.AddForce(kbVector.normalized * force, ForceMode.Impulse);
        }
    }

    private void LoseBalance(float balanceDamage, float blockStun)
    {
        if(currentBalance > 0)
        {
            currentBalance = currentBalance - balanceDamage;
            if(currentBalance <=0)
            {
                //switch EnemyAIStateMachine to "BalanceBroken" state, stop all coroutines and play balancebroken animation (probably just a longer stun with a vfx), play sound effect, etc.
                return;
            }
        }
    }

    public GameObject ragdollPrefab;

    void OnEnemyDeath()
    {
        GameEventSystem.current.enemyDeath(gameObject);

        //switch EnemyAIStateMachine to "dying" state, stop all coroutines
        
        Instantiate(ragdollPrefab, transform.position, transform.rotation);

        Destroy(gameObject);
    }
    
    //IsMoving getters/setters
    public void SetIsMoving(bool isMoving)
    {
        this.isMoving = isMoving;
    }

    public bool GetIsMoving()
    {
        return isMoving;
    }

    //IsIdle getters/setters
    public void SetIsIdle(bool isIdle)
    {
        this.isIdle = isIdle;
    }

    public bool GetIsIdle()
    {
        return isIdle;
    }


    //CircleRadius
    public float GetCircleRadius() { return circleRadius; }

    public void SetClosePlayerRadius(float radius) { closePlayerRadius = radius; }
    public float GetClosePlayerRadius() { return closePlayerRadius; }

    public float GetFarPlayerRadius() { return farPlayerRadius; }
    public float GetMoveAwayDistance() { return moveAwayDistance; }

    //Movement speed related
    public float GetCircleSpeed()
    {
        return circlingMovementSpeed;
    }
    public float GetDefaultSpeed()
    {
        return defaultMovementSpeed;
    }
    public void SetNavMeshSpeed(float speed)
    {
        agent.speed = speed;
    }

    //hitstun bullshit
    public bool GetIsHitStun() { return isHitStun; }
    public void SetIsHitStun(bool input) { isHitStun = input; }
    public void SetHitStunToFalse() { isHitStun = false; }

    //prepared atack bullshit
    public void SetPreparedAttack(bool input) { preparedAttack = input; }
    public void SetPreparedAttackToFalse() { preparedAttack = false; }
    public bool GetPreparedAttack() { return preparedAttack; }

    //isAttacking bullshit
    public void SetIsAttacking(bool input) { isAttacking = input;}
    public bool GetIsAttacking() { return isAttacking; }

    //isDead bullshit
    public bool GetIsDead() { return isDead; }

    private void DisableComponentsOnDeath()
    {
        Destroy(sm);
        Destroy(agent);
        Destroy(atkTimer);
    }
}
