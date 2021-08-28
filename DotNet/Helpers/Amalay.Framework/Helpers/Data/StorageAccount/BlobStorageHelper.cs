using Amalay.Entities;
using Azure.Identity;
using Azure.Storage.Blobs;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Amalay.Framework
{
    public class BlobStorageHelper
    {        
        private string fileName = "BlobStorageHelper.cs";
        private string message = string.Empty;

        #region "Singleton"

        private static readonly BlobStorageHelper instance = new BlobStorageHelper();

        private BlobStorageHelper() {  }

        public static BlobStorageHelper Instance
        {
            get
            {
                return instance;
            }
        }

        #endregion

        #region "Using MSI"

        public async Task<string> ReadDataFromBlobStorageUsingMSI(string applicationName, string moduleName, System.Collections.Generic.IDictionary<string, string> settings, string containerName, string fileName)
        {
            string result = string.Empty;
            var methodName = "ReadDataFromBlobStorageUsingMSI";

            try
            {
                var blobClient = this.GetBlobClientUsingMSI(applicationName, moduleName, settings, containerName, fileName);

                if (blobClient != null && await blobClient.ExistsAsync())
                {
                    using (StreamReader reader = new StreamReader(await blobClient.OpenReadAsync()))
                    {
                        result = await reader.ReadToEndAsync();
                    }
                }
            }
            catch (Exception ex)
            {
                ApplicationInsightsHelper.Instance.LogException(applicationName, moduleName, fileName, methodName, ex, message);
            }

            return result;
        }

        public async Task<string> WriteDataToBlobStorageUsingMSI(string applicationName, string moduleName, System.Collections.Generic.IDictionary<string, string> settings, string containerName, string fileName, string data)
        {
            string result = string.Empty;
            var methodName = "WriteDataToBlobStorageUsingMSI";

            try
            {
                var blobClient = this.GetBlobClientUsingMSI(applicationName, moduleName, settings, containerName, fileName);

                if (blobClient != null)
                {
                    byte[] byteArray = Encoding.ASCII.GetBytes(data);

                    using (MemoryStream stream = new MemoryStream(byteArray))
                    {
                        await blobClient.UploadAsync(stream, true);
                    }
                }
            }
            catch (Exception ex)
            {
                ApplicationInsightsHelper.Instance.LogException(applicationName, moduleName, fileName, methodName, ex, message);
            }

            return result;
        }

        private BlobClient GetBlobClientUsingMSI(string applicationName, string moduleName, System.Collections.Generic.IDictionary<string, string> settings, string containerName, string fileName)
        {
            BlobClient blobClient = null;

            if (settings != null)
            {
                var blobAccountName = settings.GetValue<string>("BlobStorageAccountName");

                if (!string.IsNullOrEmpty(blobAccountName))
                {
                    var blobEndPoint = $"https://{blobAccountName}.blob.core.windows.net/{containerName}/{fileName}"; //https://myaccount.blob.core.windows.net/mycontainer/myblob                    
                    blobClient = new BlobClient(new Uri(blobEndPoint), new DefaultAzureCredential());
                }
            }

            return blobClient;
        }

        #endregion

        #region "Using Blob Storage Connection String"

        public async Task<string> ReadDataFromBlobStorage(string applicationName, string moduleName, System.Collections.Generic.IDictionary<string, string> settings, string containerName, string fileName)
        {
            string result = string.Empty;
            var methodName = "ReadDataFromBlobStorageAsync";

            try
            {
                var blobContainerClient = await this.GetBlobContainerClient(applicationName, moduleName, settings, containerName);

                if (blobContainerClient != null)
                {
                    var blobClient = blobContainerClient.GetBlobClient(fileName);

                    if (blobClient != null && blobClient.ExistsAsync().Result)
                    {
                        using (StreamReader reader = new StreamReader(blobClient.OpenReadAsync().Result))
                        {
                            result = reader.ReadToEndAsync().Result;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ApplicationInsightsHelper.Instance.LogException(applicationName, moduleName, fileName, methodName, ex, message);
            }

            return result;
        }

        public async Task<string> WriteDataToBlobStorage(string applicationName, string moduleName, System.Collections.Generic.IDictionary<string, string> settings, string containerName, string fileName, string data)
        {
            string result = string.Empty;
            var methodName = "WriteDataToBlobStorageAsync";

            try
            {
                var blobContainerClient = await this.GetBlobContainerClient(applicationName, moduleName, settings, containerName);

                if (blobContainerClient != null)
                {
                    var blobClient = blobContainerClient.GetBlobClient(fileName);

                    if (blobClient != null)
                    {
                        byte[] byteArray = Encoding.ASCII.GetBytes(data);

                        using (MemoryStream stream = new MemoryStream(byteArray))
                        {
                            await blobClient.UploadAsync(stream, true);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ApplicationInsightsHelper.Instance.LogException(applicationName, moduleName, fileName, methodName, ex, message);
            }

            return result;
        }

        private async Task<BlobContainerClient> GetBlobContainerClient(string applicationName, string moduleName, System.Collections.Generic.IDictionary<string, string> settings, string containerName)
        {
            BlobContainerClient blobContainerClient = null;
            var methodName = "GetBlobContainerClient";

            if (settings != null)
            {
                var blobStorageConnectionString = settings.GetValue<string>("BlobStorageConnectionString");

                if (!string.IsNullOrEmpty(blobStorageConnectionString))
                {
                    //var blobServiceClient = new BlobServiceClient(blobStorageConnectionString);
                    //blobContainerClient = blobServiceClient.GetBlobContainerClient(containerName);
                    blobContainerClient = new BlobContainerClient(blobStorageConnectionString, containerName);
                    var blobContainerInfo = await blobContainerClient.CreateIfNotExistsAsync(Azure.Storage.Blobs.Models.PublicAccessType.BlobContainer);
                }
                else
                {
                    message = $"BlobStorageConnectionString does not exist";
                    ApplicationInsightsHelper.Instance.LogInformation(applicationName, moduleName, fileName, methodName, message, null);
                }
            }
            else
            {
                message = $"ISetting does not exist";
                ApplicationInsightsHelper.Instance.LogInformation(applicationName, moduleName, fileName, methodName, message, null);
            }

            return blobContainerClient;
        }

        #endregion
    }
}
