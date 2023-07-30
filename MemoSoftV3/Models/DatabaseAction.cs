using System;
using System.ComponentModel.DataAnnotations;

namespace MemoSoftV3.Models
{
    public class DatabaseAction
    {
        public DatabaseAction()
        {
        }

        public DatabaseAction(Comment comment, Kind kind)
        {
            Target = Target.Comment;
            TargetId = comment.Id;
            Kind = kind;
        }

        public DatabaseAction(SubComment comment, Kind kind)
        {
            Target = Target.SubComment;
            TargetId = comment.Id;
            Kind = kind;
        }
        
        public DatabaseAction(Tag tag, Kind kind)
        {
            Target = Target.Tag;
            TargetId = tag.Id;
            Kind = kind;
        }

        public DatabaseAction(TagMap tagMap, Kind kind)
        {
            Target = Target.TagMap;
            TargetId = tagMap.Id;
            Kind = kind;
        }

        public DatabaseAction(Group group, Kind kind)
        {
            Target = Target.Group;
            TargetId = group.Id;
            Kind = kind;
        }
        
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