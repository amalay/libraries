using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Amalay.Entities
{    
    public struct Test
    {
        public const string TestMessage = "Test Message!";
    }

    public struct App
    {
        public const string Api = "Amalay.Api";
        public const string SecureApi = "Amalay.SecureApi";
        public const string SecureApiClient = "Amalay.SecureApiClient";
        public const string Console = "Amalay.Console";
        public const string Website = "Amalay.Website";
        public const string Window = "Amalay.Window";
        public const string WPF = "Amalay.WPF";
        public const string UWP = "Amalay.UWP";
        public const string KuberneteContainer = "Amalay.KuberneteContainer";
        public const string AzureFunction = "Amalay.AzureFunction";
        public const string ServiceFabric = "Amalay.AzureServiceFabric";
        public const string ServiceFabricMesh = "Amalay.AzureServiceFabricMesh";
        public const string Database = "Amalay.Database";
    }

    public struct Salesforce
    {
        public const string ApiEndpoint = "services/data/";
    }

    public struct AzureManagementApiVersion
    {
        public const string V20160401 = "api-version=2016-04-01";
        public const string V20180801 = "api-version=2018-08-01";
        public const string V20150320 = "api-version=2015-03-20";
        public const string V20171001 = "api-version=2017-10-01";
        public const string V20170101_Preview = "api-version=2017-01-01-preview";
    }

    public struct FlowApiVersion
    {
        public const string V91 = "v9.1";
    }

    public struct Providers
    {
        public const string MicrosoftNetwork = "Microsoft.Network";
        public const string NetworkInterfaces = "Microsoft.Network/networkInterfaces";
        public const string IpConfigurations = "Microsoft.Network/networkInterfaces/ipConfigurations";
        public const string PublicIpAddresses = "Microsoft.Network/publicIpAddresses";
    }

    public struct Operations
    {
        public const string CreatePublicIp = "Microsoft.Network/publicIPAddresses/write";
    }

    public struct LogAnalyticsTable
    {
        public const string AV = "AV";
        public const string O365 = "O365";
        public const string O365ServiceHealth = "O365ServiceHealth";
        public const string Salesforce = "Salesforce";
        public const string SalesforceUser = "SalesforceUser";
    }

    public struct LogType
    {
        public const string DnsLookup = "DnsLookup";
        public const string GuestUser = "GuestUser";
        public const string UserSignInDetails = "UserSignInDetails";        
    }

    public struct Status
    {
        public const string None = "None";
        public const string NoRecordExist = "NoRecordExist";
        public const string AlreadyExist = "AlreadyExist";
        public const string Created = "Created";
        public const string Updated = "Updated";
        public const string Deleted = "Deleted";

        public const string Success = "Success";
        public const string Error = "Error";
        public const string ApiCallError = "ApiCallError";
        public const string Exception = "Exception";
        public const string Oops = "Oops";
    }

    public struct StatusMessage
    {
        public const string SaveSuccess = "Data saved successfully!";
        public const string SaveError = "Error occurred while saving the data!";

        public const string UpdateSuccess = "Data updated successfully!";
        public const string UpdateError = "Error occurred while updating the data!";

        public const string SendSuccess = "Message send successfully!";
        public const string SendError = "Error occurred while sending message!";
    }
}
