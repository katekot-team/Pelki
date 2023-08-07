using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    //�������� ��� ������ ����
    [SerializeField] int startEnergyValue;
    [SerializeField] int startHealthValue;
    
    public static int energy;
    public static int health;
    public static bool haveCompanion;

    private void Start()
    {
        health = startHealthValue;
        energy = startEnergyValue;
    }

    public static void OnApplicationPause(bool pause)
    {
        Time.timeScale = pause ? 0 : 1;
    }

    public static void OnLevelReplay()
    {
        SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex);
        OnApplicationPause(false);
    }
}
