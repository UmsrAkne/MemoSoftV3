using System;

namespace MemoSoftV3.Models
{
    public class GroupSearchOption
    {
        public string Name { get; set; } = string.Empty;

        public DateTime StartDateTime { get; set; } = new (0);

        public DateTime EndDateTime { get; set; } = DateTime.MaxValue;

        public bool ContainsArchivedGroup { get; set; } = false;
    }
}