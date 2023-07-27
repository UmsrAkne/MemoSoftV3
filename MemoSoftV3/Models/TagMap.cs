using System.ComponentModel.DataAnnotations;

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
    }
}