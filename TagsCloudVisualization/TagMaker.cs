﻿using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
 using TagsCloudVisualization.Helpers;

namespace TagsCloudVisualization
{
    public class TagMaker : ITagMaker
    {
        private readonly ICloudLayouter layouter;
        private readonly IExiter exiter;
        private readonly IFontSizeMaker fontSizeMaker;
        private readonly string fontName;

        public TagMaker(ICloudLayouter layouter,
            IExiter exiter,
            IFontSizeMaker fontSizeMaker, 
            string fontName)
        {
            this.layouter = layouter;
            this.exiter = exiter;
            this.fontSizeMaker = fontSizeMaker;
            this.fontName = fontName;
        }

        public Dictionary<Rectangle, (string, Font)> MakeTagRectangles(
            Dictionary<string, int> frequencyDict)
        {
            var tagsDict = new Dictionary<Rectangle, (string, Font)>();
            var maxfreq = frequencyDict.Values.Max();
            var fontFamily = Result.Of(() => new FontFamily(fontName));
            if(!fontFamily.IsSuccess)
                exiter.ExitWithError(fontFamily.Error);
            
            foreach (var word in frequencyDict)
            {
                var font = new Font(fontFamily.Value, fontSizeMaker.GetFontSizeByFreq(maxfreq, word.Value),
                    FontStyle.Regular, GraphicsUnit.Pixel);
                var tagSize = TextRenderer.MeasureText(word.Key, font);
                tagsDict.Add(layouter.PutNextRectangle(tagSize), (word.Key, font));
            }
            return tagsDict;
        }
    }
}