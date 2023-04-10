using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBall : MonoBehaviour
{
    [SerializeField] FirebollData fireballData;
    string toDamage;
    int damage;
    float speed;
    float lifeTime;

    Rigidbody2D rb;
    IEnumerator timeCount;



    private void Start()
    {
        toDamage = fireballData.toDamage.ToString();
        damage = fireballData.damage;
        speed = fireballData.speed;
        lifeTime = fireballData.lifeTime;

        rb = GetComponent<Rigidbody2D>();
        timeCount = TimeCount();
        StartCoroutine(timeCount);

        rb.velocity = transform.right * speed;
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == toDamage)
        {
            int direction = 1;
            if (collision.transform.position.x < transform.position.x) direction = -1;

            if (toDamage == "Player")
            {
                if (collision.gameObject.GetComponent<PlayerManager>() != null) collision.gameObject.GetComponent<PlayerManager>().Damage(direction);
            }
            if (toDamage == "Enemy") 
            {
                if(collision.gameObject.GetComponent<Enemy>() != null) collision.gameObject.GetComponent<Enemy>().Damage(damage);
                if (collision.gameObject.GetComponent<DestroyObject>() != null) collision.gameObject.GetComponent<DestroyObject>().Destroy(damage);
            } 
            
        }
        
        Destroy(gameObject);
        //StopCoroutine(timeCount);
    }

    IEnumerator TimeCount()
    {
        yield return new WaitForSeconds(lifeTime);
        
        StopCoroutine(timeCount);
        Destroy(gameObject);
    }
}
