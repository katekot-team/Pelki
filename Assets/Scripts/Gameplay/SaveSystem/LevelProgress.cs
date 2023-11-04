namespace Pelki.Gameplay.SaveSystem
{
    public class LevelProgress
    {
        private IGameProgressSaver gameProgressSaver;

        public string SavePointId { get; set; }

        public LevelProgress(string savePointId)
        {
            SavePointId = savePointId;
        }

        public void Save()
        {
            gameProgressSaver.SaveGameProgress(this);
        }

        public void Initialize(IGameProgressSaver gameProgressStorage)
        {
            gameProgressSaver = gameProgressStorage;
        }
    }
}