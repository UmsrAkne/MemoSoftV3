using CommandLine;

namespace MemoSoftV3.Models
{
    public class CliParser
    {
        public string[] SplitArg(string arg)
        {
            return arg.Split(" ");
        }

        public void Parse(string argString)
        {
            var args = SplitArg(argString);
            var parseResult = Parser.Default.ParseArguments<CliOption>(args);

            switch (parseResult.Tag)
            {
                // パース成功
                case ParserResultType.Parsed:
                    var parsed = parseResult as Parsed<CliOption>;
                    var opt = parsed.Value;
                    break;

                // パース失敗
                case ParserResultType.NotParsed:
                    // パースの成否でパース結果のオブジェクトの方が変わる
                    var notParsed = parseResult as NotParsed<CliOption>;

                    break;
            }
        }
    }
}