using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitPoint : MonoBehaviour
{
    
    int damageWeak;

    private void Start()
    {
        PlayerManager playerManager = transform.parent.GetComponent<PlayerManager>();
        
        damageWeak = playerManager.GetDamagePowerWeak();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<DestroyObject>() != null)
        {
            collision.gameObject.GetComponent<DestroyObject>().Destroy(damageWeak);
        }
        if(collision.gameObject.GetComponent<Enemy>() != null)
        {
            collision.gameObject.GetComponent<Enemy>().Damage(damageWeak);
        }
    }

}