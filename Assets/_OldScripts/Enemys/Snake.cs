using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Snake : MonoBehaviour
{
    [SerializeField] GameObject fireball;
    [SerializeField] float shotFrequency;
    [SerializeField] Transform shootPoint;

    Enemy enemy;
    IEnumerator fire;

    void Start()
    {
        enemy = GetComponent<Enemy>();

        fire = Fire();
        StartCoroutine(fire);

    }

    IEnumerator Fire()
    {
        while (true)
        {    
            if (enemy.isAttack) 
            {
                Instantiate(fireball, shootPoint.position, shootPoint.rotation);
                yield return new WaitForSeconds(shotFrequency);
            }
            yield return new WaitForFixedUpdate();

        }
        
    }


}
