using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MemoSoftV3.Models
{
    public class Tag : IDatabaseEntity
    {
        [Key]
        [Required]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [NotMapped]
        public DateTime DateTime { get; set; } = DateTime.Now;

        public override string ToString()
        {
            return $"#{Name}";
        }
    }
}