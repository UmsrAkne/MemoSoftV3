using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MemoSoftV3.Models
{
    public class TagMap
    {
        [Key]
        [Required]
        public int Id { get; set; }

        [Required]
        public int TagId { get; set; }

        [Required]
        public int CommentId { get; set; }

        [NotMapped]
        public DateTime DateTime { get; set; } = DateTime.Now;
    }
}