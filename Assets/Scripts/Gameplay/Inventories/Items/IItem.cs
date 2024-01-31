namespace Pelki.Gameplay.Inventories.Items
{
    public interface IItem
    {
        string Id { get; }

        void Destroy();
    }
}