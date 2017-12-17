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

        public FileReader(string filename)
        {
            this.filename = filename;
        }

        public IEnumerable<string> ReadWords()
        {
            var file = Result.Of(()=>File.ReadLines(filename));
            if (!file.IsSuccess)
                Exiter.ExitWithError(file.Error);
            
            return file.Value
                .SelectMany(line => line.Split(
                    new[] {' ', ',', '.', ':', ';', '!', '?', '\t', '–', '"'}, 
                    StringSplitOptions.RemoveEmptyEntries));
        }
    }

}