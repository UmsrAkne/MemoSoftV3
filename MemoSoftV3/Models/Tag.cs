using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Prism.Mvvm;

namespace MemoSoftV3.Models
{
    public class Tag : BindableBase, IDatabaseEntity
    {
        private string name;

        [Key]
        [Required]
        public int Id { get; set; }

        [Required]
        public string Name { get => name; set => SetProperty(ref name, value); }
        
        [NotMapped]
        public DateTime DateTime { get; set; } = DateTime.Now;

        [NotMapped]
        public bool Applying { get; set; }

        public override string ToString()
        {
            return $"#{Name}";
        }
    }
}