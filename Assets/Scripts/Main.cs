using Pelki.Configs;
using Pelki.Gameplay;
using Pelki.Gameplay.Input;
using Pelki.UI;
using Pelki.UI.Screens;
using UnityEngine;

namespace Pelki
{
    public class Main : MonoBehaviour, IMain
    {
        [SerializeField] private MainSettingsConfig mainSettingsConfig;
        [SerializeField] private ScreenSwitcher screenSwitcher;

        private Game game;
        private IInput input;

        private void Awake()
        {
#if UNITY_EDITOR
            input = new InputBySimpleInputAndKeyboard(mainSettingsConfig.InputConfig);
#else
            input = new InputBySimpleInput(mainSettingsConfig.InputConfig);
#endif
        }

        private void Start()
        {
            var menuScreen = screenSwitcher.ShowScreen<MenuScreen>();
            menuScreen.Construct((IMain)this);

            game = new Game(mainSettingsConfig.LevelsConfig, mainSettingsConfig.CharactersConfig, screenSwitcher,
                input);

            //SichTM: temporal addition until game menu will be done
            StartGame();
        }

        private void Update()
        {
            game.ThisUpdate();
        }

        public void StartGame()
        {
            game.StartGame();
        }
    }
}