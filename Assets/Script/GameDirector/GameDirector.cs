using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static GameDirector;

public class GameDirector : MonoBehaviour
{
    public AudioSource audioSource;

    public struct SpawnPoint
    {
        public Vector2 position;
        public bool isBelonged;
    }
    
    public List<Vector2> checkPoints    = new List<Vector2>();
    public List<SpawnPoint> spawnPoints = new List<SpawnPoint>();
    public GameObject CheckPointPrefab;

    public float nextWave;
    public int   waveCount;
    public int   livingEnemyCount;
    public bool  isclear;
    private bool isCountingDown;

    public static GameDirector Instance { get; private set; }

    private void SetCheckPoint()
    {
        int min = 5;
        int max = 12;
        int randomNum = Random.Range(min, max + 1);

        for(int i = 0; i < randomNum; i++)
        {
            float x = Random.Range(-4f, 13f);
            float y = Random.Range(-8f, 9f);
            Vector2 checkPoint = new Vector2(x, y);
            checkPoints.Add(checkPoint);

            ObjectPool.Instance.SpawnObjectFromPool("CheckPoint", checkPoint);
        }
    }

    private void SetSpawnPoint()
    {
        int num = 10;
        SpawnPoint point;
        for (int i = 0; i < num; i++)
        {
            float x = Random.Range(-4f, 13f);
            float y = Random.Range(11f, 13f);

            point.position = new Vector2(x, y);
            point.isBelonged = false;
            spawnPoints.Add(point);
        }
    }

    public void AssignSpawnPoint(int index,bool isBelonged)
    {
        SpawnPoint selectedSpawnPoint = spawnPoints[index];
        selectedSpawnPoint.isBelonged = isBelonged;
        selectedSpawnPoint.position = spawnPoints[index].position;

        spawnPoints[index] = selectedSpawnPoint;
    }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        audioSource = GetComponent<AudioSource>();
        audioSource.loop = true;
        audioSource.volume = 0.068f;
        audioSource.Play();

        nextWave = 3.0f;
        livingEnemyCount = 0;
        waveCount = 0;
        isCountingDown = true;
    }

    private void MonitorGame()
    {
        EnemySpawnManager();
    }

    private void EnemySpawnManager()
    {
        if (isCountingDown)
            nextWave -= Time.deltaTime;
        else
            nextWave = 4.0f;

        livingEnemyCount = ObjectPool.Instance.activeObjDictionary["EnemyPlane"];
 
        if (livingEnemyCount == 0)
        {
            isclear = true;
            isCountingDown = true;
        }
            
        if (isclear == true && nextWave <= 0)
        {
            SetCheckPoint();
            SetSpawnPoint();
            int enemyCount = Random.Range(3,11);
            for (int i = 0; i < enemyCount; i++)
                SpawnEnemy();

            isclear = false;
            isCountingDown = false;
            waveCount++;
        }
    }

    private void SpawnEnemy() 
    {
        int index = Random.Range(0,spawnPoints.Count);
        while (spawnPoints[index].isBelonged == true)
            index = (index + 1) % spawnPoints.Count;

        AssignSpawnPoint(index, true);
        ObjectPool.Instance.SpawnObjectFromPool("EnemyPlane", spawnPoints[index].position);
    }

    private void Update()
    {
        MonitorGame();
    }
}