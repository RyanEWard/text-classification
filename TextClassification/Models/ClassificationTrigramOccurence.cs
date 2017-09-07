using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace TextClassification.Models
{
    public class ClassificationTrigramOccurence
    {
        [Key]
        [Column(Order = 1)]
        public int ClassId { get; set; }
        [Key]
        [Column(Order = 2)]
        public int TrigramId { get; set; }

        public virtual Classification Class { get; set; }
        public virtual Trigram Trigram { get; set; }

        public int Occurrences { get; set; }
    }
}