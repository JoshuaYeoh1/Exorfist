using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomStateManager : MonoBehaviour
{
    //GameState manager
    //public static GameStateManager GSManagerInstance;

    public enum RoomState {Inactive, Active, Clear, Start };

    public RoomState State;
    public List<GameObject> EnemySpawns = new List<GameObject>();

    //Total enemies dictate the number of enemies in that specific room.
    //Remaining enemies is the number of enemies currently.
    [SerializeField] private int remainingEnemies;

    private void Awake()
    {
        //CSInstance = this;
        //CSInstance.State = CombatState.DeployPhase;
    }

    private void Start()
    {
        State = RoomState.Inactive;
        GameEventSystem.current.OnEnemyDeath += ReduceEnemyCount;
    }
    public void UpdateRoomState(RoomState newState)
    {
        State = newState;

        switch (newState)
        {
            case RoomState.Inactive:
                //This is the initial state of the room
                //HandleInactive();
                break;
            case RoomState.Start:
                //This is where the function for all the enemies spawning in takes place. As well as setting up the kill count for the enemies in the room.
                //HandleStart();
                break;
            case RoomState.Active:
                //This is where the function for calculating whether or not the room is still considered as "clear" or not.
                //HandleActive();
                break;
            case RoomState.Clear:
                //HandleLose();
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(newState), newState, null);
        }

        GameEventSystem.current?.roomStateChange(newState);
    }

    private void HandleInactive()
    {
        return; //do nothing
    }

    private void OnPlayerEnter()
    {
        if(State == RoomState.Inactive)
        {
            State = RoomState.Active;
            //spawn enemies and shit
            //lock door to prevent player from leaving mid-combat
        }
    }
    private void OnEnemyDeath()
    {
        
    }

    private void ReduceEnemyCount()
    {
        remainingEnemies--;

        if(remainingEnemies <= 0)
        {
            State = RoomState.Clear; //set RoomState to "Clear"
        }
    }
}

/*
 * Context:
 * Deploy phase = Deployment part, similar to Into The Breach
 * PlayerTurn, EnemyTurn, Victory, Lose = self explanatory.
 * Decide = Runs checks to see if the game should keep going (i.e if HeatGauge == max, immediately trigger lose state)
 */