using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using TextClassification.DAL;
using TextClassification.Models;
using TextClassification.Models.InputModel;
using TextClassification.Processors;

namespace TextClassification.Controllers
{
    public class PredictionsController : ApiController
    {
        private ITextClassificationContext db = new TextClassificationContext();

        public PredictionsController() { }

        public PredictionsController(ITextClassificationContext context)
        {
            db = context;
        }

        public IHttpActionResult PostPrediction(PredictionInput predictionInput)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Invalid input structure.");
            }

            if (predictionInput.Content is null || predictionInput.Content == String.Empty)
            {
                return BadRequest("No content provided.");
            }

            Prediction p = PredictionProcessor.GetPrediction(predictionInput, db);

            return Ok(p);
        }
    }
}
