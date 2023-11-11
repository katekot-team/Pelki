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
            { typeof(Progress), LEVEL_SESSION }
        };

        private Progress _levelProgress;

        public Progress LevelProgress => _levelProgress;

        public bool TryLoadGameProgress()
        {
            if (PlayerPrefs.HasKey(LEVEL_SESSION))
            {
                LoadGameProgress();
                
                return true;
            }

            return false;
        }

        public void LoadGameProgress()
        {
            _levelProgress = DoLoadingGameProgress(LEVEL_SESSION);

            _levelProgress.Initialize(this);
        }

        public void SaveGameProgress(Progress progress)
        {
            var key = levelProgressKeys[typeof(Progress)];
            DoSavingGameProgress(progress, key);
        }

        private Progress DoLoadingGameProgress(string levelProressKey)
        {
            string levelProressInJson = PlayerPrefs.GetString(levelProressKey, "Not saved");

            return JsonConvert.DeserializeObject<LevelProgress>(levelProressInJson);
        }

        private void DoSavingGameProgress(Progress levelProgess, string key)
        {
            string levelProgessInJson = JsonConvert.SerializeObject(levelProgess);
            PlayerPrefs.SetString(key, levelProgessInJson);
        }

        public void Dispose()
        {
            PlayerPrefs.Save();
        }
    }
}