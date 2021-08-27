using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Amalay.Entities
{
    public class Resources
    {
        public struct KeyVaultApi
        {
            public const string Name = "https://vault.azure.net";
            public const string AppId = "cfa8b339-82a2-471a-a3c9-0fc0be7a4093"; //RoleId: "f53da476-18e3-4152-8e01-aec403e6edc0"
            public const string ObjectId = "";
            public const string BaseUrl = "";
        }

        public struct MicrosoftGraphApi
        {
            public const string Name = "https://graph.microsoft.com";
            public const string AppId = "00000003-0000-0000-c000-000000000000";
            public const string ObjectId = "";
            public const string BaseUrl = "https://graph.microsoft.com";
        }

        public struct AADGraphApi
        {
            public const string Name = "https://graph.windows.net";
            public const string AppId = "";
            public const string ObjectId = "";
            public const string BaseUrl = "https://graph.windows.net";
        }

        public struct Office365ManagementApi
        {
            public const string Name = "https://manage.office.com";
            public const string AppId = "c5393580-f805-4401-95e8-94b7a6ef2fc2";
            public const string ObjectId = "4910caae-c820-4efb-aa2b-80ce2d463ec1";
            public const string BaseUrl = "https://manage.office.com/api/v1.0";
        }

        public struct LogAnalyticsApi
        {
            public const string Name = "https://api.loganalytics.io";
            public const string AppId = "";
            public const string ObjectId = "";
            public static string BaseUrl = $"https://api.loganalytics.io";
        }

        public struct WindowsDefenderATP
        {
            public const string Name = "https://api.securitycenter.windows.com";
            public const string AppId = "";
            public const string ObjectId = "";
            public const string BaseUrl = "";
        }

        public struct PowerBiApi
        {
            public const string Name = "https://analysis.windows.net/powerbi/api";
            public const string AppId = "";
            public const string ObjectId = "";
            public const string BaseUrl = "https://api.powerbi.com/v1.0/myorg";
        }

        public struct AzureRightsManagementApi
        {
            public const string Name = "https://api.aadrm.com";
            public const string AppId = "90f610bf-206d-4950-b61d-37fa6fd1b224";
            public const string ObjectId = "";
            public const string BaseUrl = "https://admin.na.aadrm.com";
        }

        public struct AzureDataLakeApi
        {
            public const string Name = "https://datalake.azure.net/";
            public const string AppId = "";
            public const string ObjectId = "";
            public const string BaseUrl = "https://datalake.azure.net/";
        }

        public struct AzureStorageAccountApi
        {
            public const string Name = "https://storage.azure.com/";
            public const string AppId = "";
            public const string ObjectId = "";
            public const string BaseUrl = "https://storage.azure.com/";
        }

        public struct AzureStorageBlobApi
        {
            public const string Name = "https://<account>.blob.core.windows.net";
            public const string AppId = "";
            public const string ObjectId = "";
            public const string BaseUrl = "https://<account>.blob.core.windows.net";
        }

        public struct AzureStorageQueueApi
        {
            public const string Name = "https://<account>.queue.core.windows.net";
            public const string AppId = "";
            public const string ObjectId = "";
            public const string BaseUrl = "https://<account>.queue.core.windows.net";
        }

        public struct AzureManagementApi
        {
            public const string Name = "https://management.azure.com/";
            public const string AppId = "";
            public const string ObjectId = "";
            public const string BaseUrl = "https://management.azure.com/";
        }

        public struct AzureSqlApi
        {
            public const string Name = "https://database.windows.net/";
            public const string AppId = "";
            public const string ObjectId = "";
            public const string BaseUrl = "https://database.windows.net/";
        }

        public struct AzureEventHubApi
        {
            public const string Name = "https://eventhubs.azure.net";
            public const string AppId = "";
            public const string ObjectId = "";
            public const string BaseUrl = "https://eventhubs.azure.net";
        }

        public struct AzureServiceBusApi
        {
            public const string Name = "https://servicebus.azure.net";
            public const string AppId = "";
            public const string ObjectId = "";
            public const string BaseUrl = "https://servicebus.azure.net";
        }

        public struct AzureAnalysisServicesApi
        {
            public const string Name = "https://*.asazure.windows.net";
            public const string AppId = "";
            public const string ObjectId = "";
            public const string BaseUrl = "https://*.asazure.windows.net";
        }

        public struct Dynamics365Api
        {
            public const string Name = "https://api.securitycenter.windows.com";
            public const string AppId = "";
            public const string ObjectId = "";
            public const string BaseUrl = "";
        }
    }
}
