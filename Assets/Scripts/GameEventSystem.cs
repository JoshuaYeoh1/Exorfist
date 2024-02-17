using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;

public class GameEventSystem : MonoBehaviour
{

    //Static reference of the current game event system so that it can be accessed from anywhere in the game / project file.
    public static GameEventSystem current;



    private void Awake()
    {
        current = this;
        SceneManager.sceneLoaded += onSceneLoaded;
    }

    //==Player Related Actions==//
    public event Action onPlayerHit;
    public event Action onPlayerHurt;
    public event Action onPlayerBlock;
    public event Action onPlayerParry;
    public event Action onPlayerDeath;
    //==Player Related Actions==//

    //==Enemy Related Actions==//
    public event Action onEnemyDeath;
    public event Action onEnemySpawn;
    //==Enemy Related Actions==//

    //==Objective Related actions==//
    public event Action onScoreReached;
    public event Action onLevelFinish;
    public event Action onLevelStart;
    //==Objective Related actions==//


    //==Enemy Related==//
    public void spawnEnemies()
    {
        Debug.Log("SpawnEnemies triggered");
        onEnemySpawn?.Invoke();
    }
    public void enemyDeath()
    {
        Debug.Log("Enemy died");
        onEnemyDeath?.Invoke(); //tysm Jon
    }
    //==Enemy Related==//



    //==Player Related==//
    public void playerHit()
    {
        Debug.Log("Player hit something");
        onPlayerHit?.Invoke();
    }
    public void playerHurt()
    {
        Debug.Log("Player got hurt :(");
        onPlayerHurt?.Invoke();
    }
    public void playerBlock()
    {
        //Debug.Log("PlayerTurn started");
        onPlayerBlock?.Invoke();
    }
    public void playerParry()
    {
        Debug.Log("onplayer parry func");
        onPlayerParry?.Invoke();
    }
    public void playerDeath()
    {
        Debug.Log("playerDeath()");
        onPlayerDeath?.Invoke();
    }
    //==Player Related==//



    //==Objective Related==//
    public void scoreReached()
    {
        Debug.Log("Score reached or whatever");
        onScoreReached?.Invoke();
    }
    public void levelStart()
    {
        Debug.Log("Level started");
        onLevelStart?.Invoke();
    }
    public void levelFinish()
    {
        Debug.Log("Level finished");
        onLevelFinish?.Invoke();
    }
    //==Objective Related==//

    //==Changing Scene Related==//
    private void checkGameState()
    {
        
    }

    private void onSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        checkGameState();
    }
    //==Changing Scene Related==//
}