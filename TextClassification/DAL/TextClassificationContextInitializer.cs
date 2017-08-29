using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TextClassification.Models;

namespace TextClassification.DAL
{
    public class TextClassificationContextInitializer : System.Data.Entity.DropCreateDatabaseIfModelChanges<TextClassificationContext>
    {
        protected override void Seed(TextClassificationContext context)
        {
            List<Document> documents = new List<Document>();
            documents.Add(new Document { ID = 1, Name = "post1", Class = "sp-learn", Content = "hello world" });
            documents.Add(new Document { ID = 2, Name = "post2", Class = "sp-learn2", Content = "hello world2" });

            documents.ForEach(t => context.Documents.Add(t));
            context.SaveChanges();
        }
    }
}