# text-classification
Text classification RESTful service using character trigrams in C#. Made as a way for me to get a handle on ASP.NET Web API and Entity Framework.

# Examples

```
POST /api/predictions
{
   "Content": "Far and away the best instrument I have ever used. I love it."
}
```
```
HTTP/1.1 200 OK
{
   "Content": "Far and away the best instrument I have ever used. I love it.",
   "ClassificationId": 1,
   "ClassificationName": "5 stars",
   "ClassificationSimilarityMeasure": 0.4588000554713737
}
```


```
POST /api/predictions
{
   "Content": "This is the worst product ever. My instrument broke on the first day when I was trying to tune it."
}
```

```
HTTP/1.1 200 OK
{
   "Content": "This is the worst product ever. My instrument broke on the first day when I was trying to tune it.",
   "ClassificationId": 4,
   "ClassificationName": "2 stars",
   "ClassificationSimilarityMeasure": 0.5411416242020246
}
```

```
GET /api/documents/123
```

```
HTTP/1.1 200 OK
{
   "Id": 123,
   "Name": "A4BTCECGQAIUIB000068O3X",
   "ClassificationId": 3,
   "Content": "This L Jack is good enough to make connections between my guitar pedals and it works fine. Flexible enough. I don't like only the fact that it's made out of plastic and it looks cheap, but for the price i paid for it i don't really care.",
   "Classification":    {
      "Id": 3,
      "Name": "4 stars",
      "TrigramOccurences": 1691901
   }
}
```

# Setup

Required to add DBConnectionString.config in the TextClassification folder with the following information:

```
<?xml version="1.0" encoding="utf-8" ?>
<connectionStrings>
  <add name="TextClassificationContext" connectionString="MY_CONNECTION_STRING" providerName="MY_PROVIDER" />
</connectionStrings>
```

This specified DB server will be used, with the database "text_classification" being created by the entity framework. On first use, the database is filled with ~10k Amazon reviews about musical instruments classified by their star rating (http://jmcauley.ucsd.edu/data/amazon/).

# Known Issues

- POSTing documents takes much longer than it logically should, causing the initial database setup to take over 30 minutes. I will have to do a deep dive into what I suspect are some inefficient queries caused by some LINQ or EF interactions I'm not aware of.

- The ClassificationSimiliarityMeasures are close together for all classes no matter the prediction text, causing the prediction to be more along the lines of "this is English" rather than "this is class A". However, the predictions are still OK with the expected result sticking out. I need to research how to separate out the commonalities between classes better so that the similiarity measure is more meaningful.

