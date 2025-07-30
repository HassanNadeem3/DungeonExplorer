using System;
using System.Collections.Generic;
using System.Linq;

public class Room
{
    public string Name { get; private set; }
    public string Description { get; private set; }
    public Dictionary<string, Room> Exits { get; private set; }
    public List<Monster> Monsters { get; private set; }
    public List<Item> Items { get; private set; }

    public bool HasMonsters => Monsters.Any(m => m.IsAlive);
    public bool HasItems => Items.Count > 0;

    public Room(string name, string description)
    {
        Name = name ?? throw new ArgumentNullException(nameof(name));
        Description = description ?? throw new ArgumentNullException(nameof(description));
        Exits = new Dictionary<string, Room>(StringComparer.OrdinalIgnoreCase);
        Monsters = new List<Monster>();
        Items = new List<Item>();
    }

    public void AddExit(string direction, Room room)
    {
        if (string.IsNullOrWhiteSpace(direction)) throw new ArgumentException("Direction cannot be null or empty");
        if (room == null) throw new ArgumentNullException(nameof(room));

        Exits[direction] = room;
    }

    public Room GetExit(string direction)
    {
        Exits.TryGetValue(direction, out Room room);
        return room;
    }

    public void AddMonster(Monster monster)
    {
        if (monster == null) throw new ArgumentNullException(nameof(monster));
        Monsters.Add(monster);
    }

    public void AddItem(Item item)
    {
        if (item == null) throw new ArgumentNullException(nameof(item));
        Items.Add(item);
    }

    public void RemoveItem(Item item)
    {
        Items.Remove(item);
    }

    // LINQ Example - Find strongest alive monster
    public Monster GetStrongestAliveMonster()
    {
        return Monsters.Where(m => m.IsAlive)
                      .OrderByDescending(m => m.AttackPower)
                      .FirstOrDefault();
    }

    public void DisplayRoom()
    {
        Console.WriteLine($"\n=== {Name} ===");
        Console.WriteLine(Description);

        if (HasMonsters)
        {
            var aliveMonsters = Monsters.Where(m => m.IsAlive).ToList();
            Console.WriteLine($"Monsters present: {string.Join(", ", aliveMonsters.Select(m => m.Name))}");
        }

        if (HasItems)
        {
            Console.WriteLine($"Items here: {string.Join(", ", Items.Select(i => i.Name))}");
        }

        if (Exits.Count > 0)
        {
            Console.WriteLine($"Exits: {string.Join(", ", Exits.Keys)}");
        }
    }
}