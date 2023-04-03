using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyObject : MonoBehaviour
{
    [SerializeField] EnemyData enemyData;
    [SerializeField] GameObject item;

    GameObject itemObject;
    int health;

    private void Start()
    {
        health = enemyData.health;

        if (item != null)
        {
            itemObject = Instantiate(item, transform.position, Quaternion.identity);
            itemObject.SetActive(false);
        }
    }

    public void Destroy(int dmg)
    {
        health-=dmg;
        if(health <= 0)
        {
            if (item != null) itemObject.SetActive(true);
            gameObject.SetActive(false);
        }

    }
}
