namespace Raydevs.Interfaces
{
    public interface IDamageable
    {
        public void TakeDamage(int damage, float knockbackForce, bool isCritical);

    }
}