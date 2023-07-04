using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spine.Unity;

public enum AnimationPlayer
{
    idle,
    run,
    run_jump,
    jump_up_state,
    jump_up_to_down,
    cast_fireball,
    kick,
}

public class PlayerManager : MonoBehaviour
{
    [SerializeField] SkeletonAnimation skeletonAnimation;
    [SerializeField] float moveSpeed;
    [SerializeField] float jumpForce;
    [SerializeField] float dashObstacleSize;
    [SerializeField] float castFireballDelay;
    [SerializeField] int damagePowerHit;
    [SerializeField] GameObject hitPoint;
    [SerializeField] Transform hitRight;
    [SerializeField] GameObject fireball;
    [SerializeField] Transform pointCompanion;

    Rigidbody2D rb;
    SpriteRenderer rend;
    CapsuleCollider2D capsule;
    Spine.AnimationState spineAnimationState;

    
    IEnumerator toHitObject;
    
    public float move { get; set; }
    public bool toMove { get; set; }
    public bool jump { get; set; }
    public bool dash { get; set; }
    public bool hit { get; set; }
    public bool fire { get; set; }
    

    bool isAlive;
    bool isGround;
    public int direction = 1;
    Vector2 capsuleColliderSize;
    Vector2 damageVector = new Vector2(5000, 100);

    [SerializeField] Animation anim;

    public Transform GetPointCompanion() { return pointCompanion; }




    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rend = GetComponent<SpriteRenderer>();
        capsule = GetComponent<CapsuleCollider2D>();

        spineAnimationState = skeletonAnimation.AnimationState;

        hitPoint.SetActive(false);
        capsuleColliderSize = capsule.size;

        isAlive = true;

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

        }

        
    }

    public void SetAnimation(AnimationPlayer anim, bool loop = false)
    {
        spineAnimationState.SetAnimation(0, anim.ToString(), loop);

    }

    public int GetDamagePowerWeak()
    {
        return damagePowerHit;
    }


    void Move()
    {

        if (move < 0)
        {
            direction = -1;
            
        }
        else if (move > 0)
        {
            direction = 1;
        }
        
        Vector2 movement = new Vector2();
        if (direction > 0) transform.eulerAngles = new Vector2(0, 0);
        else transform.eulerAngles = new Vector2(0, 180);
        movement = new Vector2(move, 0) * moveSpeed;
        movement = Vector2.ClampMagnitude(movement, moveSpeed);
        movement.y = rb.velocity.y;
        rb.velocity = movement;

    }

    public void ClickJoystick(bool click)
    {
        if (click) SetAnimation(AnimationPlayer.run, true);
        else SetAnimation(AnimationPlayer.idle, true);
    }

    void Jump()
    {
        if (jump)
        {
            if (isGround) 
            {
                rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
                StartCoroutine(JumpAnim());
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

    IEnumerator JumpAnim()
    {
        SetAnimation(AnimationPlayer.jump_up_to_down);
        yield return new WaitForSeconds(0.7f);
        if(rb.velocity.x == 0)SetAnimation(AnimationPlayer.idle);
        else SetAnimation(AnimationPlayer.run);
    }

    Transform RayCast()
    {
        RaycastHit2D[] hits;
        Debug.DrawRay(hitRight.transform.position, transform.right * dashObstacleSize, Color.red);

        hits = Physics2D.RaycastAll(hitRight.transform.position, transform.right, dashObstacleSize);

        if(hits.Length > 0)
        {
            return hits[hits.Length - 1].transform;

        }
        return transform;
    }

    void Dash()
    {
        if (dash && GameManager.haveCompanion)
        {
            
            Transform target = RayCast();
            if(target.tag != "Wall")
            {
                float sizeTarget = target.lossyScale.x;
                if(sizeTarget < dashObstacleSize)transform.position = new Vector2(target.position.x + (sizeTarget + capsule.size.x) * direction, transform.position.y);
                GameManager.energy--;
            }

        }
        
    }


    IEnumerator climbUp;
    IEnumerator ClimbUp(Transform posFinish)
    {
        Debug.Log("climb up to");
        rb.simulated = false;
        float elapsed = 0;
        
        Vector2 targetUpPosition = new Vector2(transform.position.x, posFinish.position.y + capsule.size.y / 2 + posFinish.localScale.y / 2);
        float timeUp = Vector2.Distance(transform.position, targetUpPosition)/4;
        while (elapsed < timeUp)
        {
            transform.position = Vector2.Lerp(transform.position, targetUpPosition, elapsed/timeUp);
            elapsed += Time.deltaTime;
            yield return null;
        }
        
        elapsed = 0;
        Vector2 targetTranslatePosition = new Vector2(transform.position.x + capsule.size.x * direction, transform.position.y);
        float timeTranslate = Vector2.Distance(transform.position, targetTranslatePosition)/4;
        while (elapsed < timeTranslate)
        {
            transform.position = Vector2.Lerp(transform.position, targetTranslatePosition, elapsed / timeTranslate);
            elapsed += Time.deltaTime;
            yield return null;
        }

        rb.simulated = true;
        StopCoroutine(climbUp);

    }

    public void TakeACompanion()
    {
        GameManager.haveCompanion = true;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {

        if(collision.gameObject.tag == "Untagged")
        {
            
            if (!isGround)
            {
                Transform platformTransform = collision.gameObject.transform;
                if (platformTransform.position.x + platformTransform.localScale.x/2 < transform.position.x || platformTransform.position.x - platformTransform.localScale.x / 2 > transform.position.x)
                {
                    climbUp = ClimbUp(collision.gameObject.transform);
                    StartCoroutine(climbUp);
                }


            }
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
        if (fire && GameManager.haveCompanion)
        {
            if(GameManager.energy > 0)
            {
                StartCoroutine(CastFireball());
               
            }

        }
    }

    IEnumerator CastFireball()
    {
        SetAnimation(AnimationPlayer.cast_fireball);
        yield return new WaitForSeconds(castFireballDelay);
        Transform shootPoint;
        shootPoint = hitRight;
        Instantiate(fireball, shootPoint.position, shootPoint.rotation);
        GameManager.energy--;
    }

    IEnumerator ToHitObject()
    {
        SetAnimation(AnimationPlayer.kick);
        yield return new WaitForSeconds(0.3f);
        hitPoint.SetActive(true);
        
        hitPoint.transform.position = hitRight.position;
        yield return new WaitForSeconds(0.1f);
        
        hitPoint.transform.position = new Vector2(transform.position.x, transform.position.y);
        hitPoint.SetActive(false);
        yield return new WaitForSeconds(0.5f);
        SetAnimation(AnimationPlayer.idle, true);
        
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
