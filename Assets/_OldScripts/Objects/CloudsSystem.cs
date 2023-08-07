using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloudsSystem : MonoBehaviour
{
    [SerializeField] List<GameObject> clouds = new List<GameObject>();
    [SerializeField] float timeDelay;

    void Start()
    {
        StartCoroutine(Disappearance());
    }

    IEnumerator Disappearance()
    {
        while (true)
        {
            for(int i = 0; i < clouds.Count; i++)
            {
                clouds[clouds.Count-1].SetActive(true);
                clouds[i].SetActive(false);
                if (i > 0) clouds[i - 1].SetActive(true);
                yield return new WaitForSeconds(timeDelay);
            }
        }
    }
}
