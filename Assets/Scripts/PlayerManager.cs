using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    [SerializeField] float moveSpeed;
    [SerializeField] float jumpForce;
    [SerializeField] float normalY = 80;

    Rigidbody2D rb;
    SpriteRenderer rend;
    CapsuleCollider2D capsule;
    IEnumerator toDash;
    public float move { get; set; }
    public bool jump { get; set; }
    public bool dash { get; set; }

    bool isGround;
    bool isCollision;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rend = GetComponent<SpriteRenderer>();
        capsule = GetComponent<CapsuleCollider2D>();

        
    }

    
    void FixedUpdate()
    {
        Move();
        
    }

    private void Update()
    {
        Jump();
        Dash();
    }

    void Move()
    {

        if (move < 0) rend.flipX = true;
        else if (move > 0) rend.flipX = false;
        Vector2 movement = new Vector2();
        movement = new Vector2(move, 0) * moveSpeed;
        movement = Vector2.ClampMagnitude(movement, moveSpeed);
        movement.y = rb.velocity.y;
        if(!isCollision || isGround)rb.velocity = movement;
        

    }

    void Jump()
    {
        if (jump)
        {
            if (isGround) 
            {
                rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            } 

        }

        Vector2 max = capsule.bounds.max;
        Vector2 min = capsule.bounds.min;
        Vector2 corner1 = new Vector2(max.x, min.y - 0.1f);
        Vector2 corner2 = new Vector2(min.x, min.y - 0.1f);
        Collider2D hit = Physics2D.OverlapArea(corner1, corner2);

        isGround = false;
        if (hit != null) isGround = true;

    }

    void Dash() 
    {
        if (dash)
        {

            transform.position = new Vector2(transform.position.x + 3 * move, transform.position.y);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(!isGround) isCollision = true;


    }


    private void OnCollisionExit2D(Collision2D collision)
    {
        if (!isGround) isCollision = false;
        
    }


}
