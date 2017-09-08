using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using TextClassification.DAL;
using TextClassification.Models;

namespace TextClassification.Processors
{
    public static class DocumentProcessor
    {
        public static void AddDocument(Document doc, ITextClassificationContext db)
        {
            List<Trigram> docTrigrams = TextHelper.GetDocTrigrams(doc.Content);

            docTrigrams = AddOrGetTrigrams(docTrigrams, db);

            AddOrUpdateClassificationTrigramOccurences(doc.ClassificationId, docTrigrams, db);

            UpdateClassificationCount(doc.ClassificationId, docTrigrams.Count, db);

            db.SaveChanges();
        }

        private static List<Trigram> AddOrGetTrigrams(List<Trigram> trigrams, ITextClassificationContext db)
        {
            //link up existing trigram ids with db
            //double select hack to get EF to assign new ids
            List<Trigram> trigramsWithIds =
                (
                from t1 in trigrams
                join dbTrigram in db.Trigrams on t1.Sequence equals dbTrigram.Sequence into intermediate
                from t2 in intermediate.DefaultIfEmpty()
                select new Trigram { Sequence = t1.Sequence, Id = t2?.Id ?? 0 }
                )
                .Select(t => t.Id == 0 ? new Trigram { Sequence = t.Sequence } : t)
                .ToList();

            //add trigrams with no id to db
            trigramsWithIds
                .Where(t => t.Id == 0)
                .ToList()
                .ForEach(t => db.Trigrams.Add(t));

            db.SaveChanges(); //give ids to those trigrams missing ones

            return trigramsWithIds;
        }

        private static void AddOrUpdateClassificationTrigramOccurences(int classId, List<Trigram> docTrigrams, ITextClassificationContext db)
        {
            foreach (Trigram t in docTrigrams)
            {
                ClassificationTrigramOccurence cto
                    = db.ClassTrigramOccurences.SingleOrDefault(o => o.ClassId == classId && o.TrigramId == t.Id);

                if (cto is null)
                {
                    db.ClassTrigramOccurences.Add(new ClassificationTrigramOccurence
                    {
                        ClassId = classId,
                        TrigramId = t.Id,
                        Occurrences = 1
                    });
                }
                else
                {
                    cto.Occurrences++;
                }
            }
        }

        private static void UpdateClassificationCount(int classificationId, int newTrigramCount, ITextClassificationContext db)
        {
            Classification classification = db.Classifications.Single(c => c.Id == classificationId);

            classification.TrigramOccurences += newTrigramCount;
        }
    }
}