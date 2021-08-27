using Amalay.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Amalay.Framework
{
    public class EventHubHelper
    {
        private static readonly EventHubHelper instance = new EventHubHelper();
        private string fileName = "EventHubHelper.cs";
        private string message = string.Empty;

        private EventHubHelper()
        {

        }

        public static EventHubHelper Instance
        {
            get
            {
                return instance;
            }
        }

        public void GetEventHubClientUsingMSI(string applicationName, string moduleName, System.Collections.Generic.IDictionary<string, string> settings)
        {
            var eventHubName = settings.GetValue<string>("EventHubName");
            var eventHubPath = settings.GetValue<string>("EventHubPath");
            var eventHubEndpoint = $"{eventHubName}.eventhubs.windows.net";
            //var eventHubClient = new EventHubClient(eventHubEndpoint, eventHubPath, new DefaultAzureCredential());
            //var eventHubClient = new EventHubProducerClient(eventHubEndpoint, eventHubPath, new DefaultAzureCredential());
        }
    }
}
