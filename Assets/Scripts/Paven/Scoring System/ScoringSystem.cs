using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoringSystem : MonoBehaviour
{
    
    public int score;
    public int multiplier;

    public int AttacksLanded, AttacksParried, AttacksReceived, AbilitiesUsed;

    //==Score Values==//
    [SerializeField] private int parryIncrement, hitIncrement, hurtDecrement, multiplierIncrement, enemyKillIncrement;
    private GameObject scoreRankPopupPrefab;
    private void Start()
    {
        multiplier = 1;
        if(GameEventSystem.current != null)
        {
            GameEventSystem.current.OnPlayerParry += IncreaseScoreParry;
            GameEventSystem.current.OnEnemyDeath += IncreaseScoreKill;
            GameEventSystem.current.OnEnemyDeath += IncreaseMultiplier;
            GameEventSystem.current.OnPlayerHit += IncreaseScoreHit;
            GameEventSystem.current.OnPlayerHurt += DecreaseMultiplier;
            GameEventSystem.current.OnPlayerHurt += DecreaseScore;
        }
    }

    private void OnDestroy()
    {
        if(GameEventSystem.current != null)
        {
            GameEventSystem.current.OnPlayerParry -= IncreaseScoreParry;
            GameEventSystem.current.OnEnemyDeath -= IncreaseScoreKill;
            GameEventSystem.current.OnEnemyDeath -= IncreaseMultiplier;
            GameEventSystem.current.OnPlayerHit -= IncreaseScoreHit;
            GameEventSystem.current.OnPlayerHurt -= DecreaseMultiplier;
            GameEventSystem.current.OnPlayerHurt -= DecreaseScore;
        }
    }
    private void IncreaseScoreParry()
    {
        score += parryIncrement * multiplier;
    }

    private void IncreaseScoreHit()
    {
        score += hitIncrement * multiplier;
    }

    private void IncreaseScoreKill(GameObject victim)
    {
        score += enemyKillIncrement * multiplier;
    }

    private void DecreaseScore()
    {
        score -= hurtDecrement;
    }

    private void IncreaseMultiplier(GameObject enemy)
    {
        multiplier += multiplierIncrement;
    }

    private void DecreaseMultiplier()
    {
        multiplier -= multiplierIncrement;
    }
}
