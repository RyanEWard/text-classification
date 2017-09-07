using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TextClassification.Controllers;
using TextClassification.Models;
using TextClassification.Models.InputModel;

namespace TextClassification.DAL
{
    public class TextClassificationContextInitializer : System.Data.Entity.DropCreateDatabaseIfModelChanges<TextClassificationContext>
    {
        protected override void Seed(TextClassificationContext context)
        {
            List<DocumentInput> documents = new List<DocumentInput>();
            documents.Add(new DocumentInput { Name = "post1", ClassificationName = "learn1", Content = "hello world" });
            documents.Add(new DocumentInput { Name = "post2", ClassificationName = "learn2", Content = "hello world2" });

            DocumentsController dc = new DocumentsController(context);

            documents.ForEach(d => dc.PostDocument(d));
            context.SaveChanges();
        }
    }
}