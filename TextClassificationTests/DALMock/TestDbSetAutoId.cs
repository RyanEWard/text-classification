using TextClassification.Models;

// mocks the auto increasing Id handled by the entity framework

namespace TextClassificationTests.DALMock
{
    class TestDbSetAutoId<T> : TestDbSet<T>
        where T : class, IAutoIncrementId
    {
        int currentId = 1;

        public override T Add(T item)
        {
            item.Id = currentId;
            currentId++;

            return base.Add(item);
        }
    }
}
