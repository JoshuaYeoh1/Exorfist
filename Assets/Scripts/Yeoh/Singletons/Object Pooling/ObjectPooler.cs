using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPooler : Monostate<ObjectPooler>
{
    [System.Serializable]
    public class Pool
    {
        public string poolName;
        public GameObject prefab;
        public int maxSize;
        
        public IPooledObject pooledObject; // if it uses OnSpawnFromPool method from the interface
    }

    public bool hideObjectsInHierarchy=true;

    public List<Pool> poolList;
    public Dictionary<string, Queue<GameObject>> poolDict;

    void Awake()
    {
        PreparePools();
    }

    void PreparePools()
    {
        poolDict = new Dictionary<string, Queue<GameObject>>();

        foreach(Pool pool in poolList)
        {
            Queue<GameObject> poolQueue = new Queue<GameObject>();

            for(int i=0; i<pool.maxSize; i++)
            {
                GameObject obj = Instantiate(pool.prefab);

                if(hideObjectsInHierarchy) obj.hideFlags = HideFlags.HideInHierarchy;

                obj.SetActive(false);

                pool.pooledObject = obj.GetComponent<IPooledObject>();

                poolQueue.Enqueue(obj);
            }

            poolDict.Add(pool.poolName, poolQueue);
        }
    }

    public GameObject SpawnFromPool(string poolName, Vector3 position, Quaternion rotation = default, Vector3 scale = default)
    {
        if(!poolDict.ContainsKey(poolName)) { Debug.LogError($"No pools have the name {poolName}"); return null; }
        
        GameObject spawnedObj = poolDict[poolName].Dequeue();

        spawnedObj.SetActive(true);
        spawnedObj.transform.position = position;
        spawnedObj.transform.rotation = rotation;
        spawnedObj.transform.localScale = scale;

        Pool pool = poolList.Find(p => p.poolName == poolName);
        if(pool!=null && pool.pooledObject!=null) pool.pooledObject.OnSpawnFromPool();

        poolDict[poolName].Enqueue(spawnedObj); // move to the back of the queue

        return spawnedObj;
    }
}
