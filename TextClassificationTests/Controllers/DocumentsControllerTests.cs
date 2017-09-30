using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Web.Http.Results;
using TextClassification.Models;
using TextClassificationTests.DALMock;
using System.Linq;
using TextClassification.Models.InputModel;
using System.Web.Http;

namespace TextClassification.Controllers.Tests
{
    [TestClass()]
    public class DocumentsControllerTests
    {
        [TestMethod()]
        public void GetDocumentTest()
        {
            TestTextClassificationContext db = new TestTextClassificationContext();
            Document testDoc = new Document { Id = 1, ClassificationId = 2, Content = "abc", Name = "test" };
            db.Documents.Add(testDoc);

            DocumentsController docController = new DocumentsController(db);

            var response = docController.GetDocument(1) as OkNegotiatedContentResult<Document>;

            Assert.IsNotNull(response);

            Assert.AreEqual(testDoc.Name, response.Content.Name);
        }

        [TestMethod()]
        public void GetDocumentTest_FailNotFound()
        {
            TestTextClassificationContext db = new TestTextClassificationContext();
            Document testDoc = new Document { Id = 1, ClassificationId = 2, Content = "abc", Name = "test" };
            db.Documents.Add(testDoc);

            DocumentsController docController = new DocumentsController(db);

            IHttpActionResult response = docController.GetDocument(2);

            Assert.IsNotNull(response);

            Assert.IsInstanceOfType(response, typeof(NotFoundResult));
        }

        [TestMethod()]
        public void GetDocumentsTest()
        {
            TestTextClassificationContext db = new TestTextClassificationContext();
            Document testDoc1 = new Document { Id = 1, ClassificationId = 2, Content = "abc", Name = "test" };
            Document testDoc2 = new Document { Id = 2, ClassificationId = 2, Content = "xyz", Name = "test2" };
            db.Documents.Add(testDoc1);
            db.Documents.Add(testDoc2);

            DocumentsController docController = new DocumentsController(db);

            List<Document> response = docController.GetDocuments().ToList();

            Assert.IsNotNull(response);

            Assert.AreEqual(2, response.Count);
            Assert.AreEqual(testDoc1.Content, response[0].Content);
            Assert.AreEqual(testDoc2.Content, response[1].Content);
        }

        [TestMethod()]
        public void PostDocumentTest_Multiple()
        {
            TestTextClassificationContext db = new TestTextClassificationContext();

            DocumentsController docController = new DocumentsController(db);

            DocumentInput docInput1 = new DocumentInput { Name = "test", Content = "hello world", ClassificationName = "test_class" };
            DocumentInput docInput2 = new DocumentInput { Name = "test2", Content = "hello world2", ClassificationId = 1 };
            DocumentInput docInput3 = new DocumentInput { Name = "test3", Content = "rld", ClassificationName = "test_class2" };
            DocumentInput docInput4 = new DocumentInput { Name = "test4", Content = "rld", ClassificationName = "test_class2" };

            var response1 = docController.PostDocument(docInput1) as OkNegotiatedContentResult<Document>;
            var response2 = docController.PostDocument(docInput2) as OkNegotiatedContentResult<Document>;
            var response3 = docController.PostDocument(docInput3) as OkNegotiatedContentResult<Document>;
            var response4 = docController.PostDocument(docInput4) as OkNegotiatedContentResult<Document>;

            // grab trigram Ids corresponding to "o_w" and  "ld2" to check counts later 
            int o_w_Id = db.Trigrams.Single(t => t.Sequence == "o_w").Id;
            int ld2_Id = db.Trigrams.Single(t => t.Sequence == "ld2").Id;

            Assert.IsNotNull(response1);
            Assert.IsNotNull(response2);

            Assert.AreEqual(2, db.Classifications.Count());
            Assert.AreEqual(4, db.Documents.Count());
            Assert.AreEqual(10, db.Trigrams.Count());
            Assert.AreEqual(11, db.ClassificationTrigramOccurences.Count());

            Assert.AreEqual(2, db.ClassificationTrigramOccurences.Single(cto => cto.TrigramId == o_w_Id).Occurrences);
            Assert.AreEqual(1, db.ClassificationTrigramOccurences.Single(cto => cto.TrigramId == ld2_Id).Occurrences);
        }

        [TestMethod()]
        public void PostDocumentTest_FailNoClassification()
        {
            TestTextClassificationContext db = new TestTextClassificationContext();

            DocumentsController docController = new DocumentsController(db);

            DocumentInput docInput = new DocumentInput { Name = "test", Content = "hello world" };

            IHttpActionResult response = docController.PostDocument(docInput);

            Assert.IsNotNull(response);
            Assert.IsInstanceOfType(response, typeof(BadRequestErrorMessageResult));

            BadRequestErrorMessageResult errorMessage = response as BadRequestErrorMessageResult;

            Assert.AreEqual("ClassificationId or ClassificationName must be specified.", errorMessage.Message);
        }

        [TestMethod()]
        public void PostDocumentTest_FailClassificationIdNotFound()
        {
            TestTextClassificationContext db = new TestTextClassificationContext();

            DocumentsController docController = new DocumentsController(db);

            DocumentInput docInput = new DocumentInput { Name = "test", Content = "hello world", ClassificationId = 1 };

            IHttpActionResult response = docController.PostDocument(docInput);

            Assert.IsNotNull(response);
            Assert.IsInstanceOfType(response, typeof(BadRequestErrorMessageResult));

            BadRequestErrorMessageResult errorMessage = response as BadRequestErrorMessageResult;

            Assert.AreEqual("ClassificationId '1' not found.", errorMessage.Message);
        }
    }
}