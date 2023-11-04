namespace Pelki.Gameplay.SaveSystem
{
    public interface IGameProgressSaver
    {
        void SaveGameProgress<TObj>(TObj t);
    }
}