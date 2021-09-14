using Amalay.Entities;
//using Microsoft.ServiceBus.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Amalay.Framework
{
    public class WindowServiceBusHelper
    {
        private string fileName = "WindowServiceBusHelper.cs";

        #region "Singleton"

        private static readonly WindowServiceBusHelper instance = new WindowServiceBusHelper();

        private WindowServiceBusHelper() { }

        public static WindowServiceBusHelper Instance
        {
            get
            {
                return instance;
            }
        }

        #endregion

        #region "Public Methods"

        //public void CreateTopicAndSubscription(string topicName, string subscriptionName, string serviceBusConnectionString)
        //{
        //    TopicDescription topicDescription = new TopicDescription(topicName);
        //    topicDescription.MaxSizeInMegabytes = 5120;
        //    topicDescription.DefaultMessageTimeToLive = new TimeSpan(0, 1, 0);

        //    var namespaceManager = Microsoft.ServiceBus.NamespaceManager.CreateFromConnectionString(serviceBusConnectionString);

        //    if (!namespaceManager.TopicExists(topicName))
        //    {
        //        namespaceManager.CreateTopic(topicDescription);
        //    }

        //    if (!namespaceManager.SubscriptionExists(topicName, subscriptionName))
        //    {
        //        namespaceManager.CreateSubscription(topicName, subscriptionName, new SqlFilter(this.CreateFilter(subscriptionName)));
        //    }
        //}

        //public Dictionary<string, object> InitialiseMessageHeader(string subscriptionName)
        //{
        //    Dictionary<string, object> messageHeader = new Dictionary<string, object>();

        //    //required fields
        //    messageHeader.Add("MessageId", Guid.NewGuid());
        //    messageHeader.Add("MessageCorrelationId", Guid.NewGuid());
        //    messageHeader.Add("MessageSentTime", DateTime.UtcNow);

        //    messageHeader.Add("RouteParam1", subscriptionName);

        //    return messageHeader;
        //}

        //public void SendMessageToServiceBus(string serviceBusConnectionString, string topicName, string jsonMessage, Dictionary<string, object> messageHeader)
        //{
        //    var client = TopicClient.CreateFromConnectionString(serviceBusConnectionString, topicName);

        //    var brokeredMessage = new BrokeredMessage(jsonMessage);
        //    this.SetBrokeredMessageProperties(brokeredMessage, messageHeader);

        //    client.Send(brokeredMessage);
        //}

        //public void SendMessageToServiceBus<T>(string serviceBusConnectionString, string topicName, T messageInfo, Dictionary<string, object> messageHeader, JsonSerialiserType provider = JsonSerialiserType.NewtonsoftJsonSerializer)
        //{
        //    var client = TopicClient.CreateFromConnectionString(serviceBusConnectionString, topicName);

        //    BrokeredMessage brokeredMessage = null;

        //    if (provider == JsonSerialiserType.DataContractJsonSerializer)
        //    {
        //        brokeredMessage = this.CreateBrokeredMessage(messageInfo, messageHeader, new System.Runtime.Serialization.Json.DataContractJsonSerializer(typeof(T)));
        //    }
        //    else
        //    {
        //        brokeredMessage = this.CreateBrokeredMessage(messageInfo, messageHeader);
        //    }

        //    client.Send(brokeredMessage);
        //}

        #endregion

        #region "Private Methods"

        //private void SetBrokeredMessageProperties(BrokeredMessage brokeredMessage, IDictionary<string, object> msgProperties)
        //{
        //    if (msgProperties != null)
        //    {
        //        foreach (var msgProperty in msgProperties)
        //        {
        //            brokeredMessage.Properties.Add(msgProperty.Key, msgProperty.Value);
        //        }
        //    }
        //}

        //private BrokeredMessage CreateBrokeredMessage<T>(T messageInfo, IDictionary<string, object> messageProperties)
        //{
        //    string serialisedMessageInfo = Newtonsoft.Json.JsonConvert.SerializeObject(messageInfo);

        //    var brokeredMessage = new BrokeredMessage(serialisedMessageInfo)
        //    {
        //        ContentType = "text/plain"
        //    };

        //    this.SetBrokeredMessageProperties(brokeredMessage, messageProperties);

        //    return brokeredMessage;
        //}

        //private BrokeredMessage CreateBrokeredMessage<T>(T messageInfo, IDictionary<string, object> messageProperties, System.Runtime.Serialization.Json.DataContractJsonSerializer serializer)
        //{
        //    var brokeredMessage = new BrokeredMessage(messageInfo, serializer)
        //    {
        //        ContentType = typeof(T).AssemblyQualifiedName
        //    };

        //    this.SetBrokeredMessageProperties(brokeredMessage, messageProperties);

        //    return brokeredMessage;

        //}

        private string CreateFilter(string subscriptionName)
        {
            Dictionary<string, string> parameters = new Dictionary<string, string>();

            parameters.Add("RouteParam1", subscriptionName);

            return this.CreateFilter(parameters);
        }


        public string CreateFilter(Dictionary<string, string> parameters)
        {
            StringBuilder filter = new StringBuilder();

            foreach (var parameter in parameters)
            {
                filter.Append(parameter.Key);
                filter.Append(" = ");
                filter.Append("'" + parameter.Value + "'");
                filter.Append(" AND ");
            }

            filter.Remove(filter.Length - 5, 5);    //Remove last AND word from the filter.

            return filter.ToString();
        }

        #endregion
    }
}
