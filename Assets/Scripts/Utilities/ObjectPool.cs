using System;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : Singleton<ObjectPool>
{
    [Serializable]
    private class Pool
    {
        public string Tag;
        public GameObject Prefab;
        public int Size;
    }

    [SerializeField] private List<Pool> Pools;

    private Dictionary<string, Queue<GameObject>> _poolDictionary;

    protected override void Awake()
    {
        base.Awake();

        _poolDictionary = new Dictionary<string, Queue<GameObject>>();

        foreach (var pool in Pools)
        {
            var objectPool = new Queue<GameObject>();

            for (int i = 0; i < pool.Size; i++)
            {
                GameObject obj = Instantiate(pool.Prefab);
                obj.SetActive(false);
                objectPool.Enqueue(obj);
            }

            _poolDictionary.Add(pool.Tag, objectPool);
        }
    }

    public GameObject SpawnFromPool(string tag, Vector2 pos, Quaternion rotation)
    {
        if (!_poolDictionary.ContainsKey(tag))
        {
            Debug.LogWarning($"Pool with tag {tag} doesn't exist.");
            return null;
        }

        GameObject objectToSpawn = _poolDictionary[tag].Dequeue();
        
        objectToSpawn.transform.position = pos;
        objectToSpawn.transform.rotation = rotation;
        objectToSpawn.SetActive(true);

        _poolDictionary[tag].Enqueue(objectToSpawn);

        return objectToSpawn;
    }
}
