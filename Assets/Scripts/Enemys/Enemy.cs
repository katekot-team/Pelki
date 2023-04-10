using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class Enemy : MonoBehaviour
{
    [SerializeField] EnemyData enemyData;
    [SerializeField] Behavior behavior;
    [SerializeField] GameObject item;
    [SerializeField] Transform pointRaycastEmit;

    GameObject itemObject;
    Rigidbody2D rb;

    float detectionDistance;
    int health;
    float speed;
    float speedAttack;

    public float moveSpeed;

    public int moveDirection { get; set; }
    public bool isAttack { get; set; }
    


    enum Behavior
    {
        active,
        notactive,
    }


    void Start()
    {
        health = enemyData.health;
        detectionDistance = enemyData.detectionDistance;
        speed = enemyData.speed;
        speedAttack = enemyData.speedAttack;
        moveDirection = -1;

        rb = GetComponent<Rigidbody2D>();

        isAttack = false;

        if (item != null)
        {
            itemObject = Instantiate(item, new Vector2(transform.position.x, transform.position.y + 3), Quaternion.identity);
            itemObject.SetActive(false);
        }

        BehaviorEnemy();
    }


    void FixedUpdate()
    {
        Move();
        Raycast();


        if (health <= 0) ToDestroy();
    }


    void BehaviorEnemy()
    {
        switch (behavior)
        {
            case Behavior.notactive:
                isAttack = false;
                moveSpeed = speed;
                break;
            case Behavior.active:
                isAttack = true;
                moveSpeed = speedAttack;
                break;

        }
    }

    void Raycast()
    {
        RaycastHit2D[] hits;
        Debug.DrawRay(pointRaycastEmit.position, -transform.right * detectionDistance, Color.red);

        hits = Physics2D.RaycastAll(pointRaycastEmit.position, -transform.right, detectionDistance);

        if (hits.Length > 0)
        {
            foreach(RaycastHit2D hit in hits)
            {
                
                if (hit.collider.tag == "Player")
                {

                    if (hit.collider.transform.position.x < transform.position.x) moveDirection = -1;
                    else moveDirection = 1;

                    behavior = Behavior.active;
                    BehaviorEnemy();
                    break;
                }
                else
                {
                    behavior = Behavior.notactive;
                    BehaviorEnemy();
                    //Debug.Log(hit.collider.name);
                    if (speed > 0)
                    {
                        if (hit.collider.transform.position.x > transform.position.x) moveDirection = -1;
                        else moveDirection = 1;
                    }

                }
            }
        }
        else
        {
            behavior = Behavior.notactive;
            BehaviorEnemy();
        }
    }

    void Move()
    {
        if(moveDirection < 0) transform.eulerAngles = new Vector2(0, 0);
        else transform.eulerAngles = new Vector2(0, 180);
        rb.velocity = Vector2.right * moveSpeed * moveDirection;
    }

    public void Damage(int dmg)
    {
        health -= dmg;
    }

    void ToDestroy()
    {
        if(itemObject != null)itemObject.SetActive(true);

        gameObject.SetActive(false);
    }
}
