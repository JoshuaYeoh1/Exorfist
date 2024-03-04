using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum RoomState { Inactive, Active, Clear, Start };

public class RoomStateManager : MonoBehaviour
{
    //The room state manager class is similar to the GameStateManager class, however this class handles the logic for how the room FLOWS, and whether or not the room is CLEARED.
    //Since this requires enemies to be considered a "room", the AI director is directly needed for this to function properly. Make sure to have an AI Director present in the scene alongside all the necessary spawner prefabs

    public RoomState State;
    public List<GameObject> EnemySpawns = new List<GameObject>();

    //Total enemies dictate the number of enemies in that specific room.
    //Remaining enemies is the number of enemies currently.
    [SerializeField] private int enemyWaves; //determines the amount of "waves" the enemies come in, if there are more than one wave. Default is 0 for no waves.
    [SerializeField] private Transform currentRoomTeleportTransform;

    void Awake()
    {
        State = RoomState.Inactive;
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.P)) UpdateRoomState(RoomState.Active);
    }

    void OnEnable()
    {
        if(GameEventSystem.current != null)
        {
            Debug.Log("RoomStateManager event subscriptions initialized");
            GameEventSystem.current.DeathEvent += OnEnemyDeath;
            GameEventSystem.current.NotifyRoomStateManagerEvent += notifyRoomStateManager;
            GameEventSystem.current.DoorTriggerEnterEvent += SetCurrentDoorTransform;
            GameEventSystem.current.RoomEnterEvent += OnRoomEntered;
            
        }
        else
        {
            Debug.LogError("GameEventSystem not found in scene. Please add one in for the RoomStateManager to function properly.");
        }
        
    }
    void OnDisable()
    {
        if (GameEventSystem.current != null)
        {
            Debug.Log("RoomStateManager event subscriptions initialized");
            GameEventSystem.current.DeathEvent -= OnEnemyDeath;
            GameEventSystem.current.NotifyRoomStateManagerEvent -= notifyRoomStateManager;
            GameEventSystem.current.DoorTriggerEnterEvent -= SetCurrentDoorTransform;
            GameEventSystem.current.RoomEnterEvent -= OnRoomEntered;
        }
    }

    public void UpdateRoomState(RoomState newState)
    {
        State = newState;
        
        switch (newState)
        {
            case RoomState.Inactive:
                //This is the initial state of the room
                //HandleInactive();
                Debug.Log("Inactive");
                break;
            case RoomState.Start:
                Debug.Log("Start");
                HandleStart();
                break;
            case RoomState.Active:
                Debug.Log("Active");
                //HandleActive();
                break;
            case RoomState.Clear:
                Debug.Log("Room cleared!");
                //HandleLose();
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(newState), newState, null);
        }

        GameEventSystem.current?.OnRoomStateChange(newState);
    }

    private IEnumerator HandleStart()
    {
        yield return null;
    }
    private void OnPlayerEnter()
    {
        if(State == RoomState.Inactive)
        {
            UpdateRoomState(RoomState.Active);
            return;
            //spawn enemies and shit
            //lock door to prevent player from leaving mid-combat
        }
    }
    private void OnEnemyDeath(GameObject victim, GameObject killer, float dmg, float kbForce, Vector3 contactPoint)
    {
        if(victim.tag!="Player") return;

        //null check for AI Director.
        if(AIDirector.instance == null)
        {
            Debug.LogError("There's no AI director in the scene asshole! PUT IT IN!");
            return;
        }

        switch (State)
        {
            case RoomState.Inactive:
                Debug.Log("Room state is currently inactive");
                break;

            case RoomState.Active:
                //Debug.Log("Enemy killed and will eventually transition into win state");
                Debug.Log(AIDirector.instance.enemies.Count.ToString());
                if(AIDirector.instance.enemies.Count == 1)
                {
                    if (AIDirector.instance.enemies[0].GetComponent<EnemyAI>() == null)
                    {
                        Debug.Log("Updating clear state, nicely done!");
                        UpdateRoomState(RoomState.Clear);
                        break;
                    }
                }
                if(AIDirector.instance.enemies.Count == 0)
                {
                    UpdateRoomState(RoomState.Clear);
                } 
                break;

            case RoomState.Clear:
                break;

            case RoomState.Start:
                break;

            default:
                Debug.LogError("RoomState is neither inactive nor active. We ain't giving a shit");
                throw new ArgumentOutOfRangeException(nameof(State), State, null);
        }
        if(State == RoomState.Active)
        {
            //Debug.Log("Enemy killed.");
        }
    }

    private void StaticRoomSetup()
    {
        foreach(GameObject enemySpawn in EnemySpawns)
        {
            if(enemySpawn != null)
            {
                enemySpawn.gameObject.SetActive(true);
            }
        }
    }

    private void notifyRoomStateManager()
    {
        if(State == RoomState.Active)
        {
            UpdateRoomState(RoomState.Clear);
        }
    }

    private void TeleportPlayerToRoom()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if(player != null)
        {
            player.transform.position = currentRoomTeleportTransform.position;
        }
        else
        {
            Debug.LogError("Player is null, fuck off");
        }
    }

    private void OnRoomEntered()
    {
        TeleportPlayerToRoom();
        UpdateRoomState(RoomState.Active);
    }

    private void SetCurrentDoorTransform(Transform transform)
    {
        currentRoomTeleportTransform = transform;
    }
}