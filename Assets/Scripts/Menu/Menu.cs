using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Menu : MonoBehaviour
{
    [SerializeField] List<GameObject> listWheelDecor = new List<GameObject>();
    [SerializeField] float speedRotate;
    void FixedUpdate()
    {
        WheelRotate();
    }

    void WheelRotate()
    {
        foreach(GameObject wheel in listWheelDecor)
        {
            wheel.transform.Rotate(0, 0, speedRotate);
            speedRotate *= -1;
        }
    }
}
