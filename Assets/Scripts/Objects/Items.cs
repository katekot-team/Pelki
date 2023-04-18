using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Items : MonoBehaviour
{
    [SerializeField] ItemData itemData;
    ItemType type;
    int score;

    private void Awake()
    {
        score = itemData.score;
        type = itemData.type;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            switch (type)
            {
                case ItemType.largeEnergyItem:
                case ItemType.smallEnergyItem:
                    GameManager.energy += score;
                    break;
                case ItemType.largeHealthItem:
                case ItemType.smallHealthItem:
                    GameManager.health += score;
                    break;
            }
            Debug.Log(type + " = " + score);
            Destroy(gameObject);
            


            
        }
    }
}
