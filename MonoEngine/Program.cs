using System;
using MonoEngine.Framework;

namespace MonoEngine;

public static class Program {
    [STAThread]
    public static void Main(string[] args) {
        Console.WriteLine("Initialize Game!");
        GameManager game = new();
        game.Run();
    }
}