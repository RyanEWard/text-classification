using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TextClassification.Models;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;

namespace TextClassification.DAL
{
    public class TextClassificationContext : DbContext, ITextClassificationContext
    {
        public TextClassificationContext() : base("TextClassificationContext")
        {
        }

        public DbSet<Document> Documents { get; set; }
        public DbSet<Trigram> Trigrams { get; set; }
        public DbSet<ClassificationTrigramOccurence> ClassTrigramOccurences { get; set; }
        public DbSet<Classification> Classifications { get; set; }
    }
}