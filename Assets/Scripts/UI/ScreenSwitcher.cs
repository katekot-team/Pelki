using System;
using System.Collections.Generic;
using UnityEngine;

namespace Pelki.UI
{
    public class ScreenSwitcher : MonoBehaviour
    {
        [SerializeField] private List<BaseScreen> screensPrefabs = new List<BaseScreen>();

        private readonly Dictionary<Type, BaseScreen> screenCache = new Dictionary<Type, BaseScreen>();

        private BaseScreen currentScreen;

        public void ShowScreen<TScreen>() where TScreen : BaseScreen
        {
            Type screenType = typeof(TScreen);

            if (screenCache.TryGetValue(screenType, out var foundScreen))
            {
                currentScreen.Hide();
                currentScreen = foundScreen;
                currentScreen.Show();
            }
            else
            {
                //sttrox: оптимизировать, лучше в какой словарь закинуть все префабы
                BaseScreen prefab = screensPrefabs.Find(x => x.GetType().FullName == screenType.FullName);
                if (prefab != null)
                {
                    BaseScreen newScreen = Instantiate(prefab, transform);
                    screenCache[screenType] = newScreen;
                    currentScreen.Hide();
                    currentScreen = newScreen;
                    currentScreen.Show();
                }
                else
                {
                    Debug.LogError($"Screen prefab not found for type {screenType}");
                }
            }
        }

        private void HideCurrentScreen()
        {
            if (currentScreen != null)
            {
                currentScreen.Hide();
                currentScreen = null;
            }
        }
    }
}