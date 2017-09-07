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
        DbSet<ClassificationTrigramOccurence> ClassTrigramOccurences { get; set; }
        DbSet<Classification> Classifications { get; set; }

        int SaveChanges();
    }
}