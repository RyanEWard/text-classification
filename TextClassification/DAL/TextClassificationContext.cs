using TextClassification.Models;
using System.Data.Entity;

namespace TextClassification.DAL
{
    public class TextClassificationContext : DbContext, ITextClassificationContext
    {
        public TextClassificationContext() : base("TextClassificationContext")
        {
        }

        public DbSet<Document> Documents { get; set; }
        public DbSet<Trigram> Trigrams { get; set; }
        public DbSet<ClassificationTrigramOccurence> ClassificationTrigramOccurences { get; set; }
        public DbSet<Classification> Classifications { get; set; }
    }
}