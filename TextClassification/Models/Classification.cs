using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

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