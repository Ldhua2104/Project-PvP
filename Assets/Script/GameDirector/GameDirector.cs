using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameDirector : MonoBehaviour
{
    public static GameDirector Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject); // 如果已经存在实例，则销毁新创建的这个
        }
    }

    private void MonitorGame()
    {
        throw new System.NotImplementedException();
    }

    private void Update()
    {
        MonitorGame();
    }
}
