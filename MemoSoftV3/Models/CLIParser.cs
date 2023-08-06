using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using CommandLine;

namespace MemoSoftV3.Models
{
    public class CliParser
    {
        private static string commandPattern = " -(g|c|f|t).*( |$)";

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

        public SearchCliOption ParseSearchCommand(string argString)
        {
            var args = SplitArg(argString);
            var parseResult = Parser.Default.ParseArguments<SearchCliOption>(args);

            var opt = new SearchCliOption();

            switch (parseResult.Tag)
            {
                // パース成功
                case ParserResultType.Parsed:
                    if (parseResult is Parsed<SearchCliOption> parsed)
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
        
        /// <summary>
        ///     入力された文字列がコマンドかどうかを判定します。
        /// </summary>
        /// <param name="commandText">コマンドのテキスト</param>
        /// <returns>コマンドかどうかを返します。</returns>
        public bool IsCommand(string commandText)
        {
            return Regex.IsMatch(commandText, commandPattern);
        }

        public string GetArgsString(string commandText)
        {
            var m = Regex.Match(commandText, commandPattern);
            return commandText.Substring(m.Index + 1); // +1 は冒頭の半角スペースの除外分
        }

        public string GetCommentWithoutArgs(string commandText)
        {
            var m = Regex.Match(commandText, commandPattern);
            return m.Index >= 0 ? commandText[..m.Index] : commandText;
        }
    }
}