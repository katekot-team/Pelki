using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PuzzleController : MonoBehaviour
{
    [SerializeField]float speedRotate;

    private void OnMouseDrag()
    {
        transform.Rotate(0, 0, Input.GetAxis("Mouse X") * speedRotate);
        transform.Rotate(0, 0, Input.GetAxis("Mouse Y") * speedRotate);
        Debug.Log(name);
    }
}
