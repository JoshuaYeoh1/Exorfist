using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private GameObject enemyPrefab;
    private Vector3 spawnPos;
    private Quaternion rotation;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            //Instantiate with parameters to prevent enemies being a child of the EnemySpawner gameObject upon spawning.
            Instantiate(enemyPrefab, spawnPos, rotation);
        }
    }

    private void Start()
    {
        spawnPos = transform.position;
        rotation = transform.rotation;
    }
}
