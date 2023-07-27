using System.ComponentModel.DataAnnotations;

namespace MemoSoftV3.Models
{
    public class Tag
    {
        [Key]
        [Required]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }
    }
}