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
}
