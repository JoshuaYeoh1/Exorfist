using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Assertions.Must;

public enum RoomState { Inactive, Active, Clear, Start };

public class RoomStateManager : MonoBehaviour
{
    //The room state manager class is similar to the GameStateManager class, however this class handles the logic for how the room FLOWS, and whether or not the room is CLEARED.
    //Since this requires enemies to be considered a "room", the AI director is directly needed for this to function properly. Make sure to have an AI Director present in the scene alongside all the necessary spawner prefabs

    //Room ID to differentiate between different rooms.
    [SerializeField] private int RoomID;

    [Header("Logic Properties")]
    [SerializeField] private bool ActiveOnAwake;
    [SerializeField] private bool DeleteSpawnersOnClear;
    [SerializeField] private bool SetBarriersToActiveOnActive;
    [SerializeField] private bool DeleteBarriersOnClear;
    [SerializeField] private bool TeleportOnRoomStart;
    public int GetRoomID()
    {
        return RoomID;
    }

    public void SetRoomID(int num)
    {
        RoomID = num;
    }

    public RoomState State;
    public List<GameObject> EnemySpawns = new List<GameObject>();

    [Header("Barriers")]
    public List<GameObject> RoomBarriers = new List<GameObject>();
    public List<GameObject> PermanentBarriers = new List<GameObject>();

    //Total enemies dictate the number of enemies in that specific room.

    //Remaining enemies is the number of enemies currently.
    //[SerializeField] private int enemyWaves; //determines the amount of "waves" the enemies come in, if there are more than one wave. Default is 0 for no waves.
    [SerializeField] private Transform currentRoomTeleportTransform;
    void Awake()
    {
        if(ActiveOnAwake == false)
        {
            State = RoomState.Inactive;
        }
        else
        {
            UpdateRoomState(RoomState.Active);
        }
    }

    void Update()
    {
        //if(Input.GetKeyDown(KeyCode.P)) UpdateRoomState(RoomState.Active);
    }

    void OnEnable()
    {
        Debug.Log("RoomStateManager event subscriptions initialized");
        StaticRoomSetup();
        GameEventSystem.Current.DeathEvent += OnEnemyDeath;
        GameEventSystem.Current.NotifyRoomStateManagerEvent += notifyRoomStateManager;
        GameEventSystem.Current.DoorTriggerEnterEvent += SetCurrentDoorTransform;
        GameEventSystem.Current.RoomEnterEvent += OnRoomEntered;
    }
    void OnDisable()
    {
        GameEventSystem.Current.DeathEvent -= OnEnemyDeath;
        GameEventSystem.Current.NotifyRoomStateManagerEvent -= notifyRoomStateManager;
        GameEventSystem.Current.DoorTriggerEnterEvent -= SetCurrentDoorTransform;
        GameEventSystem.Current.RoomEnterEvent -= OnRoomEntered;
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
                //HandleStart();
                break;
            case RoomState.Active:
                Debug.Log("Active");
                HandleActive();
                break;
            case RoomState.Clear:
                Debug.Log("Room cleared!");
                HandleClear();
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(newState), newState, null);
        }

        GameEventSystem.Current?.OnRoomStateChange(newState);
    }

    private void HandleActive()
    {
        Debug.Log("Handling active");
        if(PermanentBarriers.Count > 0)
        {
            foreach (GameObject barrier in PermanentBarriers)
            {
                if (barrier != null)
                {
                    if (barrier.activeSelf == false)
                    {
                        barrier.SetActive(true);
                    }
                }
            }
        }
        
        if(RoomBarriers.Count > 0)
        {
            foreach (GameObject barrier in RoomBarriers)
            {
                if (barrier != null)
                {
                    if (barrier.activeSelf == false)
                    {
                        barrier.SetActive(true);
                    }
                }
            }
        }
    }

    private void HandleClear()
    {
        //Debug.Log("Unsubbing from events");
        // Clear the list by iterating through and removing elements
        GameObject go;
        for (int i = 0; i < EnemySpawns.Count; i++)
        {
            go = EnemySpawns[i];
            if(go != null)
            {
                Destroy(go);
            }
            else
            {
                Debug.Log("GameObject is null, exiting loop early");
                break;
            }
        }

        for (int i = 0; i < RoomBarriers.Count; i++)
        {
            go = RoomBarriers[i];
            if (go != null)
            {
                Destroy(go);
            }
            else
            {
                Debug.Log("GameObject is null, exiting loop early");
                break;
            }
        }
        gameObject.SetActive(false);
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
    private void OnEnemyDeath(GameObject victim, GameObject killer, HurtInfo hurtInfo)
    {
        if(victim.tag=="Player") return;

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
            if(TeleportOnRoomStart == true)
            {
                if (currentRoomTeleportTransform == null)
                {
                    Debug.Log("RSM does not have a transport point set.");
                    return;
                }
                else
                {
                    player.transform.position = currentRoomTeleportTransform.position;
                }
            }
            else
            {
                //Debug.Log("Room starting, good luck!");
            }
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