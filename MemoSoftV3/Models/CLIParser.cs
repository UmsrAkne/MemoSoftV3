using System.Collections.Generic;
using System.Linq;
using CommandLine;

namespace MemoSoftV3.Models
{
    public class CliParser
    {
        public List<string> SplitArg(string arg)
        {
            var sp1 = arg.Split('"');
            List<string> sps = new ();
            for (var i = 0; i < sp1.Length; i++)
            {
                if (i % 2 == 1)
                {
                    sps.Add(sp1[i]);
                }
                else
                {
                    sps.AddRange(sp1[i].Split(" "));
                }
            }

            sps = sps.Where(s => !string.IsNullOrEmpty(s)).ToList();

            return sps;
        }

        public CliOption Parse(string argString)
        {
            var args = SplitArg(argString);
            var parseResult = Parser.Default.ParseArguments<CliOption>(args);

            var opt = new CliOption();

            switch (parseResult.Tag)
            {
                // パース成功
                case ParserResultType.Parsed:
                    if (parseResult is Parsed<CliOption> parsed)
                    {
                        opt = parsed.Value;
                        opt.Tags = opt.Tags.Select(s => s.Replace(",", string.Empty));
                    }

                    break;

                // パース失敗
                case ParserResultType.NotParsed:
                    break;
            }

            return opt;
        }
    }
}