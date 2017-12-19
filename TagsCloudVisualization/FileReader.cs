using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using NUnit.Framework;
using TagsCloudVisualization.Helpers;


namespace TagsCloudVisualization
{
    public class FileReader : IReader
    {
        private readonly string filename;
        private readonly IExiter exiter;

        public FileReader(string filename, IExiter exiter)
        {
            this.filename = filename;
            this.exiter = exiter;
        }

        public IEnumerable<string> ReadWords()
        {
            var file = Result.Of(()=>File.ReadLines(filename));
            if (!file.IsSuccess)
                exiter.ExitWithError(file.Error);
            
            return file.Value
                .SelectMany(line => line.Split(
                    new[] {' ', ',', '.', ':', ';', '!', '?', '\t', '–', '"'}, 
                    StringSplitOptions.RemoveEmptyEntries));
        }
    }

}