using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public abstract class Bullet : MonoBehaviour
{
    public string belong;
    public float damage = 10f;
    public float speed = 20f;
    public float lifetime = 1.0f;

    protected abstract void ResetDefault();

    protected abstract void BulletMove();

    protected abstract void HandleCollision(GameObject other);

    private void OnCollisionEnter2D(Collision2D collision)
    {
        HandleCollision(collision.gameObject);
    }

    protected abstract void DestroyBullet();
}