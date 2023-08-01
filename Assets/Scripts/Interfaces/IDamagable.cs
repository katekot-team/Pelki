namespace Pelki.Interfaces
{
    public interface IDamageable
    {
        public int Health { get; }
        void TakeDamage(int damage);
    }
}
