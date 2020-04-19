using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;

namespace JSONApp
{
    class Program
    {
        private static string Handle(string jsonIn)
        {
            if (string.IsNullOrWhiteSpace(jsonIn))
                throw new ArgumentException("Value cannot be null or whitespace.", nameof(jsonIn));
            var resultJsonString = string.Empty;
            try
            {
                IEnumerable<JsonSample> jsonSamplesCollection = JsonConvert
                    .DeserializeObject<IEnumerable<JsonSample>>(jsonIn, new JsonSerializerSettings
                {
                    MissingMemberHandling = MissingMemberHandling.Error
                });

                if (jsonSamplesCollection == null) return string.Empty;

                var samplesCollection = jsonSamplesCollection.ToList();
                var resultObject = new ModifySampleResult
                {
                    SumIdResult = samplesCollection.Sum(x => x.Id),
                    StringsValues = samplesCollection.Select(s => s.StringText).ToList()
                };
                try
                {
                     resultJsonString = JsonConvert.SerializeObject(resultObject, Formatting.Indented, new JsonSerializerSettings
                     {
                         NullValueHandling = NullValueHandling.Ignore
                     });
                    Console.WriteLine(resultJsonString);
                }
                catch (JsonSerializationException ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
            catch (JsonSerializationException ex)
            {
                Console.WriteLine(ex.Message);
            }
            return resultJsonString;

        }
        static void Main(string[] args)
        {
            Handle(@"[{'i':1, s:'string1'},
                            {'i':2, s:'string2'},
                            {'i':3, s:'string3'},
                            {'i':4, s:'string4'},
                            {'i':5, s:'string5'},
                            {'i':6, s:'string6'},
                            {'i':7, s:'string7'},
                            {'i':8, s:'string8'}]");


            Console.ReadLine();
        }
    }
}
