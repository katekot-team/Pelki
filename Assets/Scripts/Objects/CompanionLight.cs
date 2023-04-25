using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CompanionLight : MonoBehaviour
{
    [SerializeField] float speed;
    Transform target;
    PlayerManager playerManager;

    private void Start()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            playerManager = collision.gameObject.GetComponent<PlayerManager>();
            if (target == null)target = playerManager.GetPointCompanion();
            playerManager.TakeACompanion();
            GetComponent<BoxCollider2D>().enabled = false;
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
