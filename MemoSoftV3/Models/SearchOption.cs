using System;
using System.Collections.Generic;
using System.Linq;

namespace MemoSoftV3.Models
{
    public class SearchOption
    {
        public SearchOption()
        {
        }

        public SearchOption(SearchCliOption option)
        {
            if (!string.IsNullOrEmpty(option.Text))
            {
                Text = option.Text;
            }

            if (option.Tags.Any(t => !string.IsNullOrEmpty(t)))
            {
                TagTexts = option.Tags.ToList();
            }
        }
        
        public string Text { get; set; } = string.Empty;

        public DateTime StartDateTime { get; set; } = new (0);

        public DateTime EndDateTime { get; set; } = DateTime.MaxValue;

        public List<string> TagTexts { get; set; } = new ();

        public string GroupName { get; set; } = string.Empty;

        public bool IsDefault =>
            string.IsNullOrEmpty(Text)
            && string.IsNullOrEmpty(GroupName)
            && !TagTexts.Any()
            && StartDateTime.Ticks == 0
            && EndDateTime == DateTime.MaxValue;
    }
}