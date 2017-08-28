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
    public class TextsController : ApiController
    {
        private TextContext db = new TextContext();

        public IEnumerable<Text> GetTexts()
        {
            return db.Texts;
        }

        public IHttpActionResult GetText(int id)
        {
            Text text = db.Texts.Single(t => t.ID == id);

            if (text == null)
            {
                return NotFound();
            }
            return Ok(text);
        }
    }
}
