using System;
using System.Collections.Generic;
using Pelki.Gameplay.SaveSystem;
using UnityEngine;

namespace Pelki
{
    public class Level : MonoBehaviour
    {
        [SerializeField] private Transform characterSpawnPoint;
        [SerializeField] private List<SavePoint> savePoints;
        
        public List<SavePoint> SavePoints => savePoints;
        private Dictionary<SavePoint, string> savePointsRegister;

        public IReadOnlyDictionary<SavePoint, string> SavePointsRegister => savePointsRegister;

        public Vector3 CharacterSpawnPosition => characterSpawnPoint.position;

        public void OnBeforeSerialize()
        {
            //do nothing
        }

        public void OnAfterDeserialize()
        {
            //savePointsRegister = savePoints.ToDictionary(dto => dto.SavePoint, dto => dto.ID);
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