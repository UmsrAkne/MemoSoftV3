using System;
using System.Collections.Generic;

namespace MemoSoftV3.Models
{
    public class SearchOption
    {
        public string Text { get; set; } = string.Empty;

        public DateTime StartDateTime { get; set; } = new (0);

        public DateTime EndDateTime { get; set; } = DateTime.MaxValue;

        public List<string> TagTexts { get; set; } = new ();

        public string GroupName { get; set; } = string.Empty;
    }
}