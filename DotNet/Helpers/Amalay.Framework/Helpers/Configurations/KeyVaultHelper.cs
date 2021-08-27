using Azure.Identity;
using Azure.Security.KeyVault.Secrets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Amalay.Framework
{
    public class KeyVaultHelper
    {
        private string fileName = "KeyVaultHelper.cs";

        #region "Singleton"

        private static readonly KeyVaultHelper instance = new KeyVaultHelper();

        private KeyVaultHelper() { }

        public static KeyVaultHelper Instance
        {
            get
            {
                return instance;
            }
        }

        #endregion

        #region "Read Secrets Using MSI"

        public async Task<IDictionary<string, string>> ReadSecretsAsync(string applicationName, string moduleName, string keyVaultBaseUrl)
        {
            if(string.IsNullOrEmpty(keyVaultBaseUrl))
            {
                throw new Exception("KeyVault base url is null or empty!");
            }

            var settings = new Dictionary<string, string>();
            var client = new SecretClient(new Uri(keyVaultBaseUrl), new DefaultAzureCredential());
            var properties = client.GetPropertiesOfSecretsAsync();

            await foreach (var property in properties)
            {
                var secret = await client.GetSecretAsync(property.Name);

                settings[secret.Value.Name] = secret.Value.Value;
            }

            return settings;
        }

        public async Task<IDictionary<string, string>> ReadSecretsAsync(string applicationName, string moduleName, string keyVaultBaseUrl, IEnumerable<string> secretsToRead)
        {
            if (string.IsNullOrEmpty(keyVaultBaseUrl))
            {
                throw new Exception("KeyVault base url is null or empty!");
            }

            if (secretsToRead == null || secretsToRead.Count() <= 0)
            {
                throw new Exception("List of secrets name is not provided!");
            }

            var settings = new Dictionary<string, string>();
            var client = new SecretClient(new Uri(keyVaultBaseUrl), new DefaultAzureCredential());
            var properties = client.GetPropertiesOfSecretsAsync();

            await foreach (var property in properties)
            {
                if (secretsToRead.Contains(property.Name))
                {
                    var secret = await client.GetSecretAsync(property.Name);

                    settings[secret.Value.Name] = secret.Value.Value;
                }                
            }

            return settings;
        }

        #endregion
    }
}
