using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Amalay.Entities
{
    public enum CertificateSearchBy
    {
        Name,
        FriendlyName,
        SubjectName,
        SerialNumber,
        Thumbprint
    }

    public enum JsonSerialiserType
    {
        NewtonsoftJsonSerializer = 0,
        DataContractJsonSerializer = 1
    }

    public struct DataProviders
    {
        public const string SqlServer = "SqlServer";
        public const string Oracle = "Oracle";
        public const string MySql = "MySql";
    }
}
