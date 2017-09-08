using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using TextClassification.DAL;
using TextClassification.Models;

namespace TextClassification.Processors
{
    public static class TextHelper
    {
        public static List<Trigram> GetDocTrigrams(string text)
        {
            //only a-z, A-Z, 0-9, and space as allowed characters
            string cleanedText = Regex.Replace(text, @"[^a-zA-Z0-9\s]", "");
            //lowercase everything
            cleanedText = cleanedText.ToLower();
            //replace spaces with _
            cleanedText = cleanedText.Replace(" ", "_");


            List<Trigram> docTrigrams = new List<Trigram>();

            for (int i = 0; i < cleanedText.Length - 2; i++)
            {
                docTrigrams.Add(new Trigram { Sequence = cleanedText.Substring(i, 3) });
            }

            return docTrigrams;
        }

        public static List<Trigram> LinkTrigramIds(List<Trigram> trigrams, ITextClassificationContext db)
        {
            //link up existing trigram ids with db
            //double select hack to get EF to assign new ids
            return
                (
                from t1 in trigrams
                join dbTrigram in db.Trigrams on t1.Sequence equals dbTrigram.Sequence into intermediate
                from t2 in intermediate.DefaultIfEmpty()
                select new Trigram { Sequence = t1.Sequence, Id = t2?.Id ?? 0 }
                )
                .Select(t => t.Id == 0 ? new Trigram { Sequence = t.Sequence } : t)
                .ToList();
        }
    }
}