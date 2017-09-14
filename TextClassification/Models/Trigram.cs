using System.ComponentModel.DataAnnotations;

namespace TextClassification.Models
{
    public class Trigram
    {
        [Key]
        public int Id { get; set; }

        [StringLength(3)]
        public string Sequence { get; set; }
    }
}