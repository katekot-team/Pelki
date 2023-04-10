using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    [SerializeField] float moveSpeed;
    [SerializeField] float jumpForce;
    [SerializeField] float dashObstacleSize;
    [SerializeField] int damagePowerHit;
    [SerializeField] GameObject hitPoint;
    [SerializeField] Transform hitRight;
    [SerializeField] GameObject climbUpObj;
    [SerializeField] GameObject fireball;

    Rigidbody2D rb;
    SpriteRenderer rend;
    CapsuleCollider2D capsule;
    PlatformEffector2D platformEffector2D;

    IEnumerator toHitObject;
    IEnumerator toDash;
    
    public float move { get; set; }
    public bool jump { get; set; }
    public bool dash { get; set; }
    public bool hit { get; set; }
    public bool fire { get; set; }

    bool isAlive;
    bool isGround;
    bool isWall;
    bool isCollision;
    public int direction = 1;
    Vector2 capsuleColliderSize;
    Vector2 damageVector = new Vector2(5000, 100);
    //Vector2 climbObjStartPosition;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rend = GetComponent<SpriteRenderer>();
        capsule = GetComponent<CapsuleCollider2D>();
        platformEffector2D = GetComponent<PlatformEffector2D>();

        hitPoint.SetActive(false);
        capsuleColliderSize = capsule.size;

        isAlive = true;

        //climbObjStartPosition = climbUpObj.transform.localPosition;
    }

    
    void FixedUpdate()
    {
        if (isAlive)
        {
            Move();
        }
        
        
    }

    private void Update()
    {
        if (isAlive)
        {
            Jump();
            Dash();
            ToHit();
            ToFire();

            //ClimbUp();
        }

        
    }

    private void LateUpdate()
    {
        
    }

    public int GetDamagePowerWeak()
    {
        return damagePowerHit;
    }

    //public int GetDamagePowerStrong()
    //{
    //    return damagePowerStrong;
    //}

    void Move()
    {

        if (move < 0)
        {
            //rend.flipX = true;
            direction = -1;
            
        }
        else if (move > 0)
        {
            //rend.flipX = false;
            direction = 1;
            
        }
        
        Vector2 movement = new Vector2();
        if (direction > 0) transform.eulerAngles = new Vector2(0, 0);
        else transform.eulerAngles = new Vector2(0, 180);
        movement = new Vector2(move, 0) * moveSpeed;
        movement = Vector2.ClampMagnitude(movement, moveSpeed);
        movement.y = rb.velocity.y;
        //if(!isCollision || isGround)rb.velocity = movement;
        rb.velocity = movement;

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

    void Dash()//проход сквозь препятствия
    {
        if (dash && !isWall)
        {
            if(GameManager.energyItem > 0)
            {
                toDash = ToDash();
                StartCoroutine(toDash);
                GameManager.energyItem--;
            }

            
        }
         
    }

    
    IEnumerator ToDash()
    {
        Vector2 lastPosition = transform.position;
        dash = false;
        float time = 100;
        capsule.size = new Vector2(capsuleColliderSize.x/2, capsule.size.y);
        gameObject.tag = "Untagged";
        platformEffector2D.enabled = true;
        Color32 defaulColor = rend.color;
        rend.color = new Color32(0, 0, 0, 100);//цвет при активном dash
        while (true)
        {
            if (isWall) 
            {
                transform.position = lastPosition;
                break;
            } 
            yield return new WaitForSeconds(0.01f);
            time--;
            if (time <= 0) break;
            
        }
        rend.color = defaulColor;
        gameObject.tag = "Player";
        capsule.size = capsuleColliderSize;
        platformEffector2D.enabled = false;

        StopCoroutine(toDash);
    }

    IEnumerator climbUp;
    IEnumerator ClimbUp(Transform posFinish)//доделать залазанье на платформу, в часности двигать персонажа к платформе по горизонтали
    {
        float elasted = 0;
        float translateTime = Vector2.Distance(transform.position, climbUpObj.transform.position);
        Debug.Log("climb up to");
        climbUpObj.SetActive(true);
        rb.simulated = false;
        Vector2 positionUp = climbUpObj.transform.position;
        while(elasted < translateTime)
        {
            transform.position = Vector2.Lerp(transform.position, new Vector2(transform.position.x, posFinish.position.y + capsule.size.y/2), elasted/translateTime);
            elasted += Time.deltaTime;
            yield return null;
        }
        
        //yield return new WaitForSeconds(1);
        rb.simulated = true;
        climbUpObj.SetActive(false);
        StopCoroutine(climbUp);

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        
        if (collision.gameObject.tag == "Wall") 
        {
            isWall = true;
        } 
        if(collision.gameObject.tag == "Untagged")
        {
            
            if (!isGround)
            {
                climbUp = ClimbUp(collision.gameObject.transform);
                StartCoroutine(climbUp);
            }
        }

    }


    private void OnCollisionExit2D(Collision2D collision)
    {
        
        if (collision.gameObject.tag == "Wall") isWall = false;
        if (collision.gameObject.tag == "Untagged")
        {

        }
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

    void ToFire()
    {
        if (fire)
        {
            if(GameManager.energyItem > 0)
            {
                Transform shootPoint;
                shootPoint = hitRight;;
                Instantiate(fireball, shootPoint.position, shootPoint.rotation);
                GameManager.energyItem--;
            }

        }
    }

    IEnumerator ToHitObject()
    {
        hitPoint.SetActive(true);
        hitPoint.transform.position = hitRight.position;
        yield return new WaitForSeconds(0.1f);
        hitPoint.transform.position = new Vector2(transform.position.x, transform.position.y);
        hitPoint.SetActive(false);
        StopCoroutine(toHitObject);
    }

    IEnumerator toDamage;
    public void Damage(int direct, int value)
    {
        
        Debug.Log("player DAMAGE!");
        rb.AddForce(new Vector2(damageVector.x * direct, damageVector.y), ForceMode2D.Force);
        toDamage = ToDamage();
        StartCoroutine(toDamage);

        IEnumerator ToDamage()
        {
            Color32 defaulColor = rend.color;
            rend.color = new Color32(255, 0, 0, 100);//цвет при активном damage
            yield return new WaitForSeconds(0.3f);
            rend.color = defaulColor;
            StopCoroutine(toDamage);
        }
        if(GameManager.health > 0)
        {
            GameManager.health-=value;
            
        }
        else
        {
            isAlive = false;
        }
    }


}
