using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartGame : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    float timer = 0.0f;

    IEnumerator loadScene()
    {
        SceneManager.LoadScene("MainScene");
        AsyncOperation operation = SceneManager.LoadSceneAsync(1);
        yield return operation; 
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        
        if(Input.GetMouseButtonDown(0))
        {
            //SceneManager.LoadScene("MainScene");
            
            StartCoroutine(loadScene());

            Debug.Log("has jumped!");
            Debug.Log("used time:" + timer);
        }
    }
}
