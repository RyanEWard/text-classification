using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TextClassification.Models
{
    public class Document
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public int ClassificationId { get; set; }
        public string Content { get; set; }

        public virtual Classification Classification { get; set; }
    }
}