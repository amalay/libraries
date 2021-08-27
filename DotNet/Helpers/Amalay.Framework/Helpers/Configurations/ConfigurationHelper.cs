using Amalay.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Amalay.Framework
{
    public class ConfigurationHelper
    {
        private string fileName = "ConfigurationHelper.cs";

        #region "Singleton"

        private static readonly ConfigurationHelper instance = new ConfigurationHelper();

        private ConfigurationHelper() { }

        public static ConfigurationHelper Instance
        {
            get
            {
                return instance;
            }
        }

        #endregion

        public IDictionary<string, string> LoadConfigurations(AppType appType)
        {
            var settings = new Dictionary<string, string>();

            switch (appType)
            {
                case AppType.AzureFunction:
                    var configurations = Environment.GetEnvironmentVariables();

                    if (configurations != null && configurations.Count > 0)
                    {
                        foreach (System.Collections.DictionaryEntry item in configurations)
                        {
                            settings[item.Key.ToString()] = item.Value.ConvertTo<string>();
                        }
                    }

                    break;
                default:                    

                    break;
            }

            return settings;
        }
    }
}
