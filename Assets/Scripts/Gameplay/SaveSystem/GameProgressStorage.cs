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
            { typeof(BaseProgress), LEVEL_SESSION }
        };

        /*private BaseProgress levelBaseProgress;

        public BaseProgress LevelBaseProgress => levelBaseProgress;*/

        public bool TryLoadGameProgress<TProgress>(out TProgress progress) where TProgress : BaseProgress
        {
            if (PlayerPrefs.HasKey(LEVEL_SESSION))
            {
                string levelProressInJson = PlayerPrefs.GetString(LEVEL_SESSION, "Not saved");
                progress = JsonConvert.DeserializeObject<TProgress>(levelProressInJson);
                progress.Initialize(this);
                
                return true;
            }

            progress = null;

            return false;
        }

        /*public void LoadGameProgress()
        {
            levelBaseProgress = DoLoadingGameProgress(LEVEL_SESSION);

            levelBaseProgress.Initialize(this);
        }*/

        public void SaveGameProgress<TProgress>(TProgress progress) where TProgress : BaseProgress
        {
            var key = levelProgressKeys[typeof(BaseProgress)];
            string levelProgessInJson = JsonConvert.SerializeObject(progress);
            PlayerPrefs.SetString(key, levelProgessInJson);
        }

        /*private BaseProgress DoLoadingGameProgress(string levelProressKey)
        {
            string levelProressInJson = PlayerPrefs.GetString(levelProressKey, "Not saved");

            return JsonConvert.DeserializeObject<LevelBaseProgress>(levelProressInJson);
        }*/

        public void Dispose()
        {
            PlayerPrefs.Save();
        }
    }
}