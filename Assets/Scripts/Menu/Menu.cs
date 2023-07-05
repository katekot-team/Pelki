using System.Collections;
using System.Collections.Generic;
using Assets.SimpleLocalization;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Menu : MonoBehaviour
{
    [SerializeField] List<GameObject> listWheelDecor = new List<GameObject>();
    [SerializeField] float speedRotate;
    [SerializeField] string sceneName;

    [Header("Language")]
    [NaughtyAttributes.Dropdown(nameof(GetAllLanguagesKeys))]
    [SerializeField] string ruLanguageKey;

    [NaughtyAttributes.Dropdown(nameof(GetAllLanguagesKeys))]
    [SerializeField] string enLanguageKey;

    [Header("Buttons")]
    [SerializeField] Button enLocalizationButton;

    [SerializeField] Button ruLocalizationButton;

    private void Update()
    {
        WheelRotateTick();
    }

    private void OnEnable()
    {
        enLocalizationButton.onClick.AddListener(EnLocalizationButtonOnClick);
        ruLocalizationButton.onClick.AddListener(RuLocalizationButtonOnClick);
    }

    private void OnDisable()
    {
        enLocalizationButton.onClick.RemoveListener(EnLocalizationButtonOnClick);
        ruLocalizationButton.onClick.RemoveListener(RuLocalizationButtonOnClick);
    }

    private void RuLocalizationButtonOnClick()
    {
        LocalizationManager.Language = ruLanguageKey;
    }

    private void EnLocalizationButtonOnClick()
    {
        LocalizationManager.Language = enLanguageKey;
    }

    void WheelRotateTick()
    {
        foreach (GameObject wheel in listWheelDecor)
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
        AsyncOperation _ = SceneManager.LoadSceneAsync(sceneName);
    }

    private IReadOnlyList<string> GetAllLanguagesKeys()
    {
        IReadOnlyList<string> result = LocalizationManager.GetLanguagesKeys();
        return result;
    }
}