using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Prism.Mvvm;

namespace MemoSoftV3.Models
{
    public class SubComment : BindableBase, IDatabaseEntity
    {
        private TimeSpan workingTimeSpan = TimeSpan.Zero;
        private bool timeTracking;

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
        public bool TimeTracking { get => timeTracking; set => SetProperty(ref timeTracking, value); }

        [NotMapped]
        public TimeSpan WorkingTimeSpan { get => workingTimeSpan; set => SetProperty(ref workingTimeSpan, value); }

        [NotMapped]
        public DateTime DateTime { get; set; } = DateTime.Now;
    }
}