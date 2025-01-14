﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using TextClassification.Controllers;
using TextClassification.Models.InputModel;

namespace TextClassification.DAL
{
    public class TextClassificationContextInitializer : System.Data.Entity.DropCreateDatabaseIfModelChanges<TextClassificationContext>
    {
        protected override void Seed(TextClassificationContext context)
        {
            List<DocumentInput> documents = new List<DocumentInput>();

            string path = AppDomain.CurrentDomain.GetData("DataDirectory").ToString();
            using (StreamReader r = new StreamReader(path + "/Musical_Instruments_5.json"))
            {
                string json;
                while ((json = r.ReadLine()) != null)
                {
                    dynamic item = JsonConvert.DeserializeObject(json);
                    documents.Add(new DocumentInput { Name = item.reviewerID + item.asin, ClassificationName = item.overall + (item.overall == "1" ? " star" : " stars"), Content = item.reviewText });
                }
            }

            DocumentsController dc = new DocumentsController(context);

            documents.ForEach(d => dc.PostDocument(d));
            context.SaveChanges();
        }
    }
}