using System;
using System.ComponentModel.DataAnnotations;

namespace MemoSoftV3.Models
{
    public class Comment
    {
        [Key]
        [Required]
        public int Id { get; set; }

        [Required]
        public string Text { get; set; } = string.Empty;

        [Required]
        public bool IsFavorite { get; set; }

        [Required]
        public DateTime CreationDateTime { get; set; } = DateTime.Now;
    }
}