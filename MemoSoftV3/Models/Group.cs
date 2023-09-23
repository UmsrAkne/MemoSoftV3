using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Prism.Mvvm;

namespace MemoSoftV3.Models
{
    public class Group : BindableBase, IDatabaseEntity
    {
        private bool isSmartGroup;

        [Key]
        [Required]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public bool IsSmartGroup { get => isSmartGroup; set => SetProperty(ref isSmartGroup, value); }

        [Required]
        public string Command { get; set; } = string.Empty;

        [Required]
        public bool IsArchive { get; set; } = false;

        [NotMapped]
        public DateTime DateTime { get; set; } = DateTime.Now;

        [NotMapped]
        public bool CanChangeToSmartGroup { get; set; }
    }
}