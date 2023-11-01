using System;
using System.Collections.Generic;
using UnityEngine;

namespace Pelki.Gameplay.SaveSystem
{
    public class SavesStorage : IDisposable, IGameProgressSaver
    {
        private const string LEVEL_SESSION = nameof(LEVEL_SESSION);

        private static readonly Dictionary<Type, string> saveObjectsKeys = new Dictionary<Type, string>()
        {
            { typeof(LevelSessionData), LEVEL_SESSION }
        };

        private LevelSessionData _loadSaveData;

        public LevelSessionData LoadSaveData => _loadSaveData;

        public void LoadSaves()
        {
            _loadSaveData = LoadingGameProgress<LevelSessionData>(LEVEL_SESSION);

            _loadSaveData.Initialize(this);
            /*_loadSaveData.Saved += SaveObject;
            _loadSaveData.Initialize(SaveObject);*/
        }

        private void LoadSaveDataOnSaved(object obj)
        {
            SaveObject(obj);
        }

        public void SaveObject<TObj>(TObj t)
        {
            //sttrox: получение ключа по типа
            //var key = saveObjectsKeys[typeof(TObj)];
            //sttrox: есть предположение что это не сработает, хотя проверяли в интерактивнос шарпе
            //var key = saveObjectsKeys[t.GetType()];
            //сохранение под ключем
            // попробовать реализовать на словаре
            var key = saveObjectsKeys[typeof(TObj)];
            SavingGameProgress(t, key);
        }

        private TSaveData LoadingGameProgress<TSaveData>(string saveDataKey)
        {
            //todo: sttrox: реализовать загрузку
            //десериализация из json(от Unity) и потом кастинг(?)
            // string playerName = PlayerPrefs.GetString(saveDataKey, "Unknown");
            throw new NotImplementedException();
        }

        private void SavingGameProgress<TSaveData>(TSaveData saveData, string key)
        {
            //todo: sttrox: реализовать сохранение
            //сериадизация в json(от Unity) и потом кастинг(?)
            //PlayerPrefs.SetString(key, "John Doe");
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            PlayerPrefs.Save();
        }
    }
}