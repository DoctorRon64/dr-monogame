using System;
using System.Diagnostics;
using System.IO;
using Microsoft.Xna.Framework.Content;

namespace MonoEngine.Utility;

public static class MgcbEditorLauncher
{
    public static void OpenMgcbEditor()
    {
        string solutionRoot = Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, @"..\..\..\.."));
        string mgcbPath = Path.Combine(solutionRoot, "MonoEngine", "Content", "Content.mgcb");

        Console.WriteLine("Opening MGCB Editor with file: " + mgcbPath);

        if (!File.Exists(mgcbPath))
        {
            Console.WriteLine("Error: Content.mgcb not found at: " + mgcbPath);
            return;
        }

        ProcessStartInfo startInfo = new ProcessStartInfo
        {
            FileName = "dotnet",
            Arguments = $"mgcb-editor \"{mgcbPath}\"",
            UseShellExecute = false,
            CreateNoWindow = false,
        };

        Process.Start(startInfo);
    }
    
    public static void RebuildContent()
    {
        string solutionRoot = Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, @"..\..\..\.."));
        string mgcbPath = Path.Combine(solutionRoot, "MonoEngine", "Content", "Content.mgcb");

        Console.WriteLine("Rebuilding MGCB content: " + mgcbPath);

        if (!File.Exists(mgcbPath))
        {
            Console.WriteLine("Error: Content.mgcb not found at: " + mgcbPath);
            return;
        }

        ProcessStartInfo startInfo = new ProcessStartInfo
        {
            FileName = "dotnet",
            Arguments = $"mgcb /quiet /@:\"{mgcbPath}\" /platform:DesktopGL /outputDir:\"{Path.Combine(solutionRoot, "MonoEngine", "Content", "bin", "DesktopGL", "Content")}\" /intermediateDir:\"{Path.Combine(solutionRoot, "MonoEngine", "Content", "obj", "DesktopGL", "net8.0", "Content")}\" /workingDir:\"{Path.Combine(solutionRoot, "MonoEngine", "Content")}\"",
            UseShellExecute = false,
            CreateNoWindow = false,
            RedirectStandardOutput = true,
            RedirectStandardError = true
        };

        Process process = Process.Start(startInfo);
        process?.WaitForExit();
        
        Console.WriteLine("Rebuild finished!");
    }
}