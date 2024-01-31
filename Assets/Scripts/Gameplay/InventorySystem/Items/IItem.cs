namespace Pelki.Gameplay.InventorySystem.Items
{
    public interface IItem
    {
        string Id { get; }

        void Destroy();
    }
}