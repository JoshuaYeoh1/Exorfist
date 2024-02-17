using UnityEngine;
using System;
using UnityEngine.SceneManagement;

public class GameEventSystem : MonoBehaviour
{

    //Static reference of the current game event system so that it can be accessed from anywhere in the game / project file.
    public static GameEventSystem current;

    void Awake()
    {
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
    public event Action OnEnemyDeath;
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
    public event Action<RoomStateManager.RoomState> OnRoomStateChanged;
    //==Transition and Room related==//

    //==GameStateManager Related==//
    public event Action<GameState> OnGameStateChanged;
    //==GameStateManager Related==//
    public void spawnEnemies()
    {
        Debug.Log("SpawnEnemies triggered");
        OnEnemySpawn?.Invoke();
    }
    public void enemyDeath()
    {
        Debug.Log("Enemy died");
        OnEnemyDeath?.Invoke(); //tysm Jon
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
    public void roomStateChange(RoomStateManager.RoomState roomState)
    {
        OnRoomStateChanged?.Invoke(roomState);
    }

    /*
    private void onSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        checkGameState();
    }
    */
    //==Misc==//
}