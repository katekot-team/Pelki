using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using Newtonsoft.Json;
using UnityEngine;

namespace Pelki.Gameplay.SaveSystem
{
    public class GameProgressStorage : IDisposable, IGameProgressSaver
    {
        private const string LEVEL_SESSION = nameof(LEVEL_SESSION);

        private static readonly Dictionary<Type, string> levelProgressKeys = new Dictionary<Type, string>()
        {
            { typeof(LevelProgress), LEVEL_SESSION }
        };

        public bool TryLoadGameProgress<TProgress>(out TProgress progress) where TProgress : BaseProgress
        {
            if (
                levelProgressKeys.ContainsKey(typeof(TProgress)) 
                && PlayerPrefs.HasKey(levelProgressKeys[typeof(TProgress)])
            )
            {
                string levelProressInJson = PlayerPrefs.GetString(levelProgressKeys[typeof(TProgress)]);
                progress = JsonConvert.DeserializeObject<TProgress>(levelProressInJson);
                progress.Initialize(this);
                
                return true;
            }

            progress = null;

            return false;
        }

        public void SaveGameProgress<TProgress>(TProgress progress) where TProgress : BaseProgress
        {
            var key = levelProgressKeys[typeof(TProgress)];
            string levelProgessInJson = JsonConvert.SerializeObject(progress);
            PlayerPrefs.SetString(key, levelProgessInJson);
        }

        public void Dispose()
        {
            PlayerPrefs.Save();
        }
    }
}