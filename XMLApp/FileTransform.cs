using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;

namespace XMLApp
{
    public class FileTransform : IFileOperations
    {
        private readonly Dictionary<string, string> _dict;
        private XDocument _xmlFile;


        /// <summary>
        /// Initializes a new instance of the <see cref="FileTransform"/> class.
        /// </summary>
        /// <param name="dict">The dictionary.</param>
        /// <exception cref="System.ArgumentNullException">dict</exception>
        /// <exception cref="System.ArgumentException">Value cannot be an empty collection. - dict</exception>
        public FileTransform(Dictionary<string, string> dict)
        {
            if (dict is null) throw new ArgumentNullException(nameof(dict));
            if (dict.Count == 0) throw new ArgumentException("Value cannot be an empty collection.", nameof(dict));
            _dict = dict;
        }

        /// <summary>
        /// Loads the document.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <returns></returns>
        /// <exception cref="System.ArgumentNullException">
        /// path
        /// or
        /// _xmlFile
        /// </exception>
        public XDocument LoadDocument(string path)
        {
            if (path is null) throw new ArgumentNullException(nameof(path));
            if (File.Exists(path))
                _xmlFile = XDocument.Load(path);
            if (_xmlFile is null) throw new ArgumentNullException(nameof(_xmlFile));
            return _xmlFile;
        }


        /// <summary>
        /// Saves the XML file.
        /// </summary>
        public void SaveXmlFile()
        {
            _xmlFile.Save("test.out.xml");
        }



        /// <summary>
        /// Transforms the template nodes.
        /// </summary>
        /// <exception cref="System.InvalidOperationException">
        /// </exception>
        public void TransformTemplateNodes()
        {
            var nodes = _xmlFile.Descendants("template").ToList();
            foreach (var element in nodes)
            {
                foreach (var child in element.Elements().ToList())
                {
                    var nameAttribute = child.Attribute("name");
                    if (nameAttribute is null)
                    {
                        var recursElement = NameAttribute(child);
                        if (_dict.ContainsKey(key: recursElement?.Attribute("name")?.Value ??
                                                   throw new InvalidOperationException()))
                        {
                            _dict.TryGetValue(key: recursElement?.Attribute("name")?
                                                       .Value ?? throw new InvalidOperationException(),
                                out var value);
                            recursElement.AddBeforeSelf(value);
                            recursElement.Remove();
                        }
                    }
                    else
                    {
                        if (_dict.ContainsKey(key: child?.Attribute("name")?.
                                                       Value ?? throw new InvalidOperationException()))
                        {
                            _dict.TryGetValue(nameAttribute.Value, out var value);
                            child.AddBeforeSelf(value);
                            child.Remove();
                        }
                    }
                }
                element.Parent?.Add(element.Nodes());
                element.Remove();
            }

        }

        
        private static XElement NameAttribute(XElement child)
        {
            XElement element = default;

            foreach (var subChild in child.Elements().ToList())
            {
                element = subChild;
            }

            while (element?.Attribute("name") is null)
                NameAttribute(element);


            return element;
        }
    }
}