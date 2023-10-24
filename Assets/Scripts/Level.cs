using System.Collections.Generic;
using Pelki.Gameplay.SaveSystem;
using UnityEngine;

namespace Pelki
{
    public class Level : MonoBehaviour
    {
        [SerializeField] private Transform characterSpawnPoint;
        [SerializeField] private List<SavePoint> savePoints;

        public Vector3 CharacterSpawnPosition => characterSpawnPoint.position;
        public List<SavePoint> SavePoints => savePoints;
    }
}