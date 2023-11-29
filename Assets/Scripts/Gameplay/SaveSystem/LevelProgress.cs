using System.Collections.Generic;
using Newtonsoft.Json;

namespace Pelki.Gameplay.SaveSystem
{
    public class LevelProgress : BaseProgress<LevelProgress>
    {
        private readonly List<string> _activatedSavePoints = new List<string>();

        [JsonProperty]
        public string LastSavePointId { get; private set; }

        public IReadOnlyList<string> ActivatedSavePoints => _activatedSavePoints;

        public void AddActivatedSavePoint(string savePointId)
        {
            _activatedSavePoints.Add(savePointId);
            LastSavePointId = savePointId;
        }
    }
}