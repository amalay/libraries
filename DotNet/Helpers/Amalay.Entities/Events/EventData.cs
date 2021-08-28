using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Amalay.Entities
{
    public class EventData<T>
    {
        public string Subject { get; set; }

        public string EventType { get; set; }

        public string DataVersion { get; set; }

        public string TopicName { get; set; }

        public T Data { get; set; }
    }
}
