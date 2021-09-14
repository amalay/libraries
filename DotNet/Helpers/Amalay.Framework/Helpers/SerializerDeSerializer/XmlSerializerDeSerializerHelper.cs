using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Amalay.Framework
{
    public class XmlSerializerDeSerializerHelper
    {
        private static readonly XmlSerializerDeSerializerHelper _Instance = new XmlSerializerDeSerializerHelper();

        private XmlSerializerDeSerializerHelper() { }

        public static XmlSerializerDeSerializerHelper Instance
        {
            get
            {
                return _Instance;
            }
        }

        public void SerializeToXmlFile<T>(T entity, string xmlFilePath, string rootElementName = null, System.Xml.XmlWriterSettings xmlWriterSettings = null)
        {
            if (entity == null)
            {
                throw new ArgumentException("Null instance of entity.");
            }

            if (string.IsNullOrEmpty(xmlFilePath))
            {
                throw new ArgumentException("Xml file path is not provided.");
            }

            System.IO.File.WriteAllText(xmlFilePath, this.SerializeToXmlData(entity, rootElementName, xmlWriterSettings));
        }

        public string SerializeToXmlData<T>(T entity, string rootElementName = null, System.Xml.XmlWriterSettings xmlWriterSettings = null)
        {
            if (entity == null)
            {
                throw new ArgumentException("Null instance of entity.");
            }

            System.Xml.Serialization.XmlSerializer serializer = null;

            if (string.IsNullOrEmpty(rootElementName))
            {
                serializer = new System.Xml.Serialization.XmlSerializer(typeof(T));
            }
            else
            {
                serializer = new System.Xml.Serialization.XmlSerializer(typeof(T), new System.Xml.Serialization.XmlRootAttribute(rootElementName));
            }

            var settings = xmlWriterSettings ?? new System.Xml.XmlWriterSettings
            {
                Encoding = new System.Text.UnicodeEncoding(false, false),
                Indent = true,
                OmitXmlDeclaration = true
            };

            System.Xml.Serialization.XmlSerializerNamespaces namespaces = new System.Xml.Serialization.XmlSerializerNamespaces();
            namespaces.Add(string.Empty, string.Empty);

            using (var textWriter = new System.IO.StringWriter())
            {
                using (var xmlWriter = System.Xml.XmlWriter.Create(textWriter, settings))
                {
                    serializer.Serialize(xmlWriter, entity, namespaces);
                }

                return textWriter.ToString();
            }
        }

        public T DeserializeFromXmlFile<T>(string xmlFilePath, string rootElementName = null, System.Xml.XmlReaderSettings xmlReaderSettings = null)
        {
            if (string.IsNullOrEmpty(xmlFilePath))
            {
                throw new ArgumentException("Xml file path is not provided.");
            }

            string xmlData = System.IO.File.ReadAllText(xmlFilePath);

            return this.DeserializeFromXmlData<T>(xmlData, rootElementName, xmlReaderSettings);
        }

        public T DeserializeFromXmlData<T>(string xmlData, string rootElementName = null, System.Xml.XmlReaderSettings xmlReaderSettings = null)
        {
            if (string.IsNullOrEmpty(xmlData))
            {
                throw new ArgumentException("Xml data is not provided.");
            }

            System.Xml.Serialization.XmlSerializer serializer = null;

            if (string.IsNullOrEmpty(rootElementName))
            {
                serializer = new System.Xml.Serialization.XmlSerializer(typeof(T));
            }
            else
            {
                serializer = new System.Xml.Serialization.XmlSerializer(typeof(T), new System.Xml.Serialization.XmlRootAttribute(rootElementName));
            }

            var settings = xmlReaderSettings ?? new System.Xml.XmlReaderSettings();

            // No settings need modifying here
            using (var textReader = new System.IO.StringReader(xmlData))
            {
                using (var xmlReader = System.Xml.XmlReader.Create(textReader, settings))
                {
                    return (T)serializer.Deserialize(xmlReader);
                }
            }
        }

    }
}
