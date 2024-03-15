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
    [SerializeField] int parryIncrement;
    [SerializeField] int hitIncrement;
    [SerializeField] int hurtDecrement;
    [SerializeField] int multiplierIncrement;
    [SerializeField] int enemyKillIncrement;
    GameObject scoreRankPopupPrefab;

    void Start()
    {
        multiplier = 1;
    }

    void OnEnable()
    {
        GameEventSystem.Current.HitEvent += OnHit;
        GameEventSystem.Current.HurtEvent += OnHurt;
        GameEventSystem.Current.DeathEvent += OnDeath;
        GameEventSystem.Current.ParryEvent += OnParry;
    }
    void OnDisable()
    {
        GameEventSystem.Current.HitEvent -= OnHit;
        GameEventSystem.Current.HurtEvent -= OnHurt;
        GameEventSystem.Current.DeathEvent -= OnDeath;
        GameEventSystem.Current.ParryEvent -= OnParry;
    }

    public void OnHit(GameObject attacker, GameObject victim, HurtInfo hurtInfo)
    {
        if(attacker.tag=="Player")
        {
            IncreaseScore(hitIncrement);
        }
    }

    public void OnHurt(GameObject victim, GameObject attacker, HurtInfo hurtInfo)
    {
        if(victim.tag=="Player")
        {
            DecreaseScore(hurtDecrement);

            DecreaseMultiplier();
        }
    }

    public void OnDeath(GameObject victim, GameObject killer, string victimName, HurtInfo hurtInfo)
    {
        if(killer.tag=="Player")
        {
            IncreaseScore(enemyKillIncrement);

            IncreaseMultiplier();
        }
    }

    void OnParry(GameObject defender, GameObject attacker, HurtInfo hurtInfo)
    {
        if(defender.tag=="Player")
        {
            IncreaseScore(parryIncrement);
        }
    }

    void IncreaseScore(int increment)
    {
        score += increment * multiplier;
    }

    void DecreaseScore(int increment)
    {
        int temp = score - increment;
        if(temp <= 0)
        {
            score = 0;
        }
        else
        {
            score = temp;
        }
    }

    void IncreaseMultiplier()
    {
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

    void DecreaseMultiplier()
    {
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
