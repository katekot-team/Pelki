namespace Pelki.Interfaces
{
    interface IDamageable
    {
        int Health { get; }
        void TakeDamage(int damage);
    }
}
