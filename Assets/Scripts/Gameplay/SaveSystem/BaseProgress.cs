namespace Pelki.Gameplay.SaveSystem
{
    public abstract class BaseProgress
    {
        protected IGameProgressSaver gameProgressSaver;
        
        public void Initialize(IGameProgressSaver gameProgressStorage)
        {
            gameProgressSaver = gameProgressStorage;
        }
    }
}