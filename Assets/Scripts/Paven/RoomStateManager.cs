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

    private void Awake()
    {
        //CSInstance = this;
        //CSInstance.State = CombatState.DeployPhase;
    }

    public void UpdateRoomState(RoomState newState)
    {
        State = newState;

        switch (newState)
        {
            case RoomState.Inactive:
                //This is the initial state of the room
                //HandleInPlay();
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

    private void HandeLose()
    {

    }
    //Add functions for "HandePlay", "HandlePaused" etc, for example if the game is paused, how should the gameStateManager respond?
}

/*
 * Context:
 * Deploy phase = Deployment part, similar to Into The Breach
 * PlayerTurn, EnemyTurn, Victory, Lose = self explanatory.
 * Decide = Runs checks to see if the game should keep going (i.e if HeatGauge == max, immediately trigger lose state)
 */