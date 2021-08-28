using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Amalay.Entities
{
    public class User : Entity
    {
        public string UserPrincipalName { get; set; }

        public string DisplayName { get; set; }

        public string GivenName { get; set; }

        public string UserType { get; set; }

        public bool AccountEnabled { get; set; }

        public string CreatedDateTime { get; set; }

        public string CompanyName { get; set; }

        public string ExternalUserState { get; set; }

        public string UserAccountSource { get; set; }

        public string Mail { get; set; }

        public List<AssignedLicense> AssignedLicenses { get; set; }

        public List<AssignedPlans> AssignedPlans { get; set; }

        public List<AssignedPlans> ProvisionedPlans { get; set; }

        public List<Group> Groups { get; set; }
    }

    public class AssignedPlans
    {
        public string AssignedDateTime { get; set; }

        public string CapabilityStatus { get; set; }

        public string Service { get; set; }

        public string ServicePlanId { get; set; }

        public string ProvisioningStatus { get; set; }
    }

    public class AssignedLicense
    {
        public List<string> DisabledPlans { get; set; }

        public string SkuId { get; set; }
    }
}
