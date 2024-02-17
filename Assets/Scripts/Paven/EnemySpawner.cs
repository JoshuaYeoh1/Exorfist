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

        GameEventSystem.current.OnRoomEntered += SpawnEnemy;
    }

    private void OnDestroy()
    {
        GameEventSystem.current.OnRoomEntered -= SpawnEnemy;
    }
    private void SpawnEnemy()
    {
        Instantiate(enemyPrefab, spawnPos, rotation);
    }
}
