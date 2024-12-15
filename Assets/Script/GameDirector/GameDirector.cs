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
            Destroy(gameObject); // ����Ѿ�����ʵ�����������´��������
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
