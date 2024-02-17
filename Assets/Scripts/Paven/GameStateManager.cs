using System;
using System.Collections;
using UnityEngine;

public class GameStateManager : MonoBehaviour
{
    //GameState manager
    //public static GameStateManager GSManagerInstance;

    public GameState State;
    public bool enemyTurnOver;
    public bool createSpawnTiles;

    //This is when the GameState changes
    //public static event Action<GameState> OnGameStateChanged;

    private void Awake()
    {
        //CSInstance = this;
        //CSInstance.State = CombatState.DeployPhase;
    }

    public void UpdateCombatState(GameState newState)
    {
        State = newState;

        switch (newState)
        {
            case GameState.InPlay:
                //HandleInPlay();
                break;
            case GameState.Paused:
                //HandlePaused();
                break;
            case GameState.Victory:
                //HandleVictory();
                break;
            case GameState.Lose:
                //HandleLose();
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(newState), newState, null);
        }

        GameEventSystem.current?.gameStateChange(newState);
    }

    //Add functions for "HandePlay", "HandlePaused" etc, for example if the game is paused, how should the gameStateManager respond?
}

/*
 * Context:
 * Deploy phase = Deployment part, similar to Into The Breach
 * PlayerTurn, EnemyTurn, Victory, Lose = self explanatory.
 * Decide = Runs checks to see if the game should keep going (i.e if HeatGauge == max, immediately trigger lose state)
 */

public enum GameState
{
    InPlay,
    Paused,
    Victory,
    Lose
}