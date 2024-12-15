using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    public static ObjectPool Instance { get; private set; }

    public struct Pool
    {
        public GameObject prefab;
        public string tag;
        public int maxSize;
        public int defaultSize;
    }

    public Pool PlayerAmmoPool;
    //public Pool EnemyAmmoPool;
    //public Pool EnemyPool;
    public List<Pool> pools = new List<Pool>();
    private Dictionary<string, Queue<GameObject>> poolDictionary;

    
    private void setPools()
    {
        PlayerAmmoPool.prefab = Resources.Load<GameObject>("player bullet");
        PlayerAmmoPool.maxSize = 30;
        PlayerAmmoPool.defaultSize = 20;
        PlayerAmmoPool.tag = "PlayerBullet";

        pools.Add(PlayerAmmoPool);
    }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        setPools();

        poolDictionary = new Dictionary<string, Queue<GameObject>>();

        foreach (Pool pool in pools)
        {
            if (pool.prefab == null)
            {
                Debug.LogError("Invalid pool configuration for tag: " + pool.tag);
                continue;
            }

            Queue<GameObject> objectPool = new Queue<GameObject>();

            for (int i = 0; i < pool.defaultSize; i++)
            {
                GameObject obj = Instantiate(pool.prefab);
                obj.transform.SetParent(transform);
                obj.SetActive(false);
                objectPool.Enqueue(obj);
            }

            poolDictionary.Add(pool.tag, objectPool);
        }
    }

    public GameObject SpawnObjectFromPool(string tag, Vector2 position)
    {
        if (!poolDictionary.ContainsKey(tag))
        {
            Debug.Log("Pool with tag" + tag + "doesn't exit");
            return null;
        }

        Pool poolInfo = pools.Find(p => p.tag == tag);
        Queue<GameObject> objectPool = poolDictionary[tag];
        GameObject obj = null;

        if(objectPool.Count > 0)
        {
            obj = objectPool.Dequeue();
        }
        else if(objectPool.Count < poolInfo.maxSize)
        {
            obj = Instantiate(poolInfo.prefab);
            obj.transform.SetParent(transform);
        }

        if (obj != null)
        {
            obj.SetActive(true);
            obj.transform.position = position;
        }

        Debug.Log("left ammo After Spawning:" + objectPool.Count);

        return obj;
    }

    public void ReturnToPool(GameObject objectToReturn)
    {
        if (objectToReturn == null)
        {
            Debug.Log("Trying to return a null object to the pool.");
            return;
        }

        string tag = objectToReturn.tag;

        if (poolDictionary.TryGetValue(tag, out Queue<GameObject> objectPool))
        {
            objectToReturn.SetActive(false);
            objectPool.Enqueue(objectToReturn);
        }
        else
        {
            Debug.LogError($"No pool found for object with tag '{tag}'.");
        }

        Debug.Log("left ammo After Returing:" + objectPool.Count);
    }
}
