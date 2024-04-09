using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(BoxCollider))]
[RequireComponent(typeof(LineOfSightChecker))]

public class HiddenSpawner : MonoBehaviour
{
    BoxCollider box;
    LineOfSightChecker los;

    void Awake()
    {
        box=GetComponent<BoxCollider>();
        los=GetComponent<LineOfSightChecker>();
    }
        
    /////////////////////////////////////////////////////////////////////////////////////////////////

    public int maxRetries=999;

    public List<GameObject> Spawns(List<GameObject> spawns)
    {
        List<GameObject> spawnedObjs = new List<GameObject>();

        foreach(GameObject spawn in spawns)
        {
            bool canSpawn;

            Vector3 randomSpot;

            int retries=maxRetries;

            do
            {
                retries--;

                randomSpot = GetRandomSpotInBounds(box.bounds);
                
                randomSpot = GetNearestNavMesh(randomSpot);

                randomSpot.y+=.1f;

                canSpawn = !los.HasLineOfSight(randomSpot, Camera.main.transform.position, 1.7f);

            } while(!canSpawn && retries>0);


            if(canSpawn)
            {
                GameObject spawnedObj = Instantiate(spawn, randomSpot, Quaternion.identity);

                FaceCamera(spawnedObj);

                spawnedObjs.Add(spawnedObj);
            }
        }

        return spawnedObjs;
    }
        
    /////////////////////////////////////////////////////////////////////////////////////////////////

    Vector3 GetRandomSpotInBounds(Bounds bounds)
    {
        float x = Random.Range(bounds.min.x, bounds.max.x);
        float y = Random.Range(bounds.min.y, bounds.max.y);
        float z = Random.Range(bounds.min.z, bounds.max.z);
        return new Vector3(x, y, z);
    }

    Vector3 GetNearestNavMesh(Vector3 from)
    {
        if(NavMesh.SamplePosition(from, out NavMeshHit hit, Mathf.Infinity, NavMesh.AllAreas))
        {
            return hit.position;
        }
        return Vector3.zero;
    }
    
    void FaceCamera(GameObject obj)
    {
        Vector3 dirToCam = (Camera.main.transform.position-obj.transform.position).normalized;

        Quaternion lookRotation = Quaternion.LookRotation(dirToCam);

        obj.transform.rotation = Quaternion.Euler(0f, lookRotation.eulerAngles.y, 0f); // only rotate on the Y axis
    }
}
