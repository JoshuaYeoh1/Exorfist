using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dummy : MonoBehaviour
{
    public bool spawnEnemy=true;
    public GameObject ragdollPrefab;
    public GameObject[] enemyPrefabs;
    HurtScript hurt;

    void Awake()
    {
        hurt=GetComponent<HurtScript>();
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

    void Start()
    {
        GameEventSystem.Current.OnSpawn(gameObject, "Dummy");
    }

    void OnHit(GameObject attacker, GameObject victim, HurtInfo hurtInfo)
    {
        if(victim!=gameObject) return;

        hurt.Hurt(attacker, hurtInfo);
    }

    public void OnDeath(GameObject victim, GameObject killer, HurtInfo hurtInfo)
    {
        if(victim!=gameObject) return;
        
        if(spawnEnemy)
        Instantiate(enemyPrefabs[Random.Range(0, enemyPrefabs.Length)], transform.position, Quaternion.identity);

        Ragdoller ragdoll = Instantiate(ragdollPrefab, transform.position, transform.rotation).GetComponent<Ragdoller>();

        ragdoll.PushRagdoll(hurtInfo.kbForce, hurtInfo.contactPoint);

        Destroy(gameObject);
    }    
}
