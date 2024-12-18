using System.Collections.Generic;
using System.Linq;
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

    private Pool PlayerAmmoPool;
    private Pool EnemyAmmoPool;
    private Pool EnemyPool;
    private Pool CheckPointPool;
    private Pool ExplosionPool;
    private List<Pool> pools = new List<Pool>();
    public Dictionary<string, Queue<GameObject>> poolDictionary;
    public Dictionary<string, int> activeObjDictionary;
    
    private void setPools()
    {
        PlayerAmmoPool.prefab = Resources.Load<GameObject>("PlayerBullet");
        PlayerAmmoPool.maxSize = 30;
        PlayerAmmoPool.defaultSize = 20;
        PlayerAmmoPool.tag = "PlayerBullet";

        EnemyAmmoPool.prefab = Resources.Load<GameObject>("EnemyBullet");
        EnemyAmmoPool.maxSize = 75;
        EnemyAmmoPool.defaultSize = 50;
        EnemyAmmoPool.tag = "EnemyBullet";

        EnemyPool.prefab = Resources.Load<GameObject>("EnemyPlane");
        EnemyPool.maxSize = 12;
        EnemyPool.defaultSize = 10;
        EnemyPool.tag = "EnemyPlane";

        CheckPointPool.prefab = Resources.Load<GameObject>("CheckPoint");
        CheckPointPool.maxSize = 20;
        CheckPointPool.defaultSize = 12;
        CheckPointPool.tag = "CheckPoint";

        ExplosionPool.prefab = Resources.Load<GameObject>("ExplosionEffect");
        CheckPointPool.maxSize = 10;
        CheckPointPool.defaultSize = 8;
        CheckPointPool.tag = "Explosion";

        pools.Add(PlayerAmmoPool);
        pools.Add(EnemyAmmoPool);
        pools.Add(EnemyPool);
        pools.Add(CheckPointPool);
        pools.Add(ExplosionPool);
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
        activeObjDictionary = new Dictionary<string, int>();

        foreach (Pool pool in pools)
        {
            if (pool.prefab == null)
                continue;

            Queue<GameObject> objectPool = new Queue<GameObject>();
            int activeObjCount = 0;

            for (int i = 0; i < pool.defaultSize; i++)
            {
                GameObject obj = Instantiate(pool.prefab);
                obj.transform.SetParent(transform);
                obj.SetActive(false);
                objectPool.Enqueue(obj);
            }

            poolDictionary.Add(pool.tag, objectPool);
            activeObjDictionary.Add(pool.tag,activeObjCount);
        }
    }

    public GameObject SpawnObjectFromPool(string tag, Vector2 position)
    {
        if (!poolDictionary.ContainsKey(tag))
            return null;

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
            obj.transform.position = position;
            obj.SetActive(true);
        }
        
        activeObjDictionary[tag]++;
        
        return obj;
    }

    public void ReturnToPool(string tag,GameObject objectToReturn)
    {
        if (objectToReturn == null)
            return;

        if (poolDictionary.TryGetValue(tag, out Queue<GameObject> objectPool))
        {
            objectToReturn.SetActive(false);
            objectPool.Enqueue(objectToReturn);
        }

        activeObjDictionary[tag]--;
    }
}
