public abstract class Monster : Creature
{
    public int ExperienceReward { get; protected set; }

    protected Monster(string name, int maxHealth, int attackPower, int experienceReward) 
        : base(name, maxHealth, attackPower)
    {
        ExperienceReward = experienceReward >= 0 ? experienceReward : throw new ArgumentException("Experience reward cannot be negative");
    }
}


public class Goblin : Monster
{
    public Goblin() : base("Goblin", 30, 8, 10) { }

    public override int Attack()
    {
        var random = new Random();
        // Goblins have quick, less predictable attacks
        return random.Next(AttackPower - 3, AttackPower + 4);
    }

    public override string GetAttackDescription()
    {
        return $"{Name} slashes with its rusty dagger!";
    }
}


public class Dragon : Monster
{
    public Dragon() : base("Dragon", 120, 25, 100) { }

    public override int Attack()
    {
        var random = new Random();
        // Dragons have powerful, consistent attacks
        return random.Next(AttackPower - 5, AttackPower + 6);
    }

    public override string GetAttackDescription()
    {
        return $"{Name} breathes fire!";
    }
}


public class Orc : Monster
{
    public Orc() : base("Orc", 60, 15, 25) { }

    public override int Attack()
    {
        var random = new Random();
        return random.Next(AttackPower - 4, AttackPower + 5);
    }

    public override string GetAttackDescription()
    {
        return $"{Name} swings its massive club!";
    }
}
