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

    void Start()
    {
        pause = false;
    }

    void FixedUpdate()
    {
        energyBar.fillAmount = (float)GameManager.energyItem/10;
        healthBar.fillAmount = (float)GameManager.health / 10;
        if (GameManager.health <= 0) GameOver();
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

    void GameOver()
    {
        GameManager.OnApplicationPause(true);
        panelGameOver.gameObject.SetActive(true);
    }
}
