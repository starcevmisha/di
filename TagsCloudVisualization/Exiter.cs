using System;

namespace TagsCloudVisualization
{
    public class Exiter
    {
        public static void ExitWithError(string errorMessage)
        {
            Console.WriteLine(errorMessage);
            Console.WriteLine("\nPress ESC to exit");
            while (Console.ReadKey(true).Key != ConsoleKey.Escape) { };
            Environment.Exit(1);
        }
    }
}