using UnityEngine;

public abstract class Planes : MonoBehaviour
{
    public string belong;
    public int hitPoints;
    public int maxHitPoints;

    public float atk;
    public float moveSpeed;
    public Vector2 moveDirection;

    protected abstract void SetDefault();

    protected abstract void SpawnBullet();

    protected abstract void Move();

    protected abstract void ReceiveDamage();

    protected abstract void PlaySounds();

    protected abstract void Die();

    protected abstract void HandleCollision(GameObject other);

    protected virtual void OnCollisionEnter2D(Collision2D collision)
    {
        HandleCollision(collision.gameObject);
    }
}
