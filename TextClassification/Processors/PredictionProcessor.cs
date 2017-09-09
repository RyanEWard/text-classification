using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TextClassification.DAL;
using TextClassification.Models;
using TextClassification.Models.InputModel;

namespace TextClassification.Processors
{
    public static class PredictionProcessor
    {

        public static Prediction GetPrediction(PredictionInput input, ITextClassificationContext db)
        {
            List<Trigram> inputTrigrams = TextHelper.GetDocTrigrams(input.Content);

            //trigramId -> count
            Dictionary<int, int> inputVector = ConvertTrigramsToVector(inputTrigrams, db);

            //classificationId -> (trigramId -> count)
            Dictionary<int, Dictionary<int, int>> classificationVectors = GetClassificationVectors(db);

            Prediction predictionResult = FindMostSimilarClassification(inputVector, classificationVectors);

            FinishWritingPrediction(predictionResult, input.Content, db);

            return predictionResult;
        }

        private static Dictionary<int, int> ConvertTrigramsToVector(List<Trigram> inputTrigrams, ITextClassificationContext db)
        {
            inputTrigrams = TextHelper.LinkTrigramIds(inputTrigrams, db);

            return inputTrigrams
                   .GroupBy(t => t.Sequence)
                   .ToDictionary(t => t.First().Id, t => t.Count());
        }

        private static Dictionary<int, Dictionary<int, int>> GetClassificationVectors(ITextClassificationContext db)
        {
            //classificationId -> (trigramId -> count)
            Dictionary<int, Dictionary<int, int>> classificationVectors = new Dictionary<int, Dictionary<int, int>>();

            foreach (Classification c in db.Classifications.ToList())
            {
                classificationVectors[c.Id] = db.ClassTrigramOccurences
                                                .Where(ct => ct.ClassId == c.Id)
                                                .ToDictionary(ct => ct.TrigramId, ct => ct.Occurrences);

            }

            return classificationVectors;
        }

        private static Prediction FindMostSimilarClassification(Dictionary<int, int> inputVector, Dictionary<int, Dictionary<int, int>> classificationVectors)
        {
            //classificationId -> similarity
            Dictionary<int, double> similarity = new Dictionary<int, double>();

            foreach (int classId in classificationVectors.Keys)
            {
                similarity[classId] = 1 - 2 * Math.Acos(CosineSimilarity(inputVector, classificationVectors[classId])) / Math.PI;
            }

            Prediction p = new Prediction();
            double maxSimilarity = Double.MinValue;

            foreach (int classId in similarity.Keys)
            {
                if (maxSimilarity < similarity[classId])
                {
                    maxSimilarity = similarity[classId];
                    p.ClassificationId = classId;
                    p.ClassificationSimilarityMeasure = similarity[classId];
                }
            }

            return p;
        }

        private static double CosineSimilarity(Dictionary<int, int> inputVector, Dictionary<int, int> classificationVector)
        {
            double dotProduct = 0;
            double squaredSumInput = 0;
            double squaredSumClassification = 0;

            foreach (int trigramId in inputVector.Keys)
            {
                if (classificationVector.Keys.Contains(trigramId))
                {
                    dotProduct += inputVector[trigramId] * (long)classificationVector[trigramId];
                }

                squaredSumInput += Math.Pow(inputVector[trigramId], 2);
            }

            foreach (int trigramId in classificationVector.Keys)
            {
                squaredSumClassification += Math.Pow(classificationVector[trigramId], 2);
            }

            return dotProduct / (Math.Sqrt(squaredSumInput) * Math.Sqrt(squaredSumClassification));
        }

        private static void FinishWritingPrediction(Prediction predictionResult, String inputContent, ITextClassificationContext db)
        {
            predictionResult.Content = inputContent;

            predictionResult.ClassificationName = db.Classifications.Single(c => c.Id == predictionResult.ClassificationId).Name;
        }
    }
}