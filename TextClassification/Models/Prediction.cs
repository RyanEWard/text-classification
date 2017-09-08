using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TextClassification.Models
{
    public class Prediction
    {
        public string Content;
        public string ClassificationName;
        public int ClassificationId;
        public double ClassificationSimilarityMeasure;
    }
}