using System;
using System.Collections.Generic;
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

        private LevelProgress _levelProgress;

        public LevelProgress LevelProgress => _levelProgress;

        public void LoadGameProgress()
        {
            LevelProgress defaultLevelProgress = new LevelProgress("Initial savepoint");
            _levelProgress = DoLoadingGameProgress<LevelProgress>(LEVEL_SESSION, defaultLevelProgress);

            _levelProgress.Initialize(this);
        }

        public void SaveGameProgress<TObj>(TObj t)
        {
            var key = levelProgressKeys[typeof(TObj)];
            DoSavingGameProgress(t, key);
        }

        private TProgress DoLoadingGameProgress<TProgress>(string levelProressKey, 
            LevelProgress defaultLevelProgress)
        {
            string levelProressInJson = PlayerPrefs.GetString(levelProressKey, "Not saved");
            if (levelProressInJson == "Not saved")
            {

                return (TProgress)(object)defaultLevelProgress;
            }

            return JsonConvert.DeserializeObject<TProgress>(levelProressInJson);
        }

        private void DoSavingGameProgress<TProgress>(TProgress levelProgess, string key)
        {
            LevelProgress progress = (LevelProgress)Convert.ChangeType(levelProgess, typeof(LevelProgress));
            string levelProgessInJson = JsonConvert.SerializeObject(progress);
            PlayerPrefs.SetString(key, levelProgessInJson);
        }

        public void Dispose()
        {
            PlayerPrefs.Save();
        }
    }
}