using Amalay.Entities;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security;

namespace Amalay.Framework
{
    public class Utility
    {
        private string fileName = "Utility.cs";

        #region "Singleton"

        private static readonly Utility instance = new Utility();

        private Utility() { }

        public static Utility Instance
        {
            get
            {
                return instance;
            }
        }

        #endregion

        public string GetRandomNumberInRange()
        {
            return new Random().Next(0, 999999).ToString("D6");
        }

        public SecureString SecurePassword(string password)
        {
            System.Security.SecureString securePassword = null;

            if (!string.IsNullOrEmpty(password))
            {
                securePassword = new System.Security.SecureString();

                foreach (char c in password)
                {
                    securePassword.AppendChar(c);
                }
            }

            return securePassword;
        }

        public List<string> GetPropertyNames(object instance)
        {
            var fields = new List<string>();

            if (instance != null)
            {
                var properties = instance.GetType().GetProperties();

                if (properties != null && properties.Length > 0)
                {
                    foreach (var property in properties)
                    {
                        fields.Add(property.Name);
                    }
                }
            }

            return fields;
        }

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

        public IDictionary<string, string> GetCustomerSettings(Setting setting, string searchKey)
        {
            var settings = new Dictionary<string, string>();
            
            if (setting != null && setting.CustomerSettings != null && setting.CustomerSettings.Count > 0 && !string.IsNullOrEmpty(searchKey))
            {
                settings["Environment"] = setting.AppSettings.GetValue<string>("Environment");
                settings["GenerateTokenThrough"] = setting.AppSettings.GetValue<string>("GenerateTokenThrough");
                settings["GraphApiVersion"] = setting.AppSettings.GetValue<string>("GraphApiVersion");
                settings["LogAnalyticsApiVersion"] = setting.AppSettings.GetValue<string>("LogAnalyticsApiVersion");

                var result = setting.CustomerSettings.Where(s => s.Key.StartsWith(searchKey));

                if (result != null && result.Count() > 0)
                {
                    foreach (var item in result)
                    {
                        var key = item.Key.Replace($"{searchKey}-", "");
                        var value = item.Value;

                        settings[key] = value;
                    }
                }
            }

            return settings;
        }
    }
}
