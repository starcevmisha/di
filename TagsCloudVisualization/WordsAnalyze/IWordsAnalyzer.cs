using System.Collections.Generic;

namespace TagsCloudVisualization.WordsAnalyze
{
    public interface IWordsAnalyzer
    {
        Dictionary<string, int> GetWordsFrequensy();
    }
}