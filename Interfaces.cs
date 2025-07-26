// IDamageable.cs
public interface IDamageable
{
    int Health { get; set; }
    int MaxHealth { get; }
    void TakeDamage(int damage);
    bool IsAlive { get; }
}

// ICollectible.cs
public interface ICollectible
{
    string Name { get; }
    string Description { get; }
    void OnCollected(Player player);
}