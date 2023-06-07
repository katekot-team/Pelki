using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PuzzleController : MonoBehaviour
{
    [SerializeField] float speedRotate;
    [SerializeField] GameObject[] puzzleElements;
    [SerializeField] GameObject[] activateObjects;
    [SerializeField] GameObject[] deactivateObjects;

    [SerializeField] bool puzzleActive = false;

    private void Start()
    {
        SetReward(false, activateObjects);

        foreach (var puzzleElement in puzzleElements)
        {
            PuzzleRotate puzzleRotate = puzzleElement.GetComponent<PuzzleRotate>();
            puzzleRotate.speedRotate = speedRotate;
            puzzleElement.transform.Rotate(0, 0, Random.Range(0, 360));
        }
    }

    private void Update()
    {
        if(puzzleActive)CheckSolution();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player") puzzleActive = true;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player") puzzleActive = false;
    }

    void CheckSolution()
    {
        int countSolution = 0;

        foreach(var puzzleElement in puzzleElements)
        {
            PuzzleRotate puzzleRotate = puzzleElement.GetComponent<PuzzleRotate>();
            if (puzzleRotate.solution == false)
            {
                countSolution = 0;
                break;
            }
            
            countSolution++;
        }

        if(countSolution >= puzzleElements.Length)
        {
            SetReward(false, deactivateObjects);
            SetReward(true, activateObjects);
            activateObjects = null;
        }
    }

    void SetReward(bool b, GameObject[] objects)
    {
        if(objects != null)
        {
            foreach (var obj in objects)
            {
                obj.SetActive(b);
            }
        }

    }
}
