using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MemoSoftV3.Models
{
    public class Comment
    {
        [Key]
        [Required]
        public int Id { get; set; }

        [Required]
        public int GroupId { get; set; }

        [Required]
        public string Text { get; set; } = string.Empty;

        [Required]
        public bool IsFavorite { get; set; }

        [Required]
        public bool IsCheckable { get; set; }

        [Required]
        public bool Checked { get; set; }

        [NotMapped]
        public DateTime DateTime { get; set; } = DateTime.Now;
    }
}