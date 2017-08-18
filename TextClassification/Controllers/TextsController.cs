using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using TextClassification.Models;

namespace TextClassification.Controllers
{
    public class TextsController : ApiController
    {
        Text[] texts = new Text[]
        {
            new Text { Id = 1, Name = "post1", Class = "sp-learn", Content = "hello world"},
            new Text { Id = 2, Name = "post2", Class = "sp-learn2", Content = "hello world2"}
        };


        public IEnumerable<Text> GetTexts()
        {
            return texts;
        }

        public IHttpActionResult GetText(int id)
        {
            var text = texts.FirstOrDefault((p) => p.Id == id);
            if (text == null)
            {
                return NotFound();
            }
            return Ok(text);
        }
    }
}
