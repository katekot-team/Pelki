using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class Enemy : MonoBehaviour
{
    [SerializeField] EnemyData enemyData;
    [SerializeField] Behavior behavior;
    [SerializeField] GameObject item;

    GameObject itemObject;
    float detectionDistance;
    int health;
    float speed;
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
        Raycast(1);
        Raycast(-1);

        if (health <= 0) ToDestroy();
    }


    void BehaviorEnemy()
    {
        switch (behavior)
        {
            case Behavior.notactive:
                isAttack = false;
                break;
            case Behavior.active:
                isAttack = true;

                break;

        }
    }

    void Raycast(int direction)
    {
        RaycastHit2D[] hits;
        //Debug.DrawRay(transform.position, direction * transform.right * detectionDistance, Color.red);

        hits = Physics2D.RaycastAll(transform.position, transform.right * direction, detectionDistance);

        if (hits.Length > 0)
        {
            foreach(RaycastHit2D hit in hits)
            {
                if (hit.collider.tag == "Player")
                {
                    if (hit.collider.transform.position.x < transform.position.x) transform.eulerAngles = new Vector2(0, 0);
                    else transform.eulerAngles = new Vector2(0, 180);
                    if (!isAttack)
                    {

                        behavior = Behavior.active;
                        BehaviorEnemy();
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
