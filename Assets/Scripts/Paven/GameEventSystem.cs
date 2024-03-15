using UnityEngine;
using UnityEngine.SceneManagement;
using System;

public class GameEventSystem : MonoBehaviour
{
    public static GameEventSystem Current;

    void Awake()
    {
        if(Current!=null && Current!=this)
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
        if(Current!=this) Destroy(gameObject);
    }

    //==Actor Related actions==//
    public event Action<GameObject> SpawnEvent;
    public event Action<GameObject, GameObject, HurtInfo> HitEvent; // ignores iframe
    public event Action<GameObject, GameObject, HurtInfo> HurtEvent; // respects iframe
    public event Action<GameObject, GameObject, HurtInfo> BlockEvent;
    public event Action<GameObject, GameObject, HurtInfo> ParryEvent;
    public event Action<GameObject, GameObject, HurtInfo> BlockBreakEvent;
    public event Action<GameObject, GameObject, HurtInfo> DeathEvent;

    public void OnSpawn(GameObject subject)
    {
        SpawnEvent?.Invoke(subject); //Debug.Log($"{subject.name} was spawned");
    }
    public void OnHit(GameObject attacker, GameObject victim, HurtInfo hurtInfo)
    {
        HitEvent?.Invoke(attacker, victim, hurtInfo); //Debug.Log($"{attacker.name} hit {victim.name} for {dmg}");
    }    
    public void OnHurt(GameObject victim, GameObject attacker, HurtInfo hurtInfo)
    {
        HurtEvent?.Invoke(victim, attacker, hurtInfo); //Debug.Log($"{victim.name} was hurt by {attacker.name} for {dmg}");
    }
    public void OnBlock(GameObject defender, GameObject attacker, HurtInfo hurtInfo)
    {
        BlockEvent?.Invoke(defender, attacker, hurtInfo);
    }
    public void OnParry(GameObject defender, GameObject attacker, HurtInfo hurtInfo)
    {
        ParryEvent?.Invoke(defender, attacker, hurtInfo);
    }
    public void OnBlockBreak(GameObject defender, GameObject attacker, HurtInfo hurtInfo)
    {
        BlockBreakEvent?.Invoke(defender, attacker, hurtInfo);
    }
    public void OnDeath(GameObject victim, GameObject killer, HurtInfo hurtInfo)
    {
        DeathEvent?.Invoke(victim, killer, hurtInfo); //Debug.Log($"{victim.name} was killed by {killer.name}");
    }

    //==Ability Related actions==//
    public event Action<bool> AbilitySlowMoEvent;
    public event Action<GameObject, string> AbilityCastEvent;
    public event Action<GameObject, string> AbilityCastingEvent;
    public event Action<GameObject, string> AbilityEndEvent;

    public void OnAbilitySlowMo(bool toggle)
    {
        AbilitySlowMoEvent?.Invoke(toggle);
    }
    public void OnAbilityCast(GameObject caster, string abilityName)
    {
        AbilityCastEvent?.Invoke(caster, abilityName);
    }
    public void OnAbilityCasting(GameObject caster, string abilityName)
    {
        AbilityCastingEvent?.Invoke(caster, abilityName);
    }
    public void OnAbilityEnd(GameObject caster, string abilityName)
    {
        AbilityEndEvent?.Invoke(caster, abilityName);
    }

    //==Small actions==//
    public event Action<GameObject, string, Transform> FootstepEvent;
    public event Action<GameObject, GameObject, bool> TargetEvent;

    public void OnFootstep(GameObject subject, string type, Transform footstepTr)
    {
        FootstepEvent?.Invoke(subject, type, footstepTr);
    }
    public void OnTarget(GameObject targeter, GameObject target, bool manual)
    {
        TargetEvent?.Invoke(targeter, target, manual);
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

    //==Sound Event==//
    public event Action<Transform, string> EnemySoundEvent;
    public event Action<Transform, string> PlayerSoundEvent;
    public event Action<Transform, string> EnvironmentSoundEvent;

    public void EnemySoundEventPlay(Transform transform, string searchKey)
    {
        EnemySoundEvent?.Invoke(transform, searchKey);
    }

    public void PlayerSoundEventPlay(Transform transform, string searchKey)
    {
        PlayerSoundEvent?.Invoke(transform, searchKey);
    }

    public void EnvironmentSoundEventPlay(Transform transform, string searchKey)
    {
        EnvironmentSoundEvent?.Invoke(transform, searchKey);
    }
    //==Sound Event==/


    /*
    private void onSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        checkGameState();
    }
    */
    //==Misc==//
}