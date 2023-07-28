using System;
using System.ComponentModel.DataAnnotations;

namespace MemoSoftV3.Models
{
    public class DatabaseAction
    {
        [Key]
        [Required]
        public int Id { get; set; }

        [Required]
        public DateTime DateTime { get; set; } = DateTime.Now;

        [Required]
        public Kind Kind { get; set; }

        [Required]
        public Target Target { get; set; }

        [Required]
        public int TargetId { get; set; }
    }
}