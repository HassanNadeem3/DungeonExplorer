public abstract class Creature : IDamageable
{
    public string Name { get; protected set; }
    public int Health { get; set; }
    public int MaxHealth { get; protected set; }
    public int AttackPower { get; protected set; }

    public bool IsAlive => Health > 0;

    protected Creature(string name, int maxHealth, int attackPower)
    {
        Name = name ?? throw new ArgumentNullException(nameof(name));
        MaxHealth = maxHealth > 0 ? maxHealth : throw new ArgumentException("Max health must be positive");
        Health = MaxHealth;
        AttackPower = attackPower >= 0 ? attackPower : throw new ArgumentException("Attack power cannot be negative");
    }

    public virtual void TakeDamage(int damage)
    {
        if (damage < 0) throw new ArgumentException("Damage cannot be negative");
        Health = Math.Max(0, Health - damage);
    }

    public abstract int Attack();
    public abstract string GetAttackDescription();
}