using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private GameObject enemyPrefab;
    private Vector3 spawnPos;
    private Quaternion rotation;

    private void Start()
    {
        spawnPos = transform.position;
        rotation = transform.rotation;

        //GameEventSystem.Current.OnRoomEntered += SpawnEnemy;
        GameEventSystem.Current.RoomStateChangedEvent += OnRoomStateChanged;
    }

    private void OnDestroy()
    {
        GameEventSystem.Current.RoomStateChangedEvent -= OnRoomStateChanged;
    }

    private void SpawnEnemy()
    {
        Instantiate(enemyPrefab, spawnPos, rotation);
    }

    //SpawnEnemy if the room state changes to "RoomState.Active"
    private void OnRoomStateChanged(RoomState state)
    {
        if(state == RoomState.Active)
        {
            SpawnEnemy();
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(transform.position, 1);
    }
}
