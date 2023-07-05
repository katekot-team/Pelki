using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class Enemy : MonoBehaviour
{
    [SerializeField] EnemyData enemyData;
    [SerializeField] Behavior behavior;
    [SerializeField] GameObject item;
    [SerializeField] Transform pointRaycastEmit;
    [SerializeField] Transform pointSpawnItem;
    
    GameObject itemObject;
    Rigidbody2D rb;

    float detectionDistance;
    int health;
    float speed;
    float speedAttack;
    float moveSpeed;

    public int damage { get; private set; }
    public int moveDirection { get; set; }
    public bool isAttack { get; set; }
    
    enum Behavior
    {
        active,
        notactive,
    }

    private void Awake()
    {
        health = enemyData.health;
        detectionDistance = enemyData.detectionDistance;
        speed = enemyData.speed;
        speedAttack = enemyData.speedAttack;
        damage = enemyData.damage;
        moveDirection = -1;

        rb = GetComponent<Rigidbody2D>();

        isAttack = false;
    }

    private void Start()
    {


        if (item != null)
        {
            itemObject = Instantiate(item, new Vector2(transform.position.x, transform.position.y + 3), Quaternion.identity);
            itemObject.SetActive(false);
        }

        BehaviorEnemy();
    }


    private void FixedUpdate()
    {
        Move();
        Raycast();

        
    }


    private void BehaviorEnemy()
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

    private void Raycast()
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

    private void Move()
    {
        if(moveDirection < 0) transform.eulerAngles = new Vector2(0, 0);
        else transform.eulerAngles = new Vector2(0, 180);
        if(rb != null)rb.velocity = Vector2.right * moveSpeed * moveDirection;
    }

    public void Damage(int dmg)
    {       
        health -= dmg;
        if (health <= 0) ToDestroy();
    }

    private void ToDestroy()
    {
        if (itemObject != null)
        {
            GameObject itemObj = Instantiate(itemObject, pointSpawnItem.position, Quaternion.identity);
            itemObj.transform.position = transform.position;
            itemObj.SetActive(true);
        }
        gameObject.SetActive(false);
    }


}
