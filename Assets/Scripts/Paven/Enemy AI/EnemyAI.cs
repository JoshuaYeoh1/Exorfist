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
    public float balanceMax;
    private float currentBalance;

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

    EnemyHurt hurt;

    void Awake()
    {
        //code to set things like event subscriptions, etc.
        animator = GetComponent<Animator>();
        body = GetComponent<Rigidbody>();
        playerTransform = GameObject.Find("Player").transform;
        agent = GetComponent<NavMeshAgent>();
        sm = GetComponent<EnemyAIStateMachine>();
        hurt = GetComponent<EnemyHurt>();

        GameEventSystem.current.OnSpawn(gameObject);
    }

    void Start()
    {
        if(AIDirector.instance)
        {
            AIDirector.instance.enemies.Add(gameObject); //add this EnemyAI gameObject to the AIDirector script
        }
    }

    void OnEnable()
    {
        GameEventSystem.current.HitEvent += OnHit;
        GameEventSystem.current.HurtEvent += OnHurt;
        GameEventSystem.current.BlockEvent += OnParried;
        GameEventSystem.current.DeathEvent += OnDeath;
    }
    void OnDisable()
    {
        GameEventSystem.current.HitEvent -= OnHit;
        GameEventSystem.current.HurtEvent -= OnHurt;
        GameEventSystem.current.BlockEvent -= OnParried;
        GameEventSystem.current.DeathEvent -= OnDeath;
    }

    void Update()
    {
        //Debug.Log(preparingAttack);
    }

    void OnHit(GameObject attacker, GameObject victim, float dmg, float kbForce, Vector3 contactPoint, float speedDebuffMult=.3f, float stunTime=.5f)
    {
        if(victim!=gameObject) return;

        //this is for a future event system implementation
        if(!isBlocking)
        {
            hurt.Hurt(attacker, dmg, kbForce, contactPoint);
            //EnemyHurt script already broadcasts to OnHurt event, no need to broadcast it again here
        }
        else
        {
            //passing half damage into the "balanceDamage" field for now
            //placeholder: receive half damage and 10% of stun time only
            LoseBalance(dmg*.5f, stunTime*.1f);
        }
    }

    public GameObject bloodVFXPrefab;

    void OnHurt(GameObject victim, GameObject attacker, float dmg, float kbForce, Vector3 contactPoint, float speedDebuffMult=.3f, float stunTime=.5f)
    {
        if(victim!=gameObject) return;

        if(speedDebuffMult<1 && stunTime>0)
        {
            Stun(speedDebuffMult, stunTime);
        }

        //move to vfx manager later
        GameObject blood = Instantiate(bloodVFXPrefab, contactPoint, Quaternion.identity);
        blood.hideFlags = HideFlags.HideInHierarchy;
    }

    void LoseBalance(float balanceDamage, float blockStun)
    {
        if(currentBalance>0)
        {
            currentBalance -= balanceDamage;

            if(currentBalance<=0)
            {
                //switch EnemyAIStateMachine to "BalanceBroken" state, stop all coroutines and play balancebroken animation (probably just a longer stun with a vfx), play sound effect, etc.
            }
        }
    }

    void OnParried(GameObject defender, GameObject attacker, Vector3 contactPoint, bool parry, bool broke)
    {
        if(attacker!=gameObject || !parry) return;

        Stun();
    }

    void Stun(float speedDebuffMult=.3f, float stunTime=.5f)
    {
        //switch EnemyAIStateMachine to "HitStun" state, stop all coroutines and play hurt animation, play sound effect, etc.
        //we can use an event system to call sfx and hurt animations if we need to :P
        isHitStun = true;
        sm.HitStunSwitchState(sm.hitStunState);
    }

    public GameObject ragdollPrefab;

    void OnDeath(GameObject victim, GameObject killer, float dmg, float kbForce, Vector3 contactPoint)
    {
        if(victim!=gameObject) return;

        //switch EnemyAIStateMachine to "dying" state, stop all coroutines as needed (if we're using coroutines that is)
        
        //EnemyHurt script already broadcasts to OnDeath event, no need to broadcast it again here
        
        //Debug.Log($"{victim.name}: wtf {killer.name} is hacking!11!1!");

        Ragdoller ragdoll = Instantiate(ragdollPrefab, transform.position, transform.rotation).GetComponent<Ragdoller>();

        ragdoll.PushRagdoll(kbForce, contactPoint);

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
