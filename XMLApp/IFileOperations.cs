using System.Xml.Linq;

namespace XMLApp
{
    public interface IFileOperations
    {
        XDocument LoadDocument(string path);
        void TransformTemplateNodes();
        void SaveXmlFile();
    }
}