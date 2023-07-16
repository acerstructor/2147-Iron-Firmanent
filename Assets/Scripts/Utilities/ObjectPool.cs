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

    public T SpawnFromPool<T>(string tag, Vector2 pos, Quaternion rotation) where T : MonoBehaviour
    {
        if (!_poolDictionary.ContainsKey(tag))
        {
            Debug.LogWarning($"Pool with tag {tag} doesn't exist.");
            return null;
        }

        GameObject objectToSpawn = _poolDictionary[tag].Dequeue();
        objectToSpawn.SetActive(true);
        objectToSpawn.transform.position = pos;
        objectToSpawn.transform.rotation = rotation;

        T component = objectToSpawn.GetComponent<T>();

        if (component == null)
        {
            Debug.LogError($"Object with tag {tag} does not have the specified component.");
            return null;
        }

        _poolDictionary[tag].Enqueue(objectToSpawn);

        return component;
    }

    public GameObject SpawnFromPool(string tag, Vector2 pos, Quaternion rotation)
    {
        if (!_poolDictionary.ContainsKey(tag))
        {
            Debug.LogWarning($"Pool with tag {tag} doesn't exist.");
            return null;
        }

        GameObject objectToSpawn = _poolDictionary[tag].Dequeue();

        objectToSpawn.SetActive(true);
        objectToSpawn.transform.position = pos;
        objectToSpawn.transform.rotation = rotation;

        _poolDictionary[tag].Enqueue(objectToSpawn);

        return objectToSpawn;
    }
}
