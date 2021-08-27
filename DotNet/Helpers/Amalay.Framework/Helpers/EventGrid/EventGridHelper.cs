using Amalay.Entities;
using Microsoft.Azure.EventGrid;
using Microsoft.Azure.EventGrid.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Amalay.Framework
{
    public class EventGridHelper
    {
        private string fileName = $"EventGridHelper.cs";

        private EventGridHelper() { }

        public static EventGridHelper Instance { get; } = new EventGridHelper();

        public async Task<string> SendDataToEventGrid<T>(string applicationName, string moduleName, System.Collections.Generic.IDictionary<string, string> settings, T data, string eventType, string subject, string dataVersion)
        {
            string result = string.Empty;
            string methodName = "SendDataToEventGrid";

            try
            {
                if (data != null)
                {
                    string topicHostname = new Uri(settings.GetValue<string>("EventGridTopicEndPoint")).Host;
                    var topicCredentials = new TopicCredentials(settings.GetValue<string>("EventGridAccessKey"));
                    var client = new EventGridClient(topicCredentials);
                    var eventList = this.GetEventsList<T>(data, eventType, subject, dataVersion);
                    var jsonData = Newtonsoft.Json.JsonConvert.SerializeObject(eventList);
                    await client.PublishEventsAsync(topicHostname, eventList);

                    result = "OK";
                }
            }
            catch (Exception ex)
            {
                result = ex.Message;
                ApplicationInsightsHelper.Instance.LogException(applicationName, moduleName, fileName, methodName, ex, result);
            }

            return result;
        }

        private IList<EventGridEvent> GetEventsList<T>(T data, string eventType, string subject, string dataVersion)
        {
            var eventsList = new List<EventGridEvent>()
            {
                new EventGridEvent()
                {
                    Id = Guid.NewGuid().ToString(),
                    EventType = eventType,
                    EventTime = DateTime.Now,
                    Subject = subject,
                    DataVersion = dataVersion,
                    Data = data
                }
            };

            return eventsList;
        }

    }
}
