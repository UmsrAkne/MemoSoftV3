using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Prism.Mvvm;

namespace MemoSoftV3.Models
{
    public class Comment : BindableBase, IDatabaseEntity
    {
        private SubComment childSubComment = new ();

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

        [NotMapped]
        public List<Tag> Tags { get; set; } = new ();

        [NotMapped]
        public string GroupName { get; set; } = string.Empty;

        [NotMapped]
        public List<SubComment> SubComments { get; set; } = new ();

        [NotMapped]
        public SubComment ChildSubComment { get => childSubComment; set => SetProperty(ref childSubComment, value); }
    }
}