using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TextClassification.Models
{
    public interface IAutoIncrementId
    {
        int Id { get; set; }
    }
}