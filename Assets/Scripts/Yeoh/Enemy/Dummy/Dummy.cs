using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dummy : MonoBehaviour
{
    public GameObject enemyPrefab;

    void Awake()
    {
        GameEventSystem.current.OnEnemyDeath += Die;
    }

    void OnDestroy()
    {
        GameEventSystem.current.OnEnemyDeath -= Die;
    }

    public void Die(GameObject victim)
    {
        if(victim==gameObject) Instantiate(enemyPrefab, transform.position, Quaternion.identity);
    }    
}
