using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoringSystem : MonoBehaviour
{
    public int score;
    int multiplier;

    public int AttacksLanded, AttacksParried, AttacksReceived, AbilitiesUsed;

    //==Score Values==//
    [SerializeField] private int parryIncrement, hitIncrement, hurtDecrement, multiplierIncrement, enemyKillIncrement;
    private GameObject scoreRankPopupPrefab;

    void Start()
    {
        multiplier = 1;
    }

    void OnEnable()
    {
        if(GameEventSystem.current)
        {
            GameEventSystem.current.BlockEvent += IncreaseScoreParry;
            GameEventSystem.current.DeathEvent += IncreaseScoreKill;
            GameEventSystem.current.DeathEvent += IncreaseMultiplier;
            GameEventSystem.current.HitEvent += IncreaseScoreHit;
            GameEventSystem.current.HurtEvent += DecreaseMultiplier;
            GameEventSystem.current.HurtEvent += DecreaseScore;
            // GameEventSystem.current.OnPlayerParry += IncreaseScoreParry;
            // GameEventSystem.current.OnEnemyDeath += IncreaseScoreKill;
            // GameEventSystem.current.OnEnemyDeath += IncreaseMultiplier;
            // GameEventSystem.current.OnPlayerHit += IncreaseScoreHit;
            // GameEventSystem.current.OnPlayerHurt += DecreaseMultiplier;
            // GameEventSystem.current.OnPlayerHurt += DecreaseScore;
        }
    }
    void OnDisable()
    {
        if(GameEventSystem.current)
        {
            GameEventSystem.current.BlockEvent -= IncreaseScoreParry;
            GameEventSystem.current.DeathEvent -= IncreaseScoreKill;
            GameEventSystem.current.DeathEvent -= IncreaseMultiplier;
            GameEventSystem.current.HitEvent -= IncreaseScoreHit;
            GameEventSystem.current.HurtEvent -= DecreaseMultiplier;
            GameEventSystem.current.HurtEvent -= DecreaseScore;
            // GameEventSystem.current.OnPlayerParry -= IncreaseScoreParry;
            // GameEventSystem.current.OnEnemyDeath -= IncreaseScoreKill;
            // GameEventSystem.current.OnEnemyDeath -= IncreaseMultiplier;
            // GameEventSystem.current.OnPlayerHit -= IncreaseScoreHit;
            // GameEventSystem.current.OnPlayerHurt -= DecreaseMultiplier;
            // GameEventSystem.current.OnPlayerHurt -= DecreaseScore;
        }
    }
    private void IncreaseScoreParry(GameObject defender, GameObject attacker, Vector3 contactPoint, bool parry, bool broke)
    {
        if(defender.tag!="Player" && !parry) return;

        score += parryIncrement * multiplier;
    }

    private void IncreaseScoreHit(GameObject attacker, GameObject victim, float dmg, float kbForce, Vector3 contactPoint, float speedDebuffMult, float stunTime)
    {
        if(attacker.tag!="Player") return;

        score += hitIncrement * multiplier;
    }

    private void IncreaseScoreKill(GameObject victim, GameObject killer)
    {
        if(killer.tag!="Player") return;

        score += enemyKillIncrement * multiplier;
    }

    private void DecreaseScore(GameObject victim, GameObject attacker, float dmg, float kbForce, Vector3 contactPoint, float speedDebuffMult, float stunTime)
    {
        if(victim.tag!="Player") return;

        score -= hurtDecrement;
    }

    private void IncreaseMultiplier(GameObject victim, GameObject killer)
    {
        if(killer.tag!="Player") return;
        
        multiplier += multiplierIncrement;
    }

    private void DecreaseMultiplier(GameObject victim, GameObject attacker, float dmg, float kbForce, Vector3 contactPoint, float speedDebuffMult, float stunTime)
    {
        if(victim.tag!="Player") return;

        multiplier -= multiplierIncrement;
    }
}
