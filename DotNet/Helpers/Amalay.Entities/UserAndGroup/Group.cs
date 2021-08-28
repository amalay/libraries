using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Amalay.Entities
{
    public class Group : Entity
    {
        public string DisplayName { get; set; }

        public string Description { get; set; }

        public string MailNickname { get; set; }

        public bool MailEnabled { get; set; }

        public bool SecurityEnabled { get; set; }

        public string Visibility { get; set; }

        public List<string> ResourceProvisioningOptions { get; set; }

        public List<string> GroupTypes { get; set; }

        public string Mail { get; set; }
    }
}
