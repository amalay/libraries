using Amalay.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;

namespace Amalay.Framework
{
    public class Utility
    {
        private static readonly Utility instance = new Utility();

        private Utility() { }

        public static Utility Instance
        {
            get
            {
                return instance;
            }
        }

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
