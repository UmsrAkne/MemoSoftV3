using System.Collections.Generic;
using CommandLine;

namespace MemoSoftV3.Models
{
    public class CliOption
    {
        [Option(
                'g',
                "group",
                Required = false,
                HelpText = "コメントを追加するグループを指定します。"),
        ]
        public string GroupName { get; set; }

        [Option(
                'c',
                "checkable",
                Default = true,
                Required = false,
                HelpText = "チェック可能なコメントを生成します。"),
        ]
        public bool Checkable { get; set; }

        [Option(
                'f',
                "favorite",
                Default = true,
                Required = false,
                HelpText = "お気に入りのコメントを生成します。"),
        ]
        public bool IsFavorite { get; set; }

        [Option(
                't',
                "tag",
                Required = false,
                Separator = ',',
                HelpText = "タグを追加します。カンマで区切ることで、複数のタグを追加可能です。"),
        ]
        public IEnumerable<string> Tags { get; set; }
    }
}