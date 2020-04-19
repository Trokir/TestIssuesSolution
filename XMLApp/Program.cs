using System;
using System.Collections.Generic;

namespace XMLApp
{
    class Program
    {

        static void Main(string[] args)
        {
            var path = @"test.xml";
            var dict = new Dictionary<string, string>
            {
                 { "k1","Alice"},
                 {"k2","Bob"}
            };
            var fileOp = new FileTransform(dict);
            fileOp.LoadDocument(path);
            fileOp.TransformTemplateNodes();
            fileOp.SaveXmlFile();
            Console.ReadLine();
        }
    }
}
