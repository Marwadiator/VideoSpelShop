using Microsoft.EntityFrameworkCore;
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
            Console.WriteLine("3. Add Category");
            Console.WriteLine("4. View Categories");
            Console.WriteLine("5. Manage Customer Cart");
            Console.WriteLine("6. Place Order");
            Console.WriteLine("7. Exit");
            Console.Write("Select an option: ");

            switch (Console.ReadLine())
            {
                case "1": AddGame(); break;
                case "2": ViewGames(); break;
                case "3": AddCategory(); break;
                case "4": ViewCategories(); break;
                case "5": ManageCart(); break;
                case "6": PlaceOrder(); break;
                case "7": return;
                default: Console.WriteLine("Invalid option! Press any key to try again."); Console.ReadKey(); break;
            }
        }
    }

    static void AddGame()
    {
        using (var context = new VideoSpelShopDbContext())
        {
            Console.Write("Enter game name: ");
            string name = Console.ReadLine();

            Console.Write("Enter genre: ");
            string genre = Console.ReadLine();

            Console.Write("Enter price: ");
            if (!decimal.TryParse(Console.ReadLine(), out decimal price))
            {
                Console.WriteLine("Invalid price. Press any key to continue.");
                Console.ReadKey();
                return;
            }

            Console.Write("Enter category ID: ");
            if (!int.TryParse(Console.ReadLine(), out int categoryId))
            {
                Console.WriteLine("Invalid category. Press any key to continue.");
                Console.ReadKey();
                return;
            }

            var game = new Game { Name = name, Genre = genre, Price = price, CategoryId = categoryId };
            context.Games.Add(game);
            context.SaveChanges();

            Console.WriteLine("Game added successfully! Press any key to continue.");
            Console.ReadKey();
        }
    }

    static void ViewGames()
    {
        using var context = new VideoSpelShopDbContext();
        var games = context.Games.Include(g => g.Category).ToList();

        if (games.Count == 0)
        {
            Console.WriteLine("No games found.");
            Console.ReadKey();
            return;
        }

        Console.WriteLine("\n📜 List of Games:");
        foreach (var game in games)
        {
            Console.WriteLine($"{game.GameId}. {game.Name} - {game.Genre} - ${game.Price} (Category: {game.Category.Name})");
        }

        Console.WriteLine("\nPress any key to continue.");
        Console.ReadKey();
    }

    static void AddCategory()
    {
        using var context = new VideoSpelShopDbContext();
        Console.Write("Enter category name: ");
        string name = Console.ReadLine();

        var category = new Category { Name = name };
        context.Categories.Add(category);
        context.SaveChanges();

        Console.WriteLine("Category added successfully! Press any key to continue.");
        Console.ReadKey();
    }

    static void ViewCategories()
    {
        using var context = new VideoSpelShopDbContext();
        var categories = context.Categories.ToList();

        if (categories.Count == 0)
        {
            Console.WriteLine("No categories found.");
            Console.ReadKey();
            return;
        }

        Console.WriteLine("\n📜 List of Categories:");
        foreach (var category in categories)
        {
            Console.WriteLine($"{category.CategoryId}. {category.Name}");
        }

        Console.WriteLine("\nPress any key to continue.");
        Console.ReadKey();
    }

    static void ManageCart()
    {
        using var context = new VideoSpelShopDbContext();
        Console.Write("Enter your email: ");
        string email = Console.ReadLine();

        var customer = context.Customers.FirstOrDefault(c => c.Email == email);
        if (customer == null)
        {
            Console.Write("Enter your name: ");
            string name = Console.ReadLine();
            customer = new Customer { Name = name, Email = email };
            context.Customers.Add(customer);
            context.SaveChanges();
        }

        Console.WriteLine("Available Games:");
        var games = context.Games.ToList();
        foreach (var game in games)
            Console.WriteLine($"{game.GameId}. {game.Name} - ${game.Price}");

        Console.Write("Enter Game ID to add to cart: ");
        if (!int.TryParse(Console.ReadLine(), out int gameId))
        {
            Console.WriteLine("Invalid Game ID.");
            return;
        }

        var gameToAdd = context.Games.Find(gameId);
        if (gameToAdd == null)
        {
            Console.WriteLine("Game not found.");
            return;
        }

        Console.Write("Enter quantity: ");
        if (!int.TryParse(Console.ReadLine(), out int quantity) || quantity < 1)
        {
            Console.WriteLine("Invalid quantity.");
            return;
        }

        var cartItem = context.CartItems.FirstOrDefault(c => c.CustomerId == customer.Id && c.GameId == gameId);
        if (cartItem != null)
            cartItem.Quantity += quantity;
        else
            context.CartItems.Add(new CartItem { CustomerId = customer.Id, GameId = gameId, Quantity = quantity });

        context.SaveChanges();
        Console.WriteLine("Game added to cart! Press any key to continue.");
        Console.ReadKey();
    }

    static void PlaceOrder()
    {
        using var context = new VideoSpelShopDbContext();
        Console.Write("Enter your email: ");
        string email = Console.ReadLine();

        var customer = context.Customers.FirstOrDefault(c => c.Email == email);
        if (customer == null)
        {
            Console.WriteLine("Customer not found! Please add items to your cart first.");
            Console.ReadKey();
            return;
        }

        var cartItems = context.CartItems.Where(c => c.CustomerId == customer.Id).ToList();
        if (!cartItems.Any())
        {
            Console.WriteLine("Your cart is empty! Add games before placing an order.");
            Console.ReadKey();
            return;
        }

        var order = new Order { CustomerId = customer.Id, OrderDetails = new List<OrderDetail>() };
        decimal totalAmount = 0;

        foreach (var item in cartItems)
        {
            var orderDetail = new OrderDetail
            {
                GameId = item.GameId,
                Quantity = item.Quantity,
                Price = item.Quantity * context.Games.Find(item.GameId).Price
            };
            totalAmount += orderDetail.Price;
            order.OrderDetails.Add(orderDetail);
        }

        context.Orders.Add(order);
        context.CartItems.RemoveRange(cartItems);
        context.SaveChanges();

        Console.WriteLine($"Order placed successfully! Total amount: ${totalAmount}");
        Console.WriteLine("Press any key to continue.");
        Console.ReadKey();
    }
}

