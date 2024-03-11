using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dummy : MonoBehaviour
{
    public GameObject enemyPrefab;
    EnemyHurt hurt;

    void Awake()
    {
        hurt=GetComponent<EnemyHurt>();

        GameEventSystem.Current.OnSpawn(gameObject);
    }

    void OnEnable()
    {
        GameEventSystem.Current.HitEvent += OnHit;
        GameEventSystem.Current.DeathEvent += OnDeath;
    }
    void OnDisable()
    {
        GameEventSystem.Current.HitEvent -= OnHit;
        GameEventSystem.Current.DeathEvent -= OnDeath;
    }

    void OnHit(GameObject attacker, GameObject victim, HurtInfo hurtInfo)
    {
        if(victim!=gameObject) return;

        hurt.Hurt(attacker, hurtInfo);
    }

    public void OnDeath(GameObject victim, GameObject killer, HurtInfo hurtInfo)
    {
        if(victim!=gameObject) return;
        
        Instantiate(enemyPrefab, transform.position, Quaternion.identity);

        Destroy(gameObject);
    }    
}
