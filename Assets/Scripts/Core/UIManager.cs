using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] Image energyBar;


    void Start()
    {
        
    }

    void FixedUpdate()
    {
        energyBar.fillAmount = (float)GameManager.energyItem/10;
    }
}
