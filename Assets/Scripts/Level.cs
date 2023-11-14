using System;
using System.Collections.Generic;
using System.Linq;
using NaughtyAttributes;
using Pelki.Gameplay.SaveSystem;
using UnityEngine;

namespace Pelki
{
    public class Level : MonoBehaviour, ISerializationCallbackReceiver
    {
        [SerializeField] private Transform characterSpawnPoint;
        [SerializeField] private List<SavePointDto> savePoints;
        
        [Dropdown(nameof(GetAllSavepointIds))]
        [SerializeField] private string characterSpawnSavePointId;

        private Dictionary<SavePoint, string> savePointsRegister;

        public IReadOnlyDictionary<SavePoint, string> SavePointsRegister => savePointsRegister;

        public Vector3 CharacterSpawnPosition => characterSpawnPoint.position;

        public string CharacterSpawnSavePointId => characterSpawnSavePointId;
        
        public IEnumerable<string> GetAllSavepointIds()
        {
            IEnumerable<string> result = SavePointsRegister.Values.ToList();
            return result;
        }

        public void OnBeforeSerialize()
        {
            //do nothing
        }

        public void OnAfterDeserialize()
        {
            savePointsRegister = savePoints.ToDictionary(dto => dto.SavePoint, dto => dto.ID);
        }

        [Serializable]
        private class SavePointDto
        {
            [SerializeField] private SavePoint savePoint;
            [SerializeField] private string id;

            public SavePoint SavePoint => savePoint;
            public string ID => id;
        }
    }
}