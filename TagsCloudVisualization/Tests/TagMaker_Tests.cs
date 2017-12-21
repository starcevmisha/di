using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using FluentAssertions;
using Moq;
using NUnit.Framework;

namespace TagsCloudVisualization.Tests
{
    [TestFixture]
    public class TagMaker_Mock
    {
        private Mock<IFontSizeMaker> fontSizeMakerMock;
        private Mock<ICloudLayouter> layouterMock;
        private Mock<IExiter> exiterMock;
        private TagMaker tagMaker;
        private Dictionary<string, int> frequencyDict;

        [SetUp]
        public void SetUp()
        {
            fontSizeMakerMock = new Mock<IFontSizeMaker>();
            layouterMock = new Mock<ICloudLayouter>();
            exiterMock = new Mock<IExiter>();
            tagMaker = new TagMaker(layouterMock.Object, exiterMock.Object,fontSizeMakerMock.Object, "Tahoma");
            frequencyDict = new Dictionary<string, int>()
            {
                {"test", 5}
            };
        }

        [Test]
        public void Fail_WhenUnrecognizedFont()
        {
            var actualError = "";
            exiterMock.Setup(x => x.ExitWithError(It.IsAny<string>()))
                .Callback<string>(er => actualError = er);
            var expectedError = "Шрифт 'NotExistFont' не найден.";      
            
            var failTagMaker = new TagMaker(layouterMock.Object, exiterMock.Object, fontSizeMakerMock.Object, "NotExistFont");

            failTagMaker.MakeTagRectangles(frequencyDict);
            actualError.Should().Be(expectedError);
        }
        [Test]
        public void TagMaker_ShouldReturnTagWithCorrectProperties()
        {          
            fontSizeMakerMock.Setup(x => x.GetFontSizeByFreq(It.IsAny<int>(), It.IsAny<int>()))
                .Returns(80);
            layouterMock.Setup(x => x.PutNextRectangle(It.IsAny<Size>()))
                .Returns((Size size) => new Rectangle(new Point(10, 10), size));

            var actualTag = tagMaker.MakeTagRectangles(frequencyDict).ToList()[0];

            actualTag.Key.Location.Should().Be(new Point(10, 10));
            actualTag.Value.Item1.Should().Be("test");
            actualTag.Value.Item2.Size.Should().Be(80);
        }


        
    }
}