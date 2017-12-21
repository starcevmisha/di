using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.Windows.Forms;
using TagsCloudVisualization.Helpers;

namespace TagsCloudVisualization
{
    public class CloudTagDrawer
    {
        private readonly IWordsAnalyzer wordsAnalyzer;
        private readonly ITagMaker tagMaker;
        private readonly IBitmapViewer bitmapViewer;
        private readonly IExiter exiter;
        private readonly int height;
        private readonly int width;
        private readonly string outputFilename;

        public CloudTagDrawer(
            IWordsAnalyzer wordsAnalyzer,
            ITagMaker tagMaker,
            IBitmapViewer bitmapViewer,
            IExiter exiter,
            int height,
            int width,
            string outputFilename
        )
        {
            this.wordsAnalyzer = wordsAnalyzer;
            this.tagMaker = tagMaker;
            this.bitmapViewer = bitmapViewer;
            this.exiter = exiter;
            this.height = height;
            this.width = width;
            this.outputFilename = outputFilename;
        }

        public void DrawTags()
        {
            var frequencyDict = wordsAnalyzer.GetWordsFrequensy();
            var tagRectangles = tagMaker.MakeTagRectangles(frequencyDict);
            
            var bitmap = Result.Of(() =>DrawTagsOnBitmap(tagRectangles));
            if (!bitmap.IsSuccess)
            {
                exiter.ExitWithError(bitmap.Error);
                return;
            }

            bitmapViewer.View(bitmap.Value);
        }



        private Bitmap DrawTagsOnBitmap(Dictionary<Rectangle, (string, Font)>tagRectangles)
        {
            var bitmap = new Bitmap(width, height);

            using (var g = Graphics.FromImage(bitmap))
            {
                g.TextRenderingHint = TextRenderingHint.AntiAliasGridFit;
                var selBrush = new SolidBrush(Color.Black);
                foreach (var tag in tagRectangles)
                {
                    checkCoordinates(tag.Key);
                    g.DrawString(tag.Value.Item1, tag.Value.Item2, selBrush, tag.Key.X, tag.Key.Y);
                }
            }
            return bitmap;
        }

        private void checkCoordinates(Rectangle rectangle)
        {
            if (rectangle.Left < 0
                || rectangle.Right > width
                || rectangle.Top < 0
                || rectangle.Bottom > height)
                throw new Exception($"Too small image size. " +
                                    $"Rectangle with coordinates x={rectangle.X}, y={rectangle.Y} " +
                                    $"and size {rectangle.Size.Width}*{rectangle.Size.Height} " +
                                    $"does not fit");
        }
                
        
    }
    
}