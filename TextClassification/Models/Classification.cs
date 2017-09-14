using System.ComponentModel.DataAnnotations;

namespace TextClassification.Models
{
    public class Classification
    {
        [Key]
        public int Id { get; set; }

        public string Name { get; set; }
        public int TrigramOccurences { get; set; }
    }
}