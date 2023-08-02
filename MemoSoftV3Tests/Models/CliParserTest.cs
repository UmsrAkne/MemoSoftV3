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
            var args = "-t \"aaa bbb\",ccc -g testGroup -f ";
            var a = parser.SplitArg(args).ToList();

            Assert.Multiple(() =>
            {
                Assert.That(a[0], Is.EqualTo("-t"));
                Assert.That(a[1], Is.EqualTo("aaa bbb"));
                Assert.That(a[2], Is.EqualTo(",ccc"));
                Assert.That(a[3], Is.EqualTo("-g"));
                Assert.That(a[4], Is.EqualTo("testGroup"));
                Assert.That(a[5], Is.EqualTo("-f"));
            });
        }

        [Test]
        public void ParseTest()
        {
            var parser = new CliParser();
            var args = "-t \"aaa bbb\",ccc -g \"test Group\" -f ";
            var a = parser.Parse(args);

            Assert.AreEqual("aaa bbb", a.Tags.ToList()[0]);
            Assert.AreEqual("ccc", a.Tags.ToList()[1]);
            Assert.AreEqual("test Group", a.GroupName);
        }
    }
}