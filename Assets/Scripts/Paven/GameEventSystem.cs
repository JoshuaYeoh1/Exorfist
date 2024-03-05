using UnityEngine;
using UnityEngine.SceneManagement;
using System;

public class GameEventSystem : MonoBehaviour
{
    public static GameEventSystem Current;

    void Awake()
    {
        if(Current != null && Current != this)
        {
            Destroy(gameObject);
            return;
        }

        Current = this;
        DontDestroyOnLoad(gameObject); // Persist across scene changes
    }

    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }
    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if(Current != this)
        {
            Destroy(gameObject);
            return;
        }
    }

    //==Actor Related actions==//
    public event Action<GameObject> SpawnEvent;
    public event Action<GameObject, GameObject, float, float, Vector3, float, float> HitEvent;
    public event Action<GameObject, GameObject, float, float, Vector3, float, float> HurtEvent;
    public event Action<GameObject, GameObject, float, float, Vector3> DeathEvent;
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
    public void OnDeath(GameObject victim, GameObject killer, float dmg, float kbForce, Vector3 contactPoint)
    {
        DeathEvent?.Invoke(victim, killer, dmg, kbForce, contactPoint); //Debug.Log($"{victim.name} was killed by {killer.name}");
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
    }

    /*
    private void onSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        checkGameState();
    }
    */
    //==Misc==//
}