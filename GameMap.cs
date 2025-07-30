using System;
using System.Collections.Generic;
using System.Linq;

public class GameMap
{
    private Dictionary<string, Room> _rooms;
    public Room StartingRoom { get; private set; }

    public GameMap()
    {
        _rooms = new Dictionary<string, Room>(StringComparer.OrdinalIgnoreCase);
        CreateMap();
    }

    private void CreateMap()
    {
        // Create rooms
        var entrance = new Room("Dungeon Entrance", "A dark entrance to the dungeon. You can smell danger ahead.");
        var corridor = new Room("Dark Corridor", "A long, narrow corridor with stone walls covered in moss.");
        var treasureRoom = new Room("Treasure Chamber", "A magnificent room filled with gold and precious artifacts!");
        var armory = new Room("Old Armory", "An abandoned armory with weapons scattered on the floor.");
        var dragonLair = new Room("Dragon's Lair", "A massive cavern with bones scattered everywhere. The air is thick with smoke.");

        // Add items to rooms
        armory.AddItem(new Weapon("Iron Sword", "A sturdy iron sword", 15));
        armory.AddItem(new Potion("Health Potion", "Restores 30 health", 30));
        treasureRoom.AddItem(new Weapon("Legendary Sword", "A glowing sword of immense power", 30));
        corridor.AddItem(new Potion("Small Potion", "Restores 15 health", 15));

        // Add monsters to rooms
        corridor.AddMonster(new Goblin());
        armory.AddMonster(new Orc());
        dragonLair.AddMonster(new Dragon());

        // Connect rooms
        entrance.AddExit("north", corridor);
        corridor.AddExit("south", entrance);
        corridor.AddExit("east", armory);
        corridor.AddExit("west", treasureRoom);
        armory.AddExit("west", corridor);
        armory.AddExit("north", dragonLair);
        treasureRoom.AddExit("east", corridor);
        dragonLair.AddExit("south", armory);

        // Add to dictionary
        _rooms["entrance"] = entrance;
        _rooms["corridor"] = corridor;
        _rooms["treasure"] = treasureRoom;
        _rooms["armory"] = armory;
        _rooms["lair"] = dragonLair;

        StartingRoom = entrance;
    }

    public Room GetRoom(string roomName)
    {
        _rooms.TryGetValue(roomName, out Room room);
        return room;
    }

    // LINQ Examples
    public IEnumerable<Room> GetRoomsWithMonsters()
    {
        return _rooms.Values.Where(r => r.HasMonsters);
    }

    public IEnumerable<Room> GetRoomsWithItems()
    {
        return _rooms.Values.Where(r => r.HasItems);
    }

    public int GetTotalAliveMonsters()
    {
        return _rooms.Values.SelectMany(r => r.Monsters).Count(m => m.IsAlive);
    }
}