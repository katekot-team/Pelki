using System.Collections.Generic;
using Assets.SimpleLocalization;
using UnityEngine;
using UnityEngine.UI;

namespace Pelki.UI.Screens
{
    public class MenuScreen : BaseScreen
    {
        [SerializeField] private List<GameObject> listWheelDecor = new List<GameObject>();
        [SerializeField] private float speedRotate;

        [Header("Language")]
        [NaughtyAttributes.Dropdown(nameof(GetAllLanguagesKeys))]
        [SerializeField] private string ruLanguageKey;

        [NaughtyAttributes.Dropdown(nameof(GetAllLanguagesKeys))]
        [SerializeField] private string enLanguageKey;

        [Header("Buttons")]
        [SerializeField] private Button enLocalizationButton;

        [SerializeField] private Button ruLocalizationButton;
        [SerializeField] private Button newGameButton;

        private IMain main;

        public void Construct(IMain main)
        {
            this.main = main;
        }

        private void Update()
        {
            WheelRotateTick();
        }

        private void OnEnable()
        {
            enLocalizationButton.onClick.AddListener(EnLocalizationButtonOnClick);
            ruLocalizationButton.onClick.AddListener(RuLocalizationButtonOnClick);
            newGameButton.onClick.AddListener(NewGameButtonOnClick);
        }

        private void OnDisable()
        {
            enLocalizationButton.onClick.RemoveListener(EnLocalizationButtonOnClick);
            ruLocalizationButton.onClick.RemoveListener(RuLocalizationButtonOnClick);
            newGameButton.onClick.RemoveListener(NewGameButtonOnClick);
        }

        private void RuLocalizationButtonOnClick()
        {
            LocalizationManager.Language = ruLanguageKey;
        }

        private void EnLocalizationButtonOnClick()
        {
            LocalizationManager.Language = enLanguageKey;
        }

        private void NewGameButtonOnClick()
        {
            main.StartGame();
        }

        private void WheelRotateTick()
        {
            foreach (GameObject wheel in listWheelDecor)
            {
                wheel.transform.Rotate(0, 0, speedRotate);
                speedRotate *= -1;
            }
        }

        private IReadOnlyList<string> GetAllLanguagesKeys()
        {
            IReadOnlyList<string> result = LocalizationManager.GetLanguagesKeys();
            return result;
        }
    }
}