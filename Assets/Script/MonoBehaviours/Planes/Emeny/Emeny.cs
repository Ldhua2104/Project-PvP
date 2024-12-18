using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class Emeny : Planes
{
    //public string fireMode;
    Rigidbody2D rb;

    public AudioSource m_AudioSource;
    public AudioClip m_Clip;

    private bool isTargeting;
    private bool isMax;
    private Vector2 impossiblePoint = new Vector2(99, 99);
    private Vector2 spawnPoint;
    private Vector2 target;
    
    private List<Vector2> haveCheckedPoint = new List<Vector2>();
    private Queue<Vector2> targetPoints = new Queue<Vector2>();
  
    protected override void SetDefault()
    {
        isTargeting = false;
        isMax = (Random.Range(0, 2) == 1);
        spawnPoint = transform.position;
        target = impossiblePoint;
        belong = "EnemyPlane";
        hitPoints = 50;
        maxHitPoints = 50;
        atk = 80;
        moveSpeed = 4;
    }

    private void SetTargetPoint()
    {
        Vector2 temp = impossiblePoint;

        if(haveCheckedPoint.Count == GameDirector.Instance.checkPoints.Count)
        {
            temp = spawnPoint;
            targetPoints.Enqueue(temp);
            
            return;
        }

        float distance = 0.0f;
        float maxDistance = 0.0f;
        float minDistance = 100.0f;
        
        foreach (Vector2 node in GameDirector.Instance.checkPoints)
        {
            if (haveCheckedPoint.Contains(node))
                continue;

            distance = (node - (Vector2)transform.position).magnitude;

            if (isMax == true)
            {
                if (distance > maxDistance)
                {
                    maxDistance = distance;
                    temp = node;
                }
            }
            else
            {
                if (distance < minDistance)
                {
                    minDistance = distance;
                    temp = node;
                }
            }
        }

        isMax = (Random.Range(0, 2) == 1); ;
        targetPoints.Enqueue(temp);
    }

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        m_AudioSource = rb.GetComponent<AudioSource>();
        m_AudioSource.clip = m_Clip;
    } 

    private void OnEnable()
    {
        SetTargetPoint();
        SetDefault();
    }

    private void MoveToPoints()
    {
        Vector2 direction = (target - (Vector2)transform.position).normalized;
        rb.velocity = moveSpeed * direction;

        if(target == spawnPoint && (spawnPoint - (Vector2)transform.position).magnitude <= 2f)
            DestroyPlane();
    }

    protected override void Move()
    {
        if(isTargeting == false)
        {
            target = targetPoints.Dequeue();
            isTargeting = true;
        }
        
        MoveToPoints();
    }

    private void Update()
    {
        Move();
        Debug.DrawLine(transform.position, target, Color.red);
        //Debug.DrawLine(transform.position, targetPoints.Peek(), Color.blue);
    }

    protected override void PlaySounds()
    {
        GameObject explosion =  ObjectPool.Instance.SpawnObjectFromPool("Explosion", transform.position);
        ObjectPool.Instance.ReturnToPool("Explosion", explosion);
    }

    protected override void Die()
    {
        PlaySounds();
        DestroyPlane();
    }

    protected override void ReceiveDamage()
    {
        throw new System.NotImplementedException();
    }

    protected override void SpawnBullet()
    {
        throw new System.NotImplementedException();
    }

    private void DestroyPlane()
    {
        ObjectPool.Instance.ReturnToPool("EnemyPlane",this.gameObject);
        this.gameObject.SetActive(false);
        this.transform.SetParent(ObjectPool.Instance.transform);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("CheckPoint") && target == (Vector2)collision.gameObject.transform.position)
        {
            haveCheckedPoint.Add(target);
            isTargeting = false;

            SetTargetPoint();
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("CheckPoint") && target == (Vector2)collision.gameObject.transform.position && !haveCheckedPoint.Contains(target))
        {
            haveCheckedPoint.Add(target);
            isTargeting = false;

            SetTargetPoint();
        }
    }

    protected override void HandleCollision(GameObject other)
    {
        if(other.CompareTag("Player"))
        {
            Die();
        }

        if(other.CompareTag("PlayerBullet"))
        {
            ReceiveDamage();
        }
    }
}
