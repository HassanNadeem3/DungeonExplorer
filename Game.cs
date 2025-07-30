using System;
using System.Collections.Generic;
using System.Linq;
public class Game
{
    private Player _player;
    private GameMap _gameMap;
    private Room _currentRoom;
    private bool _gameRunning;

    public Game()
    {
        InitializeGame();
    }

    private void InitializeGame()
    {
        Console.WriteLine("Welcome to Dungeon Explorer!");
        Console.Write("Enter your character's name: ");
        string playerName = Console.ReadLine();

        if (string.IsNullOrWhiteSpace(playerName))
            playerName = "Hero";

        _player = new Player(playerName);
        _gameMap = new GameMap();
        _currentRoom = _gameMap.StartingRoom;
        _gameRunning = true;

        Console.WriteLine($"\nWelcome, {_player.Name}! Your adventure begins...\n");
    }

    public void Start()
    {
        try
        {
            while (_gameRunning && _player.IsAlive)
            {
                _currentRoom.DisplayRoom();
                DisplayPlayerStatus();
                ProcessCommand();
            }

            EndGame();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred: {ex.Message}");
        }
    }

    private void ProcessCommand()
    {
        try
        {
            Console.Write("\nWhat would you like to do? ");
            string input = Console.ReadLine()?.Trim().ToLower();

            if (string.IsNullOrEmpty(input))
            {
                Console.WriteLine("Please enter a command.");
                return;
            }

            string[] parts = input.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            string command = parts[0];
            string argument = parts.Length > 1 ? string.Join(" ", parts.Skip(1)) : "";

            switch (command)
            {
                case "go":
                case "move":
                    Move(argument);
                    break;
                case "attack":
                case "fight":
                    Attack();
                    break;
                case "take":
                case "pick":
                case "get":
                    TakeItem(argument);
                    break;
                case "use":
                    UseItem(argument);
                    break;
                case "inventory":
                case "inv":
                    _player.Inventory.DisplayInventory();
                    break;
                case "look":
                    _currentRoom.DisplayRoom();
                    break;
                case "help":
                    DisplayHelp();
                    break;
                case "quit":
                case "exit":
                    _gameRunning = false;
                    break;
                default:
                    Console.WriteLine("Unknown command. Type 'help' for available commands.");
                    break;
            }
        }
        catch (ArgumentException ex)
        {
            Console.WriteLine($"Invalid input: {ex.Message}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred while processing the command: {ex.Message}");
        }
    }

    private void Move(string direction)
    {
        if (string.IsNullOrWhiteSpace(direction))
        {
            Console.WriteLine("Please specify a direction (north, south, east, west).");
            return;
        }

        if (_currentRoom.HasMonsters)
        {
            Console.WriteLine("You cannot leave while monsters are present! You must fight or flee.");
            return;
        }

        Room nextRoom = _currentRoom.GetExit(direction);
        if (nextRoom == null)
        {
            Console.WriteLine($"There is no exit to the {direction}.");
            return;
        }

        _currentRoom = nextRoom;
        Console.WriteLine($"You move {direction}.");
    }

    private void Attack()
    {
        var aliveMonsters = _currentRoom.Monsters.Where(m => m.IsAlive).ToList();

        if (aliveMonsters.Count == 0)
        {
            Console.WriteLine("There are no monsters to fight here.");
            return;
        }

        // Fight the strongest monster first
        Monster target = _currentRoom.GetStrongestAliveMonster();

        Console.WriteLine($"\n--- Combat with {target.Name} ---");

        // Player attacks first
        int playerDamage = _player.Attack();
        Console.WriteLine(_player.GetAttackDescription());
        target.TakeDamage(playerDamage);
        Console.WriteLine($"You deal {playerDamage} damage to {target.Name}!");

        if (!target.IsAlive)
        {
            Console.WriteLine($"You defeated the {target.Name}!");
            return;
        }

        // Monster counter-attacks
        int monsterDamage = target.Attack();
        Console.WriteLine(target.GetAttackDescription());
        _player.TakeDamage(monsterDamage);
        Console.WriteLine($"The {target.Name} deals {monsterDamage} damage to you!");

        if (!_player.IsAlive)
        {
            Console.WriteLine("You have been defeated!");
        }
    }

    private void TakeItem(string itemName)
    {
        if (string.IsNullOrWhiteSpace(itemName))
        {
            Console.WriteLine("Please specify which item to take.");
            return;
        }

        var item = _currentRoom.Items.FirstOrDefault(i =>
            i.Name.Equals(itemName, StringComparison.OrdinalIgnoreCase));

        if (item == null)
        {
            Console.WriteLine($"There is no '{itemName}' here.");
            return;
        }

        if (_player.Inventory.AddItem(item))
        {
            _currentRoom.RemoveItem(item);
            item.OnCollected(_player);
        }
    }

    private void UseItem(string itemName)
    {
        if (string.IsNullOrWhiteSpace(itemName))
        {
            Console.WriteLine("Please specify which item to use.");
            return;
        }

        _player.UseItem(itemName);
    }

    private void DisplayPlayerStatus()
    {
        Console.WriteLine($"\n{_player.Name} - Health: {_player.Health}/{_player.MaxHealth} | Attack: {_player.AttackPower}");
        if (_player.CurrentWeapon != null)
        {
            Console.WriteLine($"Current weapon: {_player.CurrentWeapon.Name}");
        }
    }

    private void DisplayHelp()
    {
        Console.WriteLine("\nAvailable Commands:");
        Console.WriteLine("- go/move [direction] - Move in a direction (north, south, east, west)");
        Console.WriteLine("- attack/fight - Attack monsters in the current room");
        Console.WriteLine("- take/pick/get [item] - Pick up an item");
        Console.WriteLine("- use [item] - Use an item from inventory");
        Console.WriteLine("- inventory/inv - View your inventory");
        Console.WriteLine("- look - Look around the current room");
        Console.WriteLine("- help - Show this help message");
        Console.WriteLine("- quit/exit - Quit the game");
    }

    private void EndGame()
    {
        if (!_player.IsAlive)
        {
            Console.WriteLine("\n=== GAME OVER ===");
            Console.WriteLine("You have been defeated in the dungeon!");
        }
        else
        {
            Console.WriteLine("\n=== GAME ENDED ===");
            Console.WriteLine("Thank you for playing Dungeon Explorer!");
        }

        // Display final statistics using LINQ
        var totalMonstersDefeated = _gameMap.GetRoomsWithMonsters()
            .SelectMany(r => r.Monsters)
            .Count(m => !m.IsAlive);

        Console.WriteLine($"Final Statistics:");
        Console.WriteLine($"- Monsters defeated: {totalMonstersDefeated}");
        Console.WriteLine($"- Items collected: {_player.Inventory.Count}");
        Console.WriteLine($"- Final health: {_player.Health}/{_player.MaxHealth}");
    }
}