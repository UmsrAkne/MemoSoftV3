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

        [Test]
        public void IsCommandTest()
        {
            var parser = new CliParser();
            Assert.Multiple(() =>
            {
                Assert.That(parser.IsCommand("text -t test"), Is.True);
                Assert.That(parser.IsCommand("text -tag test"), Is.True);
                Assert.That(parser.IsCommand("text -g test"), Is.True);
                Assert.That(parser.IsCommand("text -group test"), Is.True);
                Assert.That(parser.IsCommand("text -group test"), Is.True);
                Assert.That(parser.IsCommand("text -f"), Is.True);
                Assert.That(parser.IsCommand("text -favorite"), Is.True);

                Assert.That(parser.IsCommand("text"), Is.False);
                Assert.That(parser.IsCommand("text -"), Is.False);
                Assert.That(parser.IsCommand(string.Empty), Is.False);
            });
        }

        [Test]
        public void GetArgsStringTest()
        {
            var parser = new CliParser();
            Assert.Multiple(() =>
            {
                Assert.That(parser.GetArgsString("text -t test"), Is.EqualTo("-t test"));
                Assert.That(parser.GetArgsString("text -tag test"), Is.EqualTo("-tag test"));
            });
        }

        [Test]
        public void SearchCliParserTest()
        {
            var parser = new CliParser();
            var option = parser.ParseSearchCommand("search --text test -g testGroup -t testTag,tag2");

            Assert.That(option.Text, Is.EqualTo("test"));
            Assert.That(option.GroupNames.ToList()[0], Is.EqualTo("testGroup"));
            Assert.That(option.Tags.ToList()[0], Is.EqualTo("testTag"));
            Assert.That(option.Tags.ToList()[1], Is.EqualTo("tag2"));
        }
    }
}