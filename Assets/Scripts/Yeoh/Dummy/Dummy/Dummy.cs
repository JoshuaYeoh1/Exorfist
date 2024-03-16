using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dummy : MonoBehaviour
{
    public GameObject[] enemyPrefabs;
    HurtScript hurt;

    void Awake()
    {
        hurt=GetComponent<HurtScript>();

        GameEventSystem.Current.OnSpawn(gameObject, "Dummy");
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
        
        Instantiate(enemyPrefabs[Random.Range(0, enemyPrefabs.Length)], transform.position, Quaternion.identity);

        Destroy(gameObject);
    }    
}