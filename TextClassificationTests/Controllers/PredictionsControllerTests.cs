using Microsoft.VisualStudio.TestTools.UnitTesting;
using TextClassificationTests.DALMock;
using TextClassification.Models.InputModel;
using TextClassification.Models;
using System.Web.Http.Results;
using System.Web.Http;

namespace TextClassification.Controllers.Tests
{
    [TestClass()]
    public class PredictionsControllerTests
    {
        [TestMethod()]
        public void PostPredictionTest()
        {
            TestTextClassificationContext db = SetupTestDocumentScenario();

            PredictionsController predictionControllter = new PredictionsController(db);

            PredictionInput input = new PredictionInput { Content = "hello world2" };

            var response = predictionControllter.PostPrediction(input) as OkNegotiatedContentResult<Prediction>;

            Assert.IsNotNull(response);

            Assert.AreEqual("test_class2", response.Content.ClassificationName);
            Assert.AreEqual(1, response.Content.ClassificationSimilarityMeasure, .001);
        }

        [TestMethod()]
        public void PostPredicitionTest_FailNoContent()
        {
            TestTextClassificationContext db = new TestTextClassificationContext();

            PredictionsController predictionControllter = new PredictionsController(db);

            IHttpActionResult response = predictionControllter.PostPrediction(new PredictionInput { });

            Assert.IsNotNull(response);

            Assert.IsInstanceOfType(response, typeof(BadRequestErrorMessageResult));

            BadRequestErrorMessageResult errorMessage = response as BadRequestErrorMessageResult;

            Assert.AreEqual("No content provided.", errorMessage.Message);
        }

        private TestTextClassificationContext SetupTestDocumentScenario()
        {
            TestTextClassificationContext db = new TestTextClassificationContext();

            DocumentsController docController = new DocumentsController(db);

            DocumentInput docInput1 = new DocumentInput { Name = "test", Content = "hello world", ClassificationName = "test_class" };
            DocumentInput docInput2 = new DocumentInput { Name = "test2", Content = "hello world2", ClassificationName = "test_class2" };

            docController.PostDocument(docInput1);
            docController.PostDocument(docInput2);

            return db;
        }
    }
}