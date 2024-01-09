using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Pelki.Gameplay.SaveSystem;
using UnityEngine;

namespace Pelki.Gameplay.InventorySystem
{
    public class InventoryStorage : IDisposable, IGameProgressSaver
    {
        private const string LEVEL_INVENTORY = nameof(LEVEL_INVENTORY);

        private static readonly Dictionary<Type, string> levelInventoryKeys = new Dictionary<Type, string>()
        {
            { typeof(Inventory), LEVEL_INVENTORY }
        };
        
        public bool TryLoadGameProgress<TProgress>(out TProgress progress) where TProgress : BaseProgress<TProgress>
        {
            if (levelInventoryKeys.TryGetValue(typeof(TProgress), out string levelProgressKey))
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
                    $"[{nameof(InventoryStorage)}]:[{nameof(TryLoadGameProgress)}()] Type" +
                    $"'{typeof(TProgress).FullName}' not founded in {levelInventoryKeys}");
            }

            progress = null;

            return false;
        }
        
        public void SaveGameProgress<TProgress>(TProgress progress) where TProgress : BaseProgress<TProgress>
        {
            var key = levelInventoryKeys[typeof(TProgress)];
            string levelProgressInJson = JsonConvert.SerializeObject(progress);
            PlayerPrefs.SetString(key, levelProgressInJson);
        }

        public void Dispose()
        {
            PlayerPrefs.Save();
        }
    }
}