using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] Image healthBar;
    [SerializeField] Image energyBar;
    [SerializeField] Image panelPause;
    [SerializeField] Image panelGameOver;

    bool pause;

    private void Start()
    {
        pause = false;
    }

    private void FixedUpdate()
    {
        EnergyDisplay();
        HealthDisplay();
        if (GameManager.health <= 0) GameOver();
    }

    private void EnergyDisplay()
    {
        if (GameManager.haveCompanion) 
        {
            energyBar.gameObject.SetActive(true);
            energyBar.fillAmount = (float)GameManager.energy / 10;
        }
        else
        {
            energyBar.gameObject.SetActive(false);
        }
    }

    private void HealthDisplay()
    {
        healthBar.fillAmount = (float)GameManager.health / 10;
    }

    public void Pause()
    {
        pause = pause ? false : true;
        panelPause.gameObject.SetActive(pause);
        
        GameManager.OnApplicationPause(pause);
    }

    public void Replay()
    {
        GameManager.OnLevelReplay();
    }

    private void GameOver()
    {
        GameManager.OnApplicationPause(true);
        panelGameOver.gameObject.SetActive(true);
    }
}
