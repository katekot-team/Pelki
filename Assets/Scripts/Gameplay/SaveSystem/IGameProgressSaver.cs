namespace Pelki.Gameplay.SaveSystem
{
    public interface IGameProgressSaver
    {
        void SaveObject<TObj>(TObj t);
    }
}