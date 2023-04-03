using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Items : MonoBehaviour
{
    [SerializeField] ItemData itemData;
    int energy;

    private void Awake()
    {
        energy = itemData.score;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            Debug.Log("energy = " + energy);
            GameManager.energyItem += energy;
            Destroy(gameObject);
            
        }
    }
}
