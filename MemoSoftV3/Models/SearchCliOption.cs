using System.Collections.Generic;
using CommandLine;

namespace MemoSoftV3.Models
{
    public class SearchCliOption
    {
        [Option(
                        'x',
                        "text",
                        Required = false,
                        HelpText = "コメントのテキストによる検索を行います。"),
        ]
        public string Text { get; set; } = string.Empty;

        [Option(
                'c',
                "checkable",
                Required = false,
                HelpText = "true にすると、チェック可能なコメントのみを検索します。"),
        ]
        public bool Checkable { get; set; }

        [Option(
                'f',
                "favorite",
                Required = false,
                HelpText = "true にすると、お気に入りのコメントのみを検索します。"),
        ]
        public bool IsFavorite { get; set; }

        [Option(
                'g',
                "group",
                Required = false,
                Separator = ',',
                HelpText = "グループ名による検索を行います。"),
        ]
        public IEnumerable<string> GroupNames { get; set; } = new List<string>();

        [Option(
                't',
                "tag",
                Required = false,
                Separator = ',',
                HelpText = "タグによる検索を行います。"),
        ]
        public IEnumerable<string> Tags { get; set; } = new List<string>();
    }
}