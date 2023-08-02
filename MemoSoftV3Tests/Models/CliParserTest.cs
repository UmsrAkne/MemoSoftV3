using MemoSoftV3.Models;

namespace MemoSoftV3Tests.Models
{
    [TestFixture]
    public class CliParserTest
    {
        [Test]
        public void SplitArgTest()
        {
            var parser = new CliParser();
            var args = "-t aaa,bbb,ccc -g testGroup -f ";
            var a = parser.SplitArg(args);
        }
    }
}