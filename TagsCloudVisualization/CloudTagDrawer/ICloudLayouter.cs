using System.Drawing;

namespace TagsCloudVisualization.CloudTagDrawer
{
    public interface ICloudLayouter
    {
        Rectangle PutNextRectangle(Size rectangleSize);
    }
}