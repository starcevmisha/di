namespace TagsCloudVisualization.CloudTagDrawer
{
    public interface IFontSizeMaker
    {
        int GetFontSizeByFreq(int maxFreq, int frequency);
    }
}