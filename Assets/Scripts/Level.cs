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

        private Dictionary<SavePoint, string> savePointIdsRegister;
        private Dictionary<string, SavePoint> savePointsRegister;

        public IReadOnlyDictionary<SavePoint, string> SavePointIdsRegister => savePointIdsRegister;
        public IReadOnlyDictionary<string, SavePoint> SavePointsRegister => savePointsRegister;

        public Vector3 CharacterSpawnPosition => characterSpawnPoint.position;

        public string CharacterSpawnSavePointId => characterSpawnSavePointId;
        
        public IEnumerable<string> GetAllSavepointIds()
        {
            IEnumerable<string> result = SavePointIdsRegister.Values.ToList();
            return result;
        }

        void ISerializationCallbackReceiver.OnBeforeSerialize()
        {
            //do nothing
        }

        void ISerializationCallbackReceiver.OnAfterDeserialize()
        {
            savePointIdsRegister = savePoints.ToDictionary(dto => dto.SavePoint, dto => dto.ID);
            savePointsRegister = savePoints.ToDictionary(dto => dto.ID, dto => dto.SavePoint);
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