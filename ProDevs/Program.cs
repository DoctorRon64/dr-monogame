using System;

namespace ProDevs;

public static class Program {
    public static void Main(string[] args) {
        Console.WriteLine(" Initialize Game!");
        GameManager game = new();
        game.Run();
    }
}