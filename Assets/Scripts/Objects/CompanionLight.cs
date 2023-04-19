using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CompanionLight : MonoBehaviour
{
    [SerializeField] float speed;
    Transform target;

    private void Start()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            if(target == null)target = collision.gameObject.GetComponent<PlayerManager>().GetPointCompanion();
            
            
        }
    }

    private void Update()
    {
        Move();
    }

    void Move()
    {
        if (target != null)
        {
            transform.position = Vector2.Lerp(transform.position, target.transform.position, speed * Time.deltaTime);
        }
    }
}
