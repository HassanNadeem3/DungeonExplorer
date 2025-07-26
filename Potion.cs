public class Potion : Item
{
    public int HealAmount { get; private set; }

    public Potion(string name, string description, int healAmount) 
        : base(name, description)
    {
        HealAmount = healAmount > 0 ? healAmount : throw new ArgumentException("Heal amount must be positive");
    }

    public override void OnCollected(Player player)
    {
        Console.WriteLine($"You found a {Name}!");
    }

    public override void Use(Player player)
    {
        int healedAmount = Math.Min(HealAmount, player.MaxHealth - player.Health);
        player.Health += healedAmount;
        Console.WriteLine($"You used {Name} and restored {healedAmount} health!");
    }
}