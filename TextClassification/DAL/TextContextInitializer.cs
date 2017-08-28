using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TextClassification.Models;

namespace TextClassification.DAL
{
    public class TextContextInitializer : System.Data.Entity.DropCreateDatabaseIfModelChanges<TextContext>
    {
        protected override void Seed(TextContext context)
        {
            List<Text> texts = new List<Text>();
            texts.Add(new Text { ID = 1, Name = "post1", Class = "sp-learn", Content = "hello world" });
            texts.Add(new Text { ID = 2, Name = "post2", Class = "sp-learn2", Content = "hello world2" });

            texts.ForEach(t => context.Texts.Add(t));
            context.SaveChanges();
        }
    }
}