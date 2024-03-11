using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject[] enemyPrefabs;

    void OnEnable()
    {
        GameEventSystem.Current.RoomStateChangedEvent += OnRoomStateChanged;
    }
    void OnDestroy()
    {
        GameEventSystem.Current.RoomStateChangedEvent -= OnRoomStateChanged;
    }

    void SpawnEnemy()
    {
        Instantiate(enemyPrefabs[Random.Range(0, enemyPrefabs.Length)], transform.position, transform.rotation);
    }

    //SpawnEnemy if the room state changes to "RoomState.Active"
    void OnRoomStateChanged(RoomState state)
    {
        if(state == RoomState.Active)
        {
            SpawnEnemy();
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(transform.position, 1);
    }
}
