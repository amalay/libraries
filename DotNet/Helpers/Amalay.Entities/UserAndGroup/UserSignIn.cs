using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Amalay.Entities
{
    public class UserSignIn : Entity
    {
        public UserSignInDevice DeviceDetail { get; set; }

        public UserSignInStatus Status { get; set; }

        public UserMFADetail MFADetail { get; set; }

        public List<string> AuthenticationMethodsUsed { get; set; }

        public string ResourceDisplayName { get; set; }

        public string RiskDetail { get; set; }

        public string RiskState { get; set; }

        public string TokenIssuerType { get; set; }

        public bool IsInteractive { get; set; }

        public string IpAddress { get; set; }

        public string AppDisplayName { get; set; }

        public string AppId { get; set; }

        public string UserSignInSource { get; set; }

        public string UserId { get; set; }

        public string UserPrincipalName { get; set; }

        public string UserDisplayName { get; set; }

        public string CreatedDateTime { get; set; }

        public Location Location { get; set; }

        public string UserType { get; set; }
    }

    public class UserSignInDevice
    {
        public string DeviceId { get; set; }

        public string DisplayName { get; set; }

        public string OperatingSystem { get; set; }

        public string Browser { get; set; }

        public string IsCompliant { get; set; }

        public string IsManaged { get; set; }

        public string TrustType { get; set; }
    }

    public class UserSignInStatus
    {
        public string ErrorCode { get; set; }

        public string FailureReason { get; set; }

        public string AdditionalDetails { get; set; }
    }

    public class UserMFADetail
    {
        public string AuthMethod { get; set; }

        public string AuthDetail { get; set; }
    }

    public class Location
    {
        public string City { get; set; }

        public string State { get; set; }

        public string CountryOrRegion { get; set; }

        public Geocoordinates GeoCoordinates { get; set; }
    }

    public class Geocoordinates
    {
        public object Altitude { get; set; }

        public float? Latitude { get; set; }

        public float? Longitude { get; set; }
    }
}
