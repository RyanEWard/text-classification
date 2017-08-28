using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TextClassification.Models
{
    public class Text
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Class { get; set; }
        public string Content { get; set; }
    }
}