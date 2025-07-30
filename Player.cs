using System;
using System.Collections.Generic;
using System.Linq;

public class Player : Creature
{
    public Inventory Inventory { get; private set; }
    public Weapon CurrentWeapon { get; private set; }
    private int _baseAttackPower;

    public Player(string name, int maxHealth = 100, int baseAttackPower = 10)
        : base(name, maxHealth, baseAttackPower)
    {
        Inventory = new Inventory();
        _baseAttackPower = baseAttackPower;
    }

    public void EquipWeapon(Weapon weapon)
    {
        if (weapon == null) throw new ArgumentNullException(nameof(weapon));

        CurrentWeapon = weapon;
        AttackPower = _baseAttackPower + weapon.Damage;
    }

    public override int Attack()
    {
        var random = new Random();
        int damage = random.Next(AttackPower / 2, AttackPower + 1);
        return damage;
    }

    public override string GetAttackDescription()
    {
        return CurrentWeapon != null
            ? $"{Name} attacks with {CurrentWeapon.Name}!"
            : $"{Name} attacks with bare hands!";
    }

    public void UseItem(string itemName)
    {
        var item = Inventory.FindItem(itemName);
        if (item == null)
        {
            Console.WriteLine($"Item '{itemName}' not found in inventory.");
            return;
        }

        item.Use(this);

        // Remove potions after use
        if (item is Potion)
        {
            Inventory.RemoveItem(item);
        }
    }
}