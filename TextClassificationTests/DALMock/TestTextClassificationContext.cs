using System.Data.Entity;
using TextClassification.DAL;
using TextClassification.Models;

namespace TextClassificationTests.DALMock
{
    class TestTextClassificationContext : ITextClassificationContext
    {

        public DbSet<Document> Documents { get; set; }
        public DbSet<Trigram> Trigrams { get; set; }
        public DbSet<ClassificationTrigramOccurence> ClassificationTrigramOccurences { get; set; }
        public DbSet<Classification> Classifications { get; set; }

        public TestTextClassificationContext()
        {
            Documents = new TestDbSetDocument<Document>(this);
            Trigrams = new TestDbSetAutoId<Trigram>();
            ClassificationTrigramOccurences = new TestDbSet<ClassificationTrigramOccurence>();
            Classifications = new TestDbSetAutoId<Classification>();
        }

        // mocking saving EF changes to database
        public int SaveChanges()
        {
            return 0;
        }
    }
}
