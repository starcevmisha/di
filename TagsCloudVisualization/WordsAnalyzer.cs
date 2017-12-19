using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using NHunspell;
using NUnit.Framework;
using TagsCloudVisualization.Helpers;

namespace TagsCloudVisualization
{
    public class WordsAnalyzer : IWordsAnalyzer
    {
        private readonly int count;
        private readonly int minLength;
        private readonly IBoringWordDeterminer boringWordDeterminer;
        private readonly IReader reader;
        private readonly IExiter exiter;

        public WordsAnalyzer(
            IBoringWordDeterminer boringWordDeterminer,
            IReader reader,
            IExiter exiter,
            int count,
            int minLength = 0)
        {
            this.count = count;
            this.minLength = minLength;
            this.boringWordDeterminer = boringWordDeterminer;
            this.reader = reader;
            this.exiter = exiter;
        }

        public Dictionary<string, int> GetWordsFrequensy()
        {
            var hunspell = Result.Of(() => new Hunspell("dictionaries/en_US.aff", "dictionaries/en_US.dic"));
            if (!hunspell.IsSuccess)
                exiter.ExitWithError(hunspell.Error);
            using(hunspell.Value)
            {
                return reader.ReadWords()
                    .Select(x=>
                    {
                        var word = x.ToLower();
                        var stems = hunspell.Value.Stem(word);
                        return stems.Any() ? stems[0] : word;
                    })
                    .Where(word => !boringWordDeterminer.IsBoringWord(word))
                    .Where(word => word.Length > minLength)
                    .GroupBy(word => word)
                    .OrderByDescending(x => x.Count())
                    .Take(count)
                    .ToDictionary(x => x.Key, x => x.Count());
            }
        }
    }

    [TestFixture]
    public class FileAnalyzer_Should
    {
        [Test]
        public void DoSomething_WhenSomething()
        {
            using (var hunspell = new Hunspell(
                TestContext.CurrentContext.TestDirectory + "\\dictionaries\\en_US.aff",
                TestContext.CurrentContext.TestDirectory + "\\dictionaries\\en_US.dic"))
            {
                var stems = hunspell.Stem("decompressed");
                var actualWord = stems[0];
                var expected = "compress";
                actualWord.Should().Be(expected);
            }
        }
    }
}