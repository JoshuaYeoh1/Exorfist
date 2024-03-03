using UnityEngine;
using System;
<<<<<<< HEAD
using UnityEngine.SceneManagement;

public class GameEventSystem : MonoBehaviour
{

=======

public class GameEventSystem : MonoBehaviour
{
>>>>>>> main
    //Static reference of the current game event system so that it can be accessed from anywhere in the game / project file.
    public static GameEventSystem current;

    void Awake()
    {
<<<<<<< HEAD
        if (GameEventSystem.current == null)
        {
            GameEventSystem.current = this;
            Debug.Log("GameEventSystem defined");
            return;
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    //==Player Related Actions==//
    public event Action OnPlayerHit;
    public event Action OnPlayerHurt;
    public event Action OnPlayerBlock;
    public event Action OnPlayerParry;
    public event Action OnPlayerDeath;
    //==Player Related Actions==//

    //==Enemy Related Actions==//
    public event Action<GameObject> OnEnemyDeath;
    public event Action OnEnemySpawn;
    //==Enemy Related Actions==//

    //==Objective Related actions==//
    public event Action OnScoreReached;
    public event Action OnLevelFinish;
    public event Action OnLevelStart;
    //==Objective Related actions==//

    //==Transition and Room related==//
    //"Rooms" meaning things like, the rooms filled with enemies, btw.
    public event Action OnRoomEntered;
    public event Action<RoomState> OnRoomStateChanged;
    public event Action NotifyRoomStateManager;
    public event Action<Transform> OnDoorTriggerEnter;
    //==Transition and Room related==//

    //==GameStateManager Related==//
    public event Action<GameState> OnGameStateChanged;
    //==GameStateManager Related==//

    public void spawnEnemies()
    {
        Debug.Log("SpawnEnemies triggered");
        OnEnemySpawn?.Invoke();
    }
    public void enemyDeath(GameObject victim)
    {
        //Debug.Log("Enemy died");
        OnEnemyDeath?.Invoke(victim); //tysm Jon
    }
    //==Enemy Related==//



    //==Player Related==//
    public void playerHit()
    {
        Debug.Log("Player hit something");
        OnPlayerHit?.Invoke();
    }
    public void playerHurt()
    {
        Debug.Log("Player got hurt :(");
        OnPlayerHurt?.Invoke();
    }
    public void playerBlock()
    {
        //Debug.Log("PlayerTurn started");
        OnPlayerBlock?.Invoke();
    }
    public void playerParry()
    {
        Debug.Log("onplayer parry func");
        OnPlayerParry?.Invoke();
    }
    public void playerDeath()
    {
        Debug.Log("playerDeath()");
        OnPlayerDeath?.Invoke();
    }
    //==Player Related==//



    //==Objective Related==//
    public void scoreReached()
    {
        Debug.Log("Score reached or whatever");
        OnScoreReached?.Invoke();
    }
    public void levelStart()
    {
        Debug.Log("Level started");
        OnLevelStart?.Invoke();
    }
    public void levelFinish()
    {
        Debug.Log("Level finished");
        OnLevelFinish?.Invoke();
    }
    //==Objective Related==//

    //==Misc==//
    public void gameStateChange(GameState newState)
    {
        OnGameStateChanged?.Invoke(newState);
    }
    public void roomStateChange(RoomState roomState)
    {
        OnRoomStateChanged?.Invoke(roomState);
    }
    public void notifyRoomStateManager()
    {
        NotifyRoomStateManager?.Invoke();
    }
    public void roomEntered()
    {
        OnRoomEntered?.Invoke();
    }
    public void doorTriggerEnter(Transform transform)
    {
        OnDoorTriggerEnter?.Invoke(transform);
=======
        if(!current) GameEventSystem.current = this;
        else Destroy(gameObject);
    }

    public event Action<GameObject> SpawnEvent;
    public event Action<GameObject, GameObject, float, float, Vector3, float, float> HitEvent;
    public event Action<GameObject, GameObject, float, float, Vector3, float, float> HurtEvent;
    public event Action<GameObject, GameObject> DeathEvent;
    public event Action<GameObject, GameObject, Vector3, bool, bool> BlockEvent;

    public void OnSpawn(GameObject subject)
    {
        SpawnEvent?.Invoke(subject); //Debug.Log($"{subject.name} was spawned");
    }
    public void OnHit(GameObject attacker, GameObject victim, float dmg, float kbForce, Vector3 contactPoint, float speedDebuffMult, float stunTime)
    {
        HitEvent?.Invoke(attacker, victim, dmg, kbForce, contactPoint, speedDebuffMult, stunTime); //Debug.Log($"{attacker.name} hit {victim.name} for {dmg}");
    }    
    public void OnHurt(GameObject victim, GameObject attacker, float dmg, float kbForce, Vector3 contactPoint, float speedDebuffMult, float stunTime)
    {
        HurtEvent?.Invoke(victim, attacker, dmg, kbForce, contactPoint, speedDebuffMult, stunTime); //Debug.Log($"{victim.name} was hurt by {attacker.name} for {dmg}");
    }
    public void OnDeath(GameObject victim, GameObject killer)
    {
        DeathEvent?.Invoke(victim, killer); //Debug.Log($"{victim.name} was killed by {killer.name}");
    }
    public void OnBlock(GameObject defender, GameObject attacker, Vector3 contactPoint, bool parry, bool broke)
    {
        BlockEvent?.Invoke(defender, attacker, contactPoint, parry, broke); //Debug.Log($"{defender.name} blocked {attacker.name}");
    }

    //==Objective Related actions==//
    public event Action ScoreReachedEvent;
    public event Action LevelStartEvent;
    public event Action LevelFinishEvent;

    public void OnScoreReached()
    {
        ScoreReachedEvent?.Invoke(); //Debug.Log("Score reached or whatever");
    }
    public void OnLevelStart()
    {
        LevelStartEvent?.Invoke(); //Debug.Log("Level started");
    }
    public void OnLevelFinish()
    {
        LevelFinishEvent?.Invoke(); //Debug.Log("Level finished");
    }

    //==Transition and Room related==//
    //"Rooms" meaning things like, the rooms filled with enemies, btw.
    public event Action<RoomState> RoomStateChangedEvent;
    public event Action NotifyRoomStateManagerEvent;
    public event Action RoomEnterEvent;
    public event Action<Transform> DoorTriggerEnterEvent;

    public void OnRoomStateChange(RoomState roomState)
    {
        RoomStateChangedEvent?.Invoke(roomState);
    }
    public void OnNotifyRoomStateManager()
    {
        NotifyRoomStateManagerEvent?.Invoke();
    }
    public void OnRoomEnter()
    {
        RoomEnterEvent?.Invoke();
    }
    public void OnDoorTriggerEnter(Transform transform)
    {
        DoorTriggerEnterEvent?.Invoke(transform);
    }

    //==GameStateManager Related==//
    public event Action<GameState> GameStateChangedEvent;

    public void OnGameStateChange(GameState newState)
    {
        GameStateChangedEvent?.Invoke(newState);
>>>>>>> main
    }

    /*
    private void onSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        checkGameState();
    }
    */
    //==Misc==//
}