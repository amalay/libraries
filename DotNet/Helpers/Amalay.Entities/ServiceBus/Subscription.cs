using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Amalay.Entities
{
    public class Subscription
    {
        public string Name { get; set; }
    }

    public class Topic
    {
        public string Name { get; set; }

        public List<Subscription> Subscriptions { get; set; }
    }

    public class Email
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public string ShortBody { get; set; }

        public string FullBody { get; set; }
    }

    public class EmailInfo
    {
        public Message Message { get; set; }

        public List<EmailRecipient> Recipients { get; set; }

        public List<EmailRecipient> CcRecipients { get; set; }

        public List<EmailRecipient> BccRecipients { get; set; }
    }

    public class EmailRecipient
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string EmailAddress { get; set; }
    }
}
