using System;
using System.Collections.Generic;
using System.Linq;

public class Weapon : Item
{
    public int Damage { get; private set; }

    public Weapon(string name, string description, int damage)
        : base(name, description)
    {
        Damage = damage > 0 ? damage : throw new ArgumentException("Weapon damage must be positive");
    }

    public override void OnCollected(Player player)
    {
        Console.WriteLine($"You picked up {Name}!");
    }

    public override void Use(Player player)
    {
        player.EquipWeapon(this);
        Console.WriteLine($"You equipped {Name}. Attack power increased by {Damage}!");
    }
}
