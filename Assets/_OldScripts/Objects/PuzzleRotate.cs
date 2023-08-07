using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleRotate : MonoBehaviour
{
    public float speedRotate{get; set;}
    public bool solution { get; private set;}

    private void OnMouseDrag()
    {

        if (Mathf.Round(transform.eulerAngles.z) == 0)
        {
            //Debug.Log(name + " true!!! " + transform.eulerAngles.z);
            solution = true;
        }
        else
        {
            transform.Rotate(0, 0, Input.GetAxis("Mouse X") * speedRotate);
            transform.Rotate(0, 0, Input.GetAxis("Mouse Y") * speedRotate);
        }

    }
}
