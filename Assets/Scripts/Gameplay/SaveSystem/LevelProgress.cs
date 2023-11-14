using UnityEngine;

namespace Pelki.Gameplay.SaveSystem
{
    public class LevelProgress : Progress
    {
        public string SavePointId { get; set; }

        public LevelProgress(string savePointId)
        {
            SavePointId = savePointId;
        }

        public void Save()
        {
            gameProgressSaver.SaveGameProgress(this);
        }
    }
}