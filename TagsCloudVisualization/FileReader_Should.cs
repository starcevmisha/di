using System.IO;
using System.Linq;
using FluentAssertions;
using NUnit.Framework;
using Moq;
using System.Text.RegularExpressions;
namespace TagsCloudVisualization
{
    [TestFixture]
    public class FileReader_Should
    {
        private Mock<IExiter> exiter;
        [SetUp]
        public void SetUp()
        {
            exiter = new Mock<IExiter>();
        }

        [Test]
        public void Fail_WhenFileDoesNotExist()
        {
            var actualError = "";
            
            var filename = "doesnot.exist";
            var path = Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, filename);
            
            var expected = new Regex("Файл .* не найден.");
            
            exiter.Setup(x => x.ExitWithError(It.IsAny<string>()))
                .Callback<string>(er => actualError = er);
            new FileReader(path, exiter.Object).ReadWords().FirstOrDefault();
            
            expected.IsMatch(actualError).Should().BeTrue();
        }
    }
}