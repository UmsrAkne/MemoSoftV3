using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MemoSoftV3.Models
{
    public class SubComment
    {
        [Key]
        [Required]
        public int Id { get; set; }

        [Required]
        public int ParentCommentId { get; set; }

        [Required]
        public string Text { get; set; } = string.Empty;

        [Required]
        public bool IsCheckable { get; set; }

        [Required]
        public bool Checked { get; set; }

        [Required]
        public bool TimeTracking { get; set; }

        [NotMapped]
        public TimeSpan WorkingTimeSpan { get; set; } = TimeSpan.Zero;

        [NotMapped]
        public DateTime DateTime { get; set; } = DateTime.Now;
    }
}