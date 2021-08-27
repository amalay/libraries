using Amalay.Entities;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Amalay.Framework
{
    public class ApplicationInsightsHelper
    {
        private string fileName = "ApplicationInsightsHelper.cs";
        private Microsoft.ApplicationInsights.TelemetryClient _telemetryClient = null;
        
        #region "Singleton"

        private static readonly ApplicationInsightsHelper instance = new ApplicationInsightsHelper();

        private ApplicationInsightsHelper() { }

        public static ApplicationInsightsHelper Instance
        {
            get
            {
                return instance;
            }
        }

        #endregion

        #region "Properties"

        public string Environment { get; set; }

        public string ApplicationInsightsKey { get; set; }

        private Microsoft.ApplicationInsights.TelemetryClient TelemetryClient
        {
            get
            {
                if (this._telemetryClient == null)
                {
                    if (string.IsNullOrEmpty(this.Environment))
                    {
                        this.Environment = Setting.Instance.AppSettings.GetValue<string>("Environment");
                    }

                    if (string.IsNullOrEmpty(this.ApplicationInsightsKey))
                    {
                        this.ApplicationInsightsKey = Setting.Instance.AppSettings.GetValue<string>("ApplicationInsightsKey");
                    }

                    this._telemetryClient = new Microsoft.ApplicationInsights.TelemetryClient(Microsoft.ApplicationInsights.Extensibility.TelemetryConfiguration.CreateDefault());
                    this._telemetryClient.InstrumentationKey = this.ApplicationInsightsKey;
                }

                return this._telemetryClient;
            }
        }

        #endregion

        #region "Public Methods"

        public void LogInformation(string applicationName, string moduleName, string fileName, string methodName, string message, string jsonData, bool logAsync = true)
        {
            var properties = this.SetTelemetryProperties(applicationName, moduleName, fileName, methodName, jsonData, DateTime.Now);

            if (logAsync)
            {
                Task.Factory.StartNew(() => this.LogInformation(message, properties));
            }
            else
            {
                this.LogInformation(message, properties);
            }
        }

        public void LogInformation(string applicationName, string moduleName, string fileName, string methodName, string message, string jsonData, IDictionary<string, string> telemetryProperties, bool logAsync = true)
        {
            var properties = this.SetTelemetryProperties(applicationName, moduleName, fileName, methodName, jsonData, telemetryProperties, DateTime.Now);

            if (logAsync)
            {
                Task.Factory.StartNew(() => this.LogInformation(message, properties));
            }
            else
            {
                this.LogInformation(message, properties);
            }
        }

        public void LogException(string applicationName, string moduleName, string fileName, string methodName, Exception exception, string jsonData, IDictionary<string, double> telemetryMetrics = null, bool logAsync = true)
        {
            if (exception != null)
            {
                var properties = this.SetTelemetryProperties(applicationName, moduleName, fileName, methodName, jsonData, DateTime.Now);
                var metrics = this.SetTelemetryMetrics(telemetryMetrics);

                if (logAsync)
                {
                    Task.Factory.StartNew(() => this.LogException(exception, properties, metrics));
                }
                else
                {
                    this.LogException(exception, properties, metrics);
                }
            }
        }

        public void LogException(string applicationName, string moduleName, string fileName, string methodName, Exception exception, string jsonData, IDictionary<string, string> telemetryProperties, IDictionary<string, double> telemetryMetrics = null, bool logAsync = true)
        {
            if (exception != null)
            {
                var properties = this.SetTelemetryProperties(applicationName, moduleName, fileName, methodName, jsonData, telemetryProperties, DateTime.Now);
                var metrics = this.SetTelemetryMetrics(telemetryMetrics);

                if (logAsync)
                {
                    Task.Factory.StartNew(() => this.LogException(exception, properties, metrics));
                }
                else
                {
                    this.LogException(exception, properties, metrics);
                }
            }
        }

        public void LogEvent(string applicationName, string moduleName, string fileName, string methodName, string eventName, string jsonData, IDictionary<string, double> telemetryMetrics = null, bool logAsync = true)
        {
            var properties = this.SetTelemetryProperties(applicationName, moduleName, fileName, methodName, jsonData, DateTime.Now);
            var metrics = this.SetTelemetryMetrics(telemetryMetrics);

            if (logAsync)
            {
                Task.Factory.StartNew(() => this.LogEvent(eventName, properties, metrics));
            }
            else
            {
                this.LogEvent(eventName, properties, metrics);
            }
        }

        public void LogEvent(string applicationName, string moduleName, string fileName, string methodName, string eventName, string jsonData, IDictionary<string, string> telemetryProperties, IDictionary<string, double> telemetryMetrics = null, bool logAsync = true)
        {
            var properties = this.SetTelemetryProperties(applicationName, moduleName, fileName, methodName, jsonData, telemetryProperties, DateTime.Now);
            var metrics = this.SetTelemetryMetrics(telemetryMetrics);

            if (logAsync)
            {
                Task.Factory.StartNew(() => this.LogEvent(eventName, properties, metrics));
            }
            else
            {
                this.LogEvent(eventName, properties, metrics);
            }
        }

        #endregion

        #region "Private Methods"

        private void LogInformation(string message, IDictionary<string, string> telemetryProperties)
        {
            if (this.TelemetryClient != null)
            {
                this.TelemetryClient.TrackTrace(message, telemetryProperties);
            }
        }

        private void LogException(Exception exception, IDictionary<string, string> telemetryProperties, IDictionary<string, double> telemetryMetrics = null)
        {
            if (this.TelemetryClient != null)
            {
                this.TelemetryClient.TrackException(exception, telemetryProperties, telemetryMetrics);
            }
        }

        private void LogEvent(string eventName, IDictionary<string, string> telemetryProperties, IDictionary<string, double> telemetryMetrics = null)
        {
            if (this.TelemetryClient != null)
            {
                this.TelemetryClient.TrackEvent(eventName, telemetryProperties, telemetryMetrics);
            }
        }

        private string BuildErrorMessage(Exception ex, string message, DateTime currentDate)
        {
            var sb = new StringBuilder();

            if (ex != null)
            {
                sb.Append(string.Format(CultureInfo.InvariantCulture, "\r\nExceptionMessage: {0}", ex.Message));
                sb.Append(string.Format(CultureInfo.InvariantCulture, "\r\nStackTrace: {0}", ex.StackTrace));

                if (ex.InnerException != null)
                {
                    sb.Append(string.Format(CultureInfo.InvariantCulture, "\r\nInnerExceptionMessage: {0}", ex.InnerException.Message));
                }

                if (!string.IsNullOrEmpty(message))
                {
                    sb.Append(string.Format(CultureInfo.InvariantCulture, "\r\nMessage: {0}", message));
                }
            }

            return sb.ToString();
        }

        private IDictionary<string, double> SetTelemetryMetrics(IDictionary<string, double> telemetryMetrics)
        {
            ConcurrentDictionary<string, double> metrics = null;

            if (telemetryMetrics != null)
            {
                metrics = new ConcurrentDictionary<string, double>(telemetryMetrics);
            }
            else
            {
                metrics = new ConcurrentDictionary<string, double>();
            }

            return telemetryMetrics;
        }

        private IDictionary<string, string> SetTelemetryProperties(string applicationName, string moduleName, string fileName, string methodName, string jsonData, DateTime currentDate)
        {
            IDictionary<string, string> telemetryProperties = new Dictionary<string, string>();
            telemetryProperties.Add("Environment", this.Environment);
            telemetryProperties.Add("LogDate", DateTimeHelper.GetFormatedDate(currentDate));
            telemetryProperties.Add("LogTime", DateTimeHelper.GetFormatedTime(currentDate));
            telemetryProperties.Add("ApplicationName", applicationName);
            telemetryProperties.Add("ModuleName", moduleName);
            telemetryProperties.Add("FileName", fileName);
            telemetryProperties.Add("MethodName", methodName);
            telemetryProperties.Add("JsonData", jsonData);

            return telemetryProperties;
        }

        private IDictionary<string, string> SetTelemetryProperties(string applicationName, string moduleName, string fileName, string methodName, string jsonData, IDictionary<string, string> telemetryProperties, DateTime currentDate)
        {
            ConcurrentDictionary<string, string> properties = null;

            if (telemetryProperties != null)
            {
                properties = new ConcurrentDictionary<string, string>(telemetryProperties);
            }
            else
            {
                properties = new ConcurrentDictionary<string, string>();
            }

            properties["Environment"] = this.Environment;
            properties["LogDate"] = DateTimeHelper.GetFormatedDate(currentDate);
            properties["LogTime"] = DateTimeHelper.GetFormatedTime(currentDate);
            properties["ApplicationName"] = applicationName;
            properties["ModuleName"] = moduleName;
            properties["FileName"] = fileName;
            properties["MethodName"] = methodName;
            properties["JsonData"] = jsonData;

            return telemetryProperties;
        }

        #endregion
    }
}
