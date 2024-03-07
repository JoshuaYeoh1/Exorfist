using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoringSystem : MonoBehaviour
{
    public int score;
    public int multiplier, multiplierMax;

    public int AttacksLanded, AttacksParried, AttacksReceived, AbilitiesUsed;

    [Header("Score Values")]
    //==Score Values==//
    [SerializeField] private int parryIncrement;
    [SerializeField] private int hitIncrement;
    [SerializeField] private int hurtDecrement;
    [SerializeField] private int multiplierIncrement;
    [SerializeField] private int enemyKillIncrement;
    private GameObject scoreRankPopupPrefab;

    void Start()
    {
        multiplier = 1;
    }

    void OnEnable()
    {
        GameEventSystem.Current.BlockEvent += IncreaseScoreParry;
        GameEventSystem.Current.DeathEvent += IncreaseScoreKill;
        GameEventSystem.Current.DeathEvent += IncreaseMultiplier;
        GameEventSystem.Current.HitEvent += IncreaseScoreHit;
        GameEventSystem.Current.HurtEvent += DecreaseMultiplier;
        GameEventSystem.Current.HurtEvent += DecreaseScore;
    }
    void OnDisable()
    {
        GameEventSystem.Current.BlockEvent -= IncreaseScoreParry;
        GameEventSystem.Current.DeathEvent -= IncreaseScoreKill;
        GameEventSystem.Current.DeathEvent -= IncreaseMultiplier;
        GameEventSystem.Current.HitEvent -= IncreaseScoreHit;
        GameEventSystem.Current.HurtEvent -= DecreaseMultiplier;
        GameEventSystem.Current.HurtEvent -= DecreaseScore;
    }
    
    private void IncreaseScoreParry(GameObject defender, GameObject attacker, Vector3 contactPoint, bool parry, bool broke)
    {
        if(defender.tag!="Player" || !parry) return;

        score += parryIncrement * multiplier;
    }

    private void IncreaseScoreHit(GameObject attacker, GameObject victim, float dmg, float kbForce, Vector3 contactPoint, float speedDebuffMult, float stunTime)
    {
        if(attacker.tag!="Player") return;

        score += hitIncrement * multiplier;
    }

    private void IncreaseScoreKill(GameObject victim, GameObject killer, float dmg, float kbForce, Vector3 contactPoint)
    {
        if(killer.tag!="Player") return;

        score += enemyKillIncrement * multiplier;
        
    }

    private void DecreaseScore(GameObject victim, GameObject attacker, float dmg, float kbForce, Vector3 contactPoint, float speedDebuffMult, float stunTime)
    {
        if(victim.tag!="Player") return;

        int temp = score - hurtDecrement;
        if(temp <= 0)
        {
            score = 0;
        }
        else
        {
            score = temp;
        }
        
    }

    private void IncreaseMultiplier(GameObject victim, GameObject killer, float dmg, float kbForce, Vector3 contactPoint)
    {
        if(killer.tag!="Player") return;
        
        int temp = multiplier + multiplierIncrement;
        if(temp >= multiplierMax)
        {
            multiplier = multiplierMax;
        }
        else
        {
            multiplier = temp;
        }
    }

    private void DecreaseMultiplier(GameObject victim, GameObject attacker, float dmg, float kbForce, Vector3 contactPoint, float speedDebuffMult, float stunTime)
    {
        if(victim.tag!="Player") return;

        int temp = multiplier - multiplierIncrement;
        if (temp <= 1)
        {
            multiplier = 1;
        }
        else
        {
            multiplier = temp;
        }
    }
}
