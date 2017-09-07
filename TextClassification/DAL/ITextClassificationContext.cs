using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using TextClassification.Models;

namespace TextClassification.DAL
{
    public interface ITextClassificationContext
    {
        DbSet<Document> Documents { get; set; }
        DbSet<Trigram> Trigrams { get; set; }
        DbSet<ClassTrigramOccurence> ClassTrigramOccurences { get; set; }
    }
}