using System.Collections.Generic;
using System.Linq;
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
            List<Trigram> trigramsWithIds = TextHelper.LinkTrigramIds(trigrams, db);

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
            //need to rollup multiple trigram occurences to avoid an EF error adding the same ClassificationTrigramOccurence twice before a SaveChange
            //trigramId -> count
            Dictionary<int, int> docVector = docTrigrams
                   .GroupBy(t => t.Sequence)
                   .ToDictionary(t => t.First().Id, t => t.Count());

            foreach (int trigramId in docVector.Keys)
            {
                ClassificationTrigramOccurence cto
                    = db.ClassificationTrigramOccurences.SingleOrDefault(o => o.ClassId == classId && o.TrigramId == trigramId);

                if (cto is null)
                {
                    db.ClassificationTrigramOccurences.Add(new ClassificationTrigramOccurence
                    {
                        ClassId = classId,
                        TrigramId = trigramId,
                        Occurrences = docVector[trigramId]
                    });
                }
                else
                {
                    cto.Occurrences += docVector[trigramId];
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