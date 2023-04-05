using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBall : MonoBehaviour
{
    [SerializeField] float speed;
    [SerializeField] float lifeTime;
    Rigidbody2D rb;
    IEnumerator timeCount;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        timeCount = TimeCount();
        StartCoroutine(timeCount);

        rb.velocity = transform.right * speed;
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            int direction = 1;
            if (collision.transform.position.x < transform.position.x) direction = -1;
            collision.gameObject.GetComponent<PlayerManager>().Damage(direction);
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
