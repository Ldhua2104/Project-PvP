using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playermove : MonoBehaviour
{
    // Start is called before the first frame update
    public float moveSpeed;

    private  Rigidbody2D rb;

    Vector2 moveDirection;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
       
    }
    private void FixedUpdate()
    {
        //获取玩家输入
        float MoveX = Input.GetAxisRaw("Horizontal");
        float MoveY = Input.GetAxisRaw("Vertical");

        //计算移动方向
        moveDirection = new Vector2(MoveX, MoveY).normalized;

        //应用移动
        rb.velocity = moveDirection * moveSpeed;
    }
}
