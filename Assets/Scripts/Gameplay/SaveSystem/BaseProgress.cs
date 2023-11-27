namespace Pelki.Gameplay.SaveSystem
{
    public abstract class BaseProgress<TSelfProgress>
        //sttrox: пришлось ограничить, что бы SaveGameProgress принял тип наследника, а не BaseProgress'a
        where TSelfProgress : BaseProgress<TSelfProgress>
    {
        protected IGameProgressSaver gameProgressSaver;

        public void Initialize(IGameProgressSaver gameProgressStorage)
        {
            gameProgressSaver = gameProgressStorage;
        }

        public void Save()
        {
            //sttrox: пришлось кастить, что бы SaveGameProgress принял тип наследника, а не BaseProgress'a
            TSelfProgress childProgress = (TSelfProgress)this;
            gameProgressSaver.SaveGameProgress(childProgress);
        }
    }
}