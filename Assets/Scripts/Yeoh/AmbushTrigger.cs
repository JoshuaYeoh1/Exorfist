using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(HiddenSpawner))]

public class AmbushTrigger : MonoBehaviour
{
    HiddenSpawner spawner;

    public List<GameObject> barriers = new List<GameObject>();

    public List<GameObject> enemyPrefabs = new List<GameObject>();
    List<GameObject> activeEnemies = new List<GameObject>();

    void Awake()
    {
        spawner=GetComponent<HiddenSpawner>();

        ToggleBarriers(false);
    }

    void OnTriggerEnter(Collider other)
    {
        Rigidbody otherRb = other.attachedRigidbody;

        if(otherRb && otherRb.gameObject.tag=="Player")
        {
            SpawnEnemies();
        }
    }

    bool roomActive;
    bool canSpawn=true;

    void SpawnEnemies()
    {
        if(canSpawn && !roomActive)
        {
            canSpawn=false;
            roomActive=true;

            ToggleBarriers(true);

            List<GameObject> spawnedEnemies = spawner.Spawns(enemyPrefabs);

            foreach(GameObject enemy in spawnedEnemies)
            {
                if(enemy.name!="Enemy1 Ragdoll")
                activeEnemies.Add(enemy);
            }

            GameEventSystem.Current.OnRoomEnter();

            AudioManager.Current.PlaySFX(SFXManager.Current.sfxUITrigger, transform.position, false);
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

    [Header("Waypoint")]
    public GameObject waypointPrefab;
    public Transform wayPointSpawnpoint;

    [Header("Last Room")]
    public bool lastRoom;
    public GameObject gameFinishPopup;
    public float popUpDelay=1;

    void Update()
    {
        CheckRoomCleared();
    }

    void CheckRoomCleared()
    {
        if(AreAllEnemiesDead() && roomActive)
        {
            roomActive=false;
            canSpawn=false;
            ToggleBarriers(false);
            activeEnemies.Clear();

            if(lastRoom)
            {
                Invoke("ShowPopup", popUpDelay);

                AudioManager.Current.PlaySFX(SFXManager.Current.sfxUIWin, transform.position, false, false);
            }
            if(waypointPrefab && wayPointSpawnpoint) 
            {
                Instantiate(waypointPrefab, wayPointSpawnpoint);
            }

            AudioManager.Current.PlaySFX(SFXManager.Current.sfxUIClear, transform.position, false, false);
        }
    }

    void ShowPopup()
    {
        Instantiate(gameFinishPopup);
    }
}
