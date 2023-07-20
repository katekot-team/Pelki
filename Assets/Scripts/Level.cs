using UnityEngine;

namespace Pelki
{
    public class Level : MonoBehaviour
    {
        [SerializeField] private Transform characterSpawnPoint;

        public Vector3 CharacterSpawnPosition => characterSpawnPoint.position;
    }
}