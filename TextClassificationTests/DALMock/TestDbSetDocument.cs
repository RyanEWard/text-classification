using TextClassification.DAL;
using TextClassification.Models;

// mocks the auto Classification add handled by the entity framework

namespace TextClassificationTests.DALMock
{
    class TestDbSetDocument<T> : TestDbSetAutoId<T>
        where T : Document
    {
        private ITextClassificationContext db;

        public TestDbSetDocument(ITextClassificationContext db)
        {
            this.db = db;
        }

        public override T Add(T item)
        {
            if (item.Classification != null)
            {
                db.Classifications.Add(item.Classification);
                item.ClassificationId = item.Classification.Id;
            }

            return base.Add(item);
        }
    }
}
