using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallenStep : MonoBehaviour
{
    [SerializeField] float timeDelayFallen = 1;
    [SerializeField] float timeDelayRecovery = 3;


    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            Invoke("Deactive", timeDelayFallen);
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            Invoke("Active", timeDelayRecovery);
        }
    }


    void Active()
    {
        gameObject.SetActive(true);
    }

    void Deactive()
    {
        gameObject.SetActive(false);
    }

}
