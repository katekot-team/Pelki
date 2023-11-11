namespace Pelki.Gameplay.SaveSystem
{
    public abstract class Progress
    {
        protected IGameProgressSaver gameProgressSaver;
        
        public void Initialize(IGameProgressSaver gameProgressStorage)
        {
            gameProgressSaver = gameProgressStorage;
        }
    }
}