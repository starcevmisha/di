using System.Collections.Generic;
using System.Drawing;
using FluentAssertions;
using NUnit.Framework;
using Moq;

namespace TagsCloudVisualization.Tests
{
    public class CloudTagDrawer_Tests
    {
        private Mock<IWordsAnalyzer> wordsAnalyzerMock;
        private Mock<ITagMaker> tagMakerMock;
        private Mock<IBitmapViewer> bitmapViewerMock;
        private Mock<IExiter> exiterMock;
        private CloudTagDrawer cloudTagDrawer;

        [SetUp]
        public void SetUp()
        {
            wordsAnalyzerMock = new Mock<IWordsAnalyzer>();
            tagMakerMock = new Mock<ITagMaker>();
            bitmapViewerMock = new Mock<IBitmapViewer>();
            exiterMock = new Mock<IExiter>();
            cloudTagDrawer = new CloudTagDrawer(wordsAnalyzerMock.Object, 
                tagMakerMock.Object, 
                bitmapViewerMock.Object, 
                exiterMock.Object,
                100,100, "SomeFileNAme");
        }

        [Test]
        public void Fail_WhenRectanglesCanNotBePutOnSmallImage()
        {
            var actualError = "";
            exiterMock.Setup(x => x.ExitWithError(It.IsAny<string>()))
                .Callback<string>(er => actualError = er);
            var expectedError = "Too small image size.";
            var tagRectangles = new Dictionary<Rectangle, (string, Font)>()
            {
                {new Rectangle(10,10,100,100), ("hello", new Font("Tahoma", 12)) }
            };
            
            tagMakerMock
                .Setup(x => x.MakeTagRectangles(It.IsAny<Dictionary<string, int>>()))
                .Returns(tagRectangles);
            
            cloudTagDrawer.DrawTags();

            actualError.Should().StartWith(expectedError);
        }
    }
}