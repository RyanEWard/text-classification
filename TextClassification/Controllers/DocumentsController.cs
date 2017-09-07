using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using TextClassification.DAL;
using TextClassification.Models;

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
            return db.Documents.Include("Class");
        }

        public IHttpActionResult GetDocument(int id)
        {
            Document document = db.Documents.Include("Class").Single(t => t.Id == id);

            if (document == null)
            {
                return NotFound();
            }
            return Ok(document);
        }
    }
}
