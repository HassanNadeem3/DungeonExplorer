class Program
{
    static void Main(string[] args)
    {
        try
        {
            Game game = new Game();
            game.Start();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Fatal error: {ex.Message}");
        }
        finally
        {
            Console.WriteLine("\nPress any key to exit...");
            Console.ReadKey();
        }
    }
}