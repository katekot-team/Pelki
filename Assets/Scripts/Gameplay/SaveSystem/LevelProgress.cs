using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;

namespace Pelki.Gameplay.SaveSystem
{
    public class LevelProgress : BaseProgress<LevelProgress>
    {
        private readonly List<string> _activatedSavePoints = new List<string>();

        [JsonProperty] public string LastSavePointId { get; private set; }

        public IReadOnlyList<string> ActivatedSavePoints => _activatedSavePoints;

        public void AddActivatedSavePoint(string savePointId)
        {
            _activatedSavePoints.Add(savePointId);
            LastSavePointId = savePointId;
        }

        public class Factory
        {
            private GameProgressStorage _gameProgressStorage;

            public Factory(GameProgressStorage gameProgressStorage)
            {
                _gameProgressStorage = gameProgressStorage;
            }

            public LevelProgress Create(Level currentLevel)
            {
                LevelProgress levelProgress;
                if (_gameProgressStorage.TryLoadGameProgress(out levelProgress) == false)
                {
                    //ещё хорошо бы попробовать сделать конструктор LevelProgress() приватным, что бы никто не могу его 
                    //создать кроме этой фабрики, но не знаю справится сериализатор или нет. попробуешь?
                    levelProgress = new LevelProgress();
                    levelProgress.Initialize(_gameProgressStorage);
                    levelProgress.AddActivatedSavePoint(currentLevel.CharacterSpawnSavePointId);
                    levelProgress.Save();
                }
                else
                {
                    if (!currentLevel.SavePointsRegister.ContainsKey(levelProgress.LastSavePointId))
                    {
                        Debug.LogError("Last save point not found, we be teleport on spawn point. The game continues");
                        levelProgress.Initialize(_gameProgressStorage);
                        levelProgress.AddActivatedSavePoint(currentLevel.CharacterSpawnSavePointId);
                        levelProgress.Save();
                    }
                }

                return levelProgress;
            }
        }
    }
}