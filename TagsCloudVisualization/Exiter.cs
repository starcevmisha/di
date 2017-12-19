using System;
using Castle.Core.Resource;

namespace TagsCloudVisualization
{
    public class Exiter : IExiter
    {
        public void ExitWithError(string errorMessage)
        {
            Console.WriteLine(errorMessage);
            Console.WriteLine("\nPress ESC to exit");
            while (Console.ReadKey(true).Key != ConsoleKey.Escape) { };
            Environment.Exit(1);
        }
    }
}