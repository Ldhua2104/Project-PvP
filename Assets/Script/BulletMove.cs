using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletMove : MonoBehaviour
{
    public float Speed;

    void Update()
    {
        if (transform.position.y > 7f)
        {
            Destroy(gameObject);
        }
        transform.Translate(Vector3.up * Speed * Time.deltaTime);
    }
}
