using System;

namespace MonoEngine;

public static class Program {
    [STAThread]
    public static void Main(string[] args) {
        Console.WriteLine("Initialize Game!");
        using var game = new GameManager();
        game.Run();
    }
}