using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TextClassification.Models.InputModel
{
    public class DocumentInput
    {
        public string Name;
        public string Content;

        public int ClassificationId;
        public string ClassificationName;
    }
}