using Azure;
using Azure.Messaging.EventGrid;

using Microsoft.Azure.EventGrid;
using Microsoft.Azure.EventGrid.Models;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Amalay.Entities;
using System.Text.Json;
using Azure.Core.Serialization;

namespace Amalay.Framework
{
    public class EventGridHelper
    {
        private string fileName = $"EventGridHelper.cs";

        private EventGridHelper() { }

        public static EventGridHelper Instance { get; } = new EventGridHelper();

        #region "Using Azure.Messaging.EventGrid"

        public async Task<string> SendSingleDataToEventGridTopic<T>(string applicationName, string moduleName, IDictionary<string, string> settings, EventData<T> eventData, bool camelCaseSerialize)
        {
            string result = string.Empty;
            string methodName = "SendSingleDataToEventGridTopic";

            try
            {
                if (eventData == null)
                {
                    throw new Exception("Event grid data is not provided!");
                }

                var eventGridTopicEndpoint = settings.GetValue<string>("EventGridTopicEndPoint");     //"https://<topic-name>.<region>-1.eventgrid.azure.net/api/events";
                var eventGridTopicAccessKey = settings.GetValue<string>("EventGridTopicAccessKey");
                                
                Azure.Messaging.EventGrid.EventGridEvent ege = null;

                if (camelCaseSerialize) //Serialize data to JSON using a custom serializer
                {
                    ege = new Azure.Messaging.EventGrid.EventGridEvent(eventData.Subject, eventData.EventType, eventData.DataVersion, CustomSerializer.Instance.CamelCaseSerializer.Serialize(eventData.Data));
                }
                else //Serialize data to JSON using
                {
                    ege = new Azure.Messaging.EventGrid.EventGridEvent(eventData.Subject, eventData.EventType, eventData.DataVersion, eventData.Data);
                }

                var client = new EventGridPublisherClient(new Uri(eventGridTopicEndpoint), new AzureKeyCredential(eventGridTopicAccessKey));                    
                    
                await client.SendEventAsync(ege);

                result = "OK";
            }
            catch (Exception ex)
            {
                result = ex.Message;
                ApplicationInsightsHelper.Instance.LogException(applicationName, moduleName, fileName, methodName, ex, result);
            }

            return result;
        }

        public async Task<string> SendMultipleDataToEventGridTopic<T>(string applicationName, string moduleName, IDictionary<string, string> settings, List<EventData<T>> eventsData, bool camelCaseSerialize)
        {
            string result = string.Empty;
            string methodName = "SendMultipleDataToEventGridTopic";

            try
            {
                if (eventsData == null || eventsData.Count <= 0)
                {
                    throw new Exception("Events data is not provided!");
                }

                var eventGridTopicEndpoint = settings.GetValue<string>("EventGridTopicEndPoint");     //"https://<topic-name>.<region>-1.eventgrid.azure.net/api/events";
                var eventGridTopicAccessKey = settings.GetValue<string>("EventGridTopicAccessKey");

                Azure.Messaging.EventGrid.EventGridEvent ege = null;
                var eventList = new List<Azure.Messaging.EventGrid.EventGridEvent>();

                foreach (var eventData in eventsData)
                {
                    if (camelCaseSerialize) //Serialize data to JSON using a custom serializer
                    {
                        ege = new Azure.Messaging.EventGrid.EventGridEvent(eventData.Subject, eventData.EventType, eventData.DataVersion, CustomSerializer.Instance.CamelCaseSerializer.Serialize(eventData.Data));
                    }
                    else //Serialize data to JSON using
                    {
                        ege = new Azure.Messaging.EventGrid.EventGridEvent(eventData.Subject, eventData.EventType, eventData.DataVersion, eventData.Data);
                    }

                    eventList.Add(ege);
                }                

                var client = new EventGridPublisherClient(new Uri(eventGridTopicEndpoint), new AzureKeyCredential(eventGridTopicAccessKey));

                await client.SendEventsAsync(eventList);

                result = "OK";
            }
            catch (Exception ex)
            {
                result = ex.Message;
                ApplicationInsightsHelper.Instance.LogException(applicationName, moduleName, fileName, methodName, ex, result);
            }

            return result;
        }

        /// <summary>
        /// Send data to desired topic.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="applicationName"></param>
        /// <param name="moduleName"></param>
        /// <param name="settings"></param>
        /// <param name="eventData"></param>
        /// <returns></returns>
        public async Task<string> SendSingleDataToEventGridDomain<T>(string applicationName, string moduleName, IDictionary<string, string> settings, EventData<T> eventData)
        {
            string result = string.Empty;
            string methodName = "SendSingleDataToEventGridDomain";

            try
            {
                if (eventData == null)
                {
                    throw new Exception("Event grid data is not provided!");
                }

                var eventGridDomainEndpoint = settings.GetValue<string>("EventGridDomainEndPoint");     //"https://<topic-name>.<region>-1.eventgrid.azure.net/api/events";
                var eventGridDomainAccessKey = settings.GetValue<string>("EventGridDomainAccessKey");

                var ege = new Azure.Messaging.EventGrid.EventGridEvent(eventData.Subject, eventData.EventType, eventData.DataVersion, eventData.Data)
                {
                    Topic = eventData.TopicName
                };

                var client = new EventGridPublisherClient(new Uri(eventGridDomainEndpoint), new AzureKeyCredential(eventGridDomainAccessKey));

                await client.SendEventAsync(ege);

                result = "OK";
            }
            catch (Exception ex)
            {
                result = ex.Message;
                ApplicationInsightsHelper.Instance.LogException(applicationName, moduleName, fileName, methodName, ex, result);
            }

            return result;
        }

        /// <summary>
        /// Send multiple data in single topic or multiple topic in one go.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="applicationName"></param>
        /// <param name="moduleName"></param>
        /// <param name="settings"></param>
        /// <param name="eventsData"></param>
        /// <returns></returns>
        public async Task<string> SendMultipleDataToEventGridDomain<T>(string applicationName, string moduleName, IDictionary<string, string> settings, List<EventData<T>> eventsData)
        {
            string result = string.Empty;
            string methodName = "SendMultipleDataToEventGridDomain";

            try
            {
                if (eventsData == null || eventsData.Count <= 0)
                {
                    throw new Exception("Events data is not provided!");
                }

                var eventGridTopicEndpoint = settings.GetValue<string>("EventGridTopicEndPoint");     //"https://<topic-name>.<region>-1.eventgrid.azure.net/api/events";
                var eventGridTopicAccessKey = settings.GetValue<string>("EventGridTopicAccessKey");                                
                var eventList = new List<Azure.Messaging.EventGrid.EventGridEvent>();

                foreach (var eventData in eventsData)
                {
                    var ege = new Azure.Messaging.EventGrid.EventGridEvent(eventData.Subject, eventData.EventType, eventData.DataVersion, eventData.Data)
                    {
                        Topic = eventData.TopicName
                    };

                    eventList.Add(ege);
                }

                var client = new EventGridPublisherClient(new Uri(eventGridTopicEndpoint), new AzureKeyCredential(eventGridTopicAccessKey));

                await client.SendEventsAsync(eventList);

                result = "OK";
            }
            catch (Exception ex)
            {
                result = ex.Message;
                ApplicationInsightsHelper.Instance.LogException(applicationName, moduleName, fileName, methodName, ex, result);
            }

            return result;
        }

        #endregion


        #region "Using Microsoft.Azure.EventGrid"

        public async Task<string> SendDataToEventGrid<T>(string applicationName, string moduleName, System.Collections.Generic.IDictionary<string, string> settings, T data, string eventType, string subject, string dataVersion)
        {
            string result = string.Empty;
            string methodName = "SendDataToEventGrid";

            try
            {
                if (data != null)
                {
                    var topicEndpoint = settings.GetValue<string>("EventGridTopicEndPoint");     //"https://<topic-name>.<region>-1.eventgrid.azure.net/api/events";
                    var eventGridAccessKey = settings.GetValue<string>("EventGridAccessKey");
                    var topicHostname = new Uri(topicEndpoint).Host;                                        
                    var topicCredentials = new TopicCredentials(eventGridAccessKey);

                    var client = new Microsoft.Azure.EventGrid.EventGridClient(topicCredentials);                    
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

        private IList<Microsoft.Azure.EventGrid.Models.EventGridEvent> GetEventsList<T>(T data, string eventType, string subject, string dataVersion)
        {
            var eventsList = new List<Microsoft.Azure.EventGrid.Models.EventGridEvent>()
            {
                new Microsoft.Azure.EventGrid.Models.EventGridEvent()
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

        #endregion        
    }
}
