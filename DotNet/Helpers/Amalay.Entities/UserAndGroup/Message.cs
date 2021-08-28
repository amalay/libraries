using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Amalay.Entities
{
    public class MessageBody
    {
        public Message Message { get; set; }
    }

    public class Message
    {
        public string Subject { get; set; }

        public ItemBody Body { get; set; }

        public List<Recipient> ToRecipients { get; set; }
    }

    public class ItemBody
    {
        public string ContentType { get; set; }

        public string Content { get; set; }
    }

    public class Recipient
    {
        public EmailAddress EmailAddress { get; set; }
    }

    public class EmailAddress
    {
        public string Name { get; set; }

        public string Address { get; set; }
    }
}
