using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fish : MonoBehaviour
{
    Enemy enemy;
    int damage;

    void Start()
    {
        enemy = GetComponent<Enemy>();
        damage = enemy.damage;
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            int direction = 1;

            if (collision.transform.position.x > transform.position.x)
            {               
                if (enemy.moveDirection < 0)
                {
                    enemy.moveDirection = 1;

                }
            }
            else
            {
                direction = -1;
                if (enemy.moveDirection > 0)
                {
                    enemy.moveDirection = -1;
                }
            }
            if(enemy.isAttack)collision.gameObject.GetComponent<PlayerManager>().Damage(direction, damage);

        }
    }
}
