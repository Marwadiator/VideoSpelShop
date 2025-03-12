using System;
using System.Linq;
using VideoSpelShop.Data;
using VideoSpelShop.Models;

class Program
{
    static void Main()
    {
        while (true)
        {
            Console.Clear();
            Console.WriteLine("🎮 VideoSpel Shop");
            Console.WriteLine("1. Add Game");
            Console.WriteLine("2. View Games");
            Console.WriteLine("3. Edit Game");
            Console.WriteLine("4. Delete Game");
            Console.WriteLine("5. Exit");
            Console.Write("Select an option: ");

            string choice = Console.ReadLine();
            switch (choice)
            {
                case "1":
                    AddGame();
                    break;
                case "2":
                    ViewGames();
                    break;
                case "3":
                    EditGame();
                    break;
                case "4":
                    DeleteGame();
                    break;
                case "5":
                    return;
                default:
                    Console.WriteLine("Invalid option! Press any key to try again.");
                    Console.ReadKey();
                    break;
            }
        }
    }

    static void AddGame()
    {
        using var context = new VideoSpelShopDbContext();
        Console.Write("Enter game name: ");
        string name = Console.ReadLine();

        Console.Write("Enter genre: ");
        string genre = Console.ReadLine();

        Console.Write("Enter price: ");
        if (!decimal.TryParse(Console.ReadLine(), out decimal price))
        {
            Console.WriteLine("Invalid price entered. Press any key to continue.");
            Console.ReadKey();
            return;
        }

        var game = new Game { Name = name, Genre = genre, Price = price };
        context.Games.Add(game);
        context.SaveChanges();

        Console.WriteLine("Game added successfully! Press any key to continue.");
        Console.ReadKey();
    }

    static void ViewGames()
    {
        using var context = new VideoSpelShopDbContext();
        var games = context.Games.ToList();

        Console.WriteLine("\n📜 List of Games:");
        foreach (var game in games)
        {
            Console.WriteLine($"{game.Id}. {game.Name} - {game.Genre} - ${game.Price}");
        }

        Console.WriteLine("\nPress any key to continue.");
        Console.ReadKey();
    }

    static void EditGame()
    {
        using var context = new VideoSpelShopDbContext();
        Console.Write("Enter game ID to edit: ");
        if (!int.TryParse(Console.ReadLine(), out int id))
        {
            Console.WriteLine("Invalid ID. Press any key to continue.");
            Console.ReadKey();
            return;
        }

        var game = context.Games.Find(id);
        if (game == null)
        {
            Console.WriteLine("Game not found! Press any key to continue.");
            Console.ReadKey();
            return;
        }

        Console.Write($"Enter new name ({game.Name}): ");
        string newName = Console.ReadLine();
        if (!string.IsNullOrWhiteSpace(newName))
            game.Name = newName;

        Console.Write($"Enter new genre ({game.Genre}): ");
        string newGenre = Console.ReadLine();
        if (!string.IsNullOrWhiteSpace(newGenre))
            game.Genre = newGenre;

        Console.Write($"Enter new price ({game.Price}): ");
        if (decimal.TryParse(Console.ReadLine(), out decimal newPrice))
            game.Price = newPrice;

        context.SaveChanges();
        Console.WriteLine("Game updated successfully! Press any key to continue.");
        Console.ReadKey();
    }

    static void DeleteGame()
    {
        using var context = new VideoSpelShopDbContext();
        Console.Write("Enter game ID to delete: ");
        if (!int.TryParse(Console.ReadLine(), out int id))
        {
            Console.WriteLine("Invalid ID. Press any key to continue.");
            Console.ReadKey();
            return;
        }

        var game = context.Games.Find(id);
        if (game == null)
        {
            Console.WriteLine("Game not found! Press any key to continue.");
            Console.ReadKey();
            return;
        }

        context.Games.Remove(game);
        context.SaveChanges();
        Console.WriteLine("Game deleted successfully! Press any key to continue.");
        Console.ReadKey();
    }
}