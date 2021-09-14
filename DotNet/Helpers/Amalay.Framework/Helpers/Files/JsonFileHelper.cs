using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Amalay.Framework
{
    public class JsonFileHelper
    {
        private string fileName = "JsonFileHelper.cs";

        #region "Singleton"

        private static readonly JsonFileHelper instance = new JsonFileHelper();

        private JsonFileHelper() { }

        public static JsonFileHelper Instance
        {
            get
            {
                return instance;
            }
        }

        #endregion

        public JObject ReadDataFromJsonFile(string root, string directoryName, string jsonFileName)
        {
            var functionName = "ReadDataFromJsonFile";
            JObject jsonData = null;

            try
            {
                if (string.IsNullOrEmpty(root))
                {
                    root = ".";
                }

                var filePath = Path.Combine(root, directoryName, jsonFileName);

                if (File.Exists(filePath))
                {
                    var content = File.ReadAllText(filePath);

                    if (!string.IsNullOrEmpty(content))
                    {
                        jsonData = JObject.Parse(content);
                    }
                }
            }
            catch
            {
                throw;
            }

            return jsonData;
        }

        public string ReadDataFromFile(string root, string directoryName, string fileName)
        {
            var functionName = "ReadDataFromFile";
            var content = string.Empty;
            var filePath = string.Empty;

            try
            {
                if (string.IsNullOrEmpty(root))
                {
                    root = ".";
                }

                if (!string.IsNullOrEmpty(directoryName))
                {
                    filePath = Path.Combine(root, directoryName, fileName);
                }
                else
                {
                    filePath = Path.Combine(root, fileName);
                }

                if (File.Exists(filePath))
                {
                    content = File.ReadAllText(filePath);
                }
            }
            catch
            {
                throw;
            }

            return content;
        }

        public string ReadContentFromJsonFile(string jsonFilePath)
        {
            string jsonMessage = string.Empty;

            try
            {
                if (!string.IsNullOrEmpty(jsonFilePath))
                {
                    string content = System.IO.File.ReadAllText(jsonFilePath);

                    jsonMessage = System.Text.RegularExpressions.Regex.Replace(content, @"\\[rnt]", m =>
                    {
                        switch (m.Value)
                        {
                            case @"\r": return "\r";
                            case @"\n": return "\n";
                            case @"\t": return "\t";
                            default: return m.Value;
                        }
                    });
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return jsonMessage;
        }
    }
}
