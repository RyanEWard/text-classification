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
            documents.Add(new Document { Name = "post1", Classification = new Classification { Name = "sp-learn" }, Content = "hello world" });
            documents.Add(new Document { Name = "post2", Classification = new Classification { Name = "sp-learn2" }, Content = "hello world2" });

            documents.ForEach(t => context.Documents.Add(t));
            context.SaveChanges();
        }
    }
}