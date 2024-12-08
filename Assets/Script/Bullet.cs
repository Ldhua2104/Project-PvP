using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class buttle : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject bu;
    int randomNumber;

    public float angel=9;
    void Start()
    {
        randomNumber = Random.Range(1, 4);
    }

    // Update is called once per frame
    void Update()
    {
       
        switch(randomNumber)
        {
            case 1: Attack1();
                    break;
            case 2: Attack2();
                break;
            case 3:Attack3();
                break;
        }
    }
    void Attack1()
    {
        for (float time = 0; time < 10f; time += Time.deltaTime)
        {
            for (int i = 0; i < 10; i++)
            {
                GameObject tembu = Instantiate(bu, transform.position, transform.rotation);
                tembu.transform.Rotate(0, 0, angel * i);
                tembu.AddComponent<BulletMove>();
            }
            for (int i = 0; i > -10; i--)
            {
                GameObject tembu = Instantiate(bu, transform.position, transform.rotation);
                tembu.transform.Rotate(0, 0, angel * i);
                tembu.AddComponent<BulletMove>();
            }
        }
    }
    void Attack2()
    {
        for (float time = 0; time < 10f; time += Time.deltaTime)
        {
            Vector3 position = transform.position;
            for (int i = -2; i < 3; i++)
            {
                GameObject tembu = Instantiate(bu, transform.position, transform.rotation);
                tembu.transform.Rotate(0, 0, angel * i);
                tembu.AddComponent<BulletMove>();
            }
            for (int i = -2; i <3; i--)
            {
                GameObject tembu = Instantiate(bu, transform.position, transform.rotation);
                position.x += 3 * i;
                tembu.transform.position = position;
                tembu.AddComponent<BulletMove>();
            }
        }
    }
    void Attack3()
    {
        for (float time = 0; time < 10f; time += Time.deltaTime)
        {
            for (int i = -10; i < 10; i++)
            {
                GameObject tembu = Instantiate(bu, transform.position, transform.rotation);
                tembu.transform.Rotate(0, 0, angel * i);
                tembu.AddComponent<BulletMove>();
            }

        }
    }

}



