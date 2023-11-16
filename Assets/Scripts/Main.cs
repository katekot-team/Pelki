using Pelki.Configs;
using Pelki.Gameplay;
using Pelki.Gameplay.Input;
using Pelki.Gameplay.SaveSystem;
using Pelki.UI;
using Pelki.UI.Screens;
using UnityEngine;

namespace Pelki
{
    public class Main : MonoBehaviour, IMain
    {
        [SerializeField] private MainSettingsConfig mainSettingsConfig;
        [SerializeField] private ScreenSwitcher screenSwitcher;
        
        private readonly GameProgressStorage gameProgressStorage = new GameProgressStorage();

        private Game game;
        private IInput input;
        private LevelProgress levelProgress;

        private void Awake()
        {
#if UNITY_EDITOR
            input = new InputBySimpleInputAndKeyboard(mainSettingsConfig.InputConfig);
#else
            input = new InputBySimpleInput(mainSettingsConfig.InputConfig);
#endif

            if (!gameProgressStorage.TryLoadGameProgress(out levelProgress))
            {
                Level level = mainSettingsConfig.LevelsConfig.DebugLevelPrefab;
                levelProgress = new LevelProgress(level.CharacterSpawnSavePointId);
                levelProgress.Initialize(gameProgressStorage);
                levelProgress.Save();
            }
        }

        private void Start()
        {
            var menuScreen = screenSwitcher.ShowScreen<MenuScreen>();
            menuScreen.Construct((IMain)this);

            game = new Game(mainSettingsConfig.LevelsConfig, mainSettingsConfig.CharactersConfig, screenSwitcher,
                input, levelProgress);

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