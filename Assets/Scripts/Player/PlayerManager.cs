using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    [SerializeField] float moveSpeed;
    [SerializeField] float jumpForce;
    [SerializeField] float dashObstacleSize;
    [SerializeField] int damagePowerWeak;
    [SerializeField] int damagePowerStrong;
    [SerializeField] GameObject hitPoint;
    [SerializeField] Transform hitRight;
    [SerializeField] Transform hitLeft;

    Rigidbody2D rb;
    SpriteRenderer rend;
    CapsuleCollider2D capsule;

    IEnumerator toHitObject;
    
    public float move { get; set; }
    public bool jump { get; set; }
    public bool dash { get; set; }
    public bool hit { get; set; }

    bool isGround;
    bool isCollision;
    public int direction = 1;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rend = GetComponent<SpriteRenderer>();
        capsule = GetComponent<CapsuleCollider2D>();

        hitPoint.SetActive(false);
    }

    
    void FixedUpdate()
    {
        Move();
        
    }

    private void Update()
    {
        Jump();
        Dash();
        ToHit();
    }

    public int GetDamagePowerWeak()
    {
        return damagePowerWeak;
    }

    public int GetDamagePowerStrong()
    {
        return damagePowerStrong;
    }

    void Move()
    {

        if (move < 0)
        {
            rend.flipX = true;
            direction = -1;
            
        }
        else if (move > 0)
        {
            rend.flipX = false;
            direction = 1;
            
        }
        //hitPoint.transform.localPosition = new Vector2(Mathf.Abs(hitPoint.transform.localPosition.x) * direction, hitPoint.transform.localPosition.y);
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

            transform.position = new Vector2(transform.position.x + dashObstacleSize * move, transform.position.y);
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

    void ToHit()
    {
        if (hit)
        {
            Debug.Log("Kick it!");
            hitPoint.transform.position = new Vector2(transform.position.x, transform.position.y);
            toHitObject = ToHitObject();
            StartCoroutine(toHitObject);


        }
        
    }

    IEnumerator ToHitObject()
    {
        hitPoint.SetActive(true);
        if (direction > 0) hitPoint.transform.position = hitRight.position;
        else hitPoint.transform.position = hitLeft.position;
        yield return new WaitForSeconds(0.1f);
        hitPoint.transform.position = new Vector2(transform.position.x, transform.position.y);
        hitPoint.SetActive(false);
        StopCoroutine(toHitObject);
    }
}
