using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class PlayerBullet : Bullet
{
    public bool isDestroyed = false;

    private void HandleBullets()
    {
        BulletMove();

        lifetime -= Time.deltaTime;
        if (isDestroyed == false && lifetime <= 0)
        {
            Debug.Log("Destroyed by lifetime");
            isDestroyed = true;
            DestroyBullet();
        }
    }

    protected override void BulletMove()
    {
        transform.Translate(Vector2.up * speed * Time.deltaTime);
    }

    protected override void HandleCollision(GameObject other)
    {
        if(!isDestroyed && other.CompareTag("AirWall"))
        {
            //HandleSpecialEvent();
            Debug.Log("Destroyed by AirWall");
            isDestroyed = true;
            DestroyBullet();
        }
        else if(!isDestroyed && other.CompareTag("Enemy"))
        {
            //enemy.takeDamage();
            isDestroyed = true;
            DestroyBullet();
        }
    }

    protected override void ResetDefault()
    {
        lifetime = 1.0f;
        isDestroyed = false;
    }
    protected override void DestroyBullet()
    {
        ObjectPool.Instance.ReturnToPool("PlayerBullet", this.gameObject);
        this.gameObject.SetActive(false);
        this.transform.SetParent(ObjectPool.Instance.transform);
        ResetDefault();
    }
    private void Update()
    {
        HandleBullets();
    }
}
