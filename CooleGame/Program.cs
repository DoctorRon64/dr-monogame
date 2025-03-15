using System;

namespace CooleGame
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            Console.WriteLine(" Initialized Game!");
            GameManager game = new();
            game.Run();
        }
    }
}