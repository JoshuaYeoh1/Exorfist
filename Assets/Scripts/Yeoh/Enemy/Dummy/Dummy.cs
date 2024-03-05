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

    void OnHit(GameObject attacker, GameObject victim, float dmg, float kbForce, Vector3 contactPoint, float speedDebuffMult=.3f, float stunTime=.5f)
    {
        if(victim!=gameObject) return;

        hurt.Hurt(attacker, dmg, kbForce, contactPoint, speedDebuffMult, stunTime);
    }

    public void OnDeath(GameObject victim, GameObject killer, float dmg, float kbForce, Vector3 contactPoint)
    {
        if(victim!=gameObject) return;
        
        Instantiate(enemyPrefab, transform.position, Quaternion.identity);

        Destroy(gameObject);
    }    
}
