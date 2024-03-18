using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomTrigger : MonoBehaviour
{
    public List<GameObject> barriers = new List<GameObject>();
    public List<Transform> enemySpawnpoints = new List<Transform>();
    public List<GameObject> enemyPrefabs = new List<GameObject>();
    List<GameObject> activeEnemies = new List<GameObject>();

    bool roomActive;
    bool canSpawn=true;

    void Awake()
    {
        ToggleBarriers(false);
    }

    void Update()
    {
        CheckRoomCleared();
    }

    void OnTriggerEnter(Collider other)
    {
        Rigidbody otherRb = other.attachedRigidbody;

        if(otherRb && otherRb.gameObject.tag=="Player")
        {
            SpawnEnemies();
        }
    }

    void SpawnEnemies()
    {
        if(canSpawn && !roomActive)
        {
            canSpawn=false;
            roomActive=true;

            ToggleBarriers(true);

            for(int i=0; i<enemySpawnpoints.Count && i<enemyPrefabs.Count; i++)
            {
                GameObject enemy = Instantiate(enemyPrefabs[i], enemySpawnpoints[i].position, enemySpawnpoints[i].rotation);

                activeEnemies.Add(enemy);
            }
        }
    }

    void ToggleBarriers(bool toggle)
    {
        if(barriers.Count==0) return;
        
        foreach(GameObject barrier in barriers)
        {
            barrier.SetActive(toggle); 
        }
    }

    void OnEnable()
    {
        GameEventSystem.Current.RespawnEvent += OnRespawn;
    }
    void OnDisable()
    {
        GameEventSystem.Current.RespawnEvent -= OnRespawn;
    }

    void OnRespawn(GameObject zombo)
    {
        if(zombo.tag!="Player") return;

        if(!AreAllEnemiesDead() && roomActive)
        {
            roomActive=false;
            DeleteEnemies();
            ToggleBarriers(false);
            canSpawn=true;
        }
    }

    bool AreAllEnemiesDead()
    {
        foreach(GameObject enemy in activeEnemies)
        {
            if(enemy) return false;
        }
        return true;
    }

    void DeleteEnemies()
    {
        foreach(GameObject enemy in activeEnemies)
        {
            if(enemy) Destroy(enemy);
        }

        activeEnemies.Clear();
    }

    void CheckRoomCleared()
    {
        if(AreAllEnemiesDead() && roomActive)
        {
            roomActive=false;
            canSpawn=false;
            ToggleBarriers(false);
            activeEnemies.Clear();
        }
    }
}
