using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using TextClassification.DAL;
using TextClassification.Models;
using TextClassification.Models.InputModel;

namespace TextClassification.Controllers
{
    public class DocumentsController : ApiController
    {
        private ITextClassificationContext db = new TextClassificationContext();

        public DocumentsController() { }

        public DocumentsController(ITextClassificationContext context)
        {
            db = context;
        }

        public IEnumerable<Document> GetDocuments()
        {
            return db.Documents.Include("Classification");
        }

        public IHttpActionResult GetDocument(int id)
        {
            Document document = db.Documents.Include("Classification").Single(d => d.Id == id);

            if (document == null)
            {
                return NotFound();
            }
            return Ok(document);
        }

        public IHttpActionResult PostDocument(DocumentInput docInput)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Invalid input structure.");
            }

            if (docInput.ClassificationId == 0
                && (docInput.ClassificationName is null || docInput.ClassificationName == ""))
            {
                return BadRequest("ClassificationId or ClassificationName must be specified");
            }

            Document docToAdd = new Document();
            docToAdd.Name = docInput.Name;
            docToAdd.Content = docInput.Content;

            if (docInput.ClassificationId != 0)
            {
                Classification classification = db.Classifications.SingleOrDefault(c => c.Id == docInput.ClassificationId);
                if (classification is null)
                {
                    return BadRequest("ClassificationId '" + docInput.ClassificationId + "' not found.");
                }

                docToAdd.ClassificationId = classification.Id;
            }
            else
            {
                Classification classification = db.Classifications.SingleOrDefault(c => c.Name == docInput.ClassificationName);

                if (classification is null)
                {
                    docToAdd.Classification = new Classification { Name = docInput.ClassificationName };
                }
                else
                {
                    docToAdd.ClassificationId = classification.Id;
                }
            }

            db.Documents.Add(docToAdd);
            db.SaveChanges();

            Document document = db.Documents.Include("Classification").Single(d => d.Id == docToAdd.Id);

            return Ok(document);
        }
    }
}
