using System;
using System.Collections;
using UnityEngine;

public class GameStateManager : MonoBehaviour
{
    //GameState manager
    //public static GameStateManager GSManagerInstance;

    public GameState State;

    //This is when the GameState changes
    //public static event Action<GameState> OnGameStateChanged;

    [SerializeField] private GameObject gameOverPopUp;

    void OnEnable()
    {
        GameEventSystem.Current.DeathEvent += OnPlayerDeath;
    }
    void OnDisable()
    {
        GameEventSystem.Current.DeathEvent -= OnPlayerDeath;
    }

    public void UpdateGameState(GameState newState)
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
                Debug.Log("Player died! Lose State enabled");
                HandleLose();
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(newState), newState, null);
        }
        //Invoke and notify observers 
        GameEventSystem.Current?.OnGameStateChange(newState);
    }

    private void HandleLose()
    {
        SpawnLosePopup();
    }

    void OnPlayerDeath(GameObject victim, GameObject killer, string victimName, HurtInfo hurtInfo)
    {
        if(victim.tag=="Player") Invoke("SwitchLoseState", 3); // lose after 3 seconds
    }

    void SwitchLoseState()
    {
        State = GameState.Lose;
        UpdateGameState(State);
    }

    void SpawnLosePopup()
    {
        Instantiate(gameOverPopUp);
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