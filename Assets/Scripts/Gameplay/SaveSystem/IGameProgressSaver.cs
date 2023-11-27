namespace Pelki.Gameplay.SaveSystem
{
    public interface IGameProgressSaver
    {
        void SaveGameProgress<TProgress>(TProgress progress) where TProgress : BaseProgress<TProgress>;
    }
}