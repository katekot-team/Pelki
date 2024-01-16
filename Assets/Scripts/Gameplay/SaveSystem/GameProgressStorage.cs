using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Pelki.Gameplay.InventorySystem;
using UnityEngine;

namespace Pelki.Gameplay.SaveSystem
{
    public class GameProgressStorage : IDisposable, IGameProgressSaver
    {
        private const string LEVEL_SESSION = nameof(LEVEL_SESSION);
        private const string LEVEL_INVENTORY = nameof(LEVEL_INVENTORY);

        private static readonly Dictionary<Type, string> levelProgressKeys = new Dictionary<Type, string>()
        {
            { typeof(LevelProgress), LEVEL_SESSION },
            { typeof(InventoryProgress), LEVEL_INVENTORY }
        };

        public bool TryLoadGameProgress<TProgress>(out TProgress progress) where TProgress : BaseProgress<TProgress>
        {
            if (levelProgressKeys.TryGetValue(typeof(TProgress), out string levelProgressKey))
            {
                if (PlayerPrefs.HasKey(levelProgressKey))
                {
                    string levelProgressInJson = PlayerPrefs.GetString(levelProgressKey);
                    progress = JsonConvert.DeserializeObject<TProgress>(levelProgressInJson);
                    progress.Initialize(this);

                    return true;
                }
            }
            else
            {
                Debug.LogError(
                    $"[{nameof(GameProgressStorage)}]:[{nameof(TryLoadGameProgress)}()] Type" +
                    $"'{typeof(TProgress).FullName}' not founded in {levelProgressKeys}");
            }

            progress = null;

            return false;
        }

        public void SaveGameProgress<TProgress>(TProgress progress) where TProgress : BaseProgress<TProgress>
        {
            var key = levelProgressKeys[typeof(TProgress)];
            string levelProgressInJson = JsonConvert.SerializeObject(progress);
            PlayerPrefs.SetString(key, levelProgressInJson);
        }

        public void Dispose()
        {
            PlayerPrefs.Save();
        }
    }
}