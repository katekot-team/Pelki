using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    [SerializeField] List<GameObject> listWheelDecor = new List<GameObject>();
    [SerializeField] float speedRotate;
    [SerializeField] string sceneName;

    AsyncOperation asyncOperation;

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

    public void LoadScene()
    {
        StartCoroutine(LoaderScene(sceneName));
    }

    IEnumerator LoaderScene(string sceneName)
    {
        yield return null;
        asyncOperation = SceneManager.LoadSceneAsync(sceneName);
    }
}
