using Amalay.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Amalay.Framework
{
    public class LogAnalyticsHelper
    {
        private string fileName = "LogAnalyticsHelper.cs";
        private string message = string.Empty;

        #region "Singleton"

        private static readonly Lazy<LogAnalyticsHelper> instance = new Lazy<LogAnalyticsHelper>(() => new LogAnalyticsHelper());        

        public static LogAnalyticsHelper Instance 
        { 
            get 
            { 
                return instance.Value; 
            } 
        }

        #endregion

        public async Task<string> PostDataToAzureLogAnalytics(string applicationName, string moduleName, System.Collections.Generic.IDictionary<string, string> settings, string jsonData, string logAnalyticsTableName)
        {
            var methodName = "PostDataToAzureLogAnalytics";
            var result = string.Empty;

            try
            {
                var logAnalyticsWorkspaceId = settings.GetValue<string>("LogAnalyticsWorkspaceId");
                var logAnalyticsSharedKey = settings.GetValue<string>("LogAnalyticsSharedKey");

                if (!string.IsNullOrEmpty(logAnalyticsWorkspaceId) && !string.IsNullOrEmpty(logAnalyticsSharedKey) && !string.IsNullOrEmpty(jsonData))
                {
                    string datestring = DateTime.UtcNow.ToString("r");
                    string accessToken = this.GetSignature(applicationName, moduleName, datestring, jsonData, logAnalyticsWorkspaceId, logAnalyticsSharedKey);
                    var uri = this.GetUri(logAnalyticsWorkspaceId, AzureManagementApiVersion.V20160401);

                    using (var client = new HttpClient())
                    {
                        client.DefaultRequestHeaders.Add("Accept", "application/json");
                        client.DefaultRequestHeaders.Add("Log-Type", logAnalyticsTableName);
                        client.DefaultRequestHeaders.Add("Authorization", accessToken);
                        client.DefaultRequestHeaders.Add("x-ms-date", datestring);

                        System.Net.Http.HttpContent httpContent = new StringContent(jsonData, Encoding.UTF8);
                        httpContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");

                        using (var httpResponse = await client.PostAsync(uri, httpContent))
                        {
                            result = httpResponse.ReasonPhrase;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                result = "API Post Exception: " + ex.Message;
                ApplicationInsightsHelper.Instance.LogException(applicationName, moduleName, fileName, methodName, ex, result);
            }

            return result;
        }

        public async Task<LogAnalyticSearchResult> SearchLogAnalytics(string applicationName, string moduleName, System.Collections.Generic.IDictionary<string, string> settings, string searchQuery)
        {
            LogAnalyticSearchResult logAnalyticSearchResult = null;
            var methodName = "SearchLogAnalytics";
            var message = string.Empty;

            try
            {
                string jsonSearchQuery = "{\"query\": \"" + searchQuery + "\"}";

                if (!string.IsNullOrEmpty(jsonSearchQuery))
                {
                    logAnalyticSearchResult = await this.SearchLogAnalytics<LogAnalyticSearchResult>(applicationName, moduleName, settings, jsonSearchQuery);
                }
            }
            catch (Exception ex)
            {
                message = "API Post Exception: " + ex.Message;
                ApplicationInsightsHelper.Instance.LogException(applicationName, moduleName, fileName, methodName, ex, message);

                throw;
            }

            return logAnalyticSearchResult;
        }

        public async Task<LogAnalyticSearchResult> SearchLogAnalytics(string applicationName, string moduleName, System.Collections.Generic.IDictionary<string, string> settings, string searchQuery, string timeSpan)
        {
            LogAnalyticSearchResult logAnalyticSearchResult = null;
            var methodName = "SearchLogAnalytics";
            var message = string.Empty;

            try
            {
                string jsonSearchQuery = "{\"query\": \"" + searchQuery + "\", \"timespan\": \"" + timeSpan + "\"}";

                if (!string.IsNullOrEmpty(jsonSearchQuery))
                {
                    logAnalyticSearchResult = await this.SearchLogAnalytics<LogAnalyticSearchResult>(applicationName, moduleName, settings, jsonSearchQuery);
                }
            }
            catch (Exception ex)
            {
                message = "API Post Exception: " + ex.Message;
                ApplicationInsightsHelper.Instance.LogException(applicationName, moduleName, fileName, methodName, ex, message);

                throw;
            }

            return logAnalyticSearchResult;
        }

        public async Task<long> GetTotalRecords(string applicationName, string moduleName, System.Collections.Generic.IDictionary<string, string> settings, string searchQuery)
        {
            long totalRecords = 0;
            var methodName = "GetTotalRecords";
            var message = string.Empty;

            try
            {
                string jsonSearchQuery = "{\"query\": \"" + searchQuery + "\"}";
                var logAnalyticSearchResult = await this.SearchLogAnalytics<LogAnalyticSearchResult>(applicationName, moduleName, settings, jsonSearchQuery);

                if (logAnalyticSearchResult != null && logAnalyticSearchResult.Tables != null && logAnalyticSearchResult.Tables.Count > 0 && logAnalyticSearchResult.Tables[0].Rows != null && logAnalyticSearchResult.Tables[0].Rows.Count > 0)
                {
                    totalRecords = Convert.ToInt64(logAnalyticSearchResult.Tables[0].Rows[0][0]);
                }
            }
            catch (Exception ex)
            {
                message = "API Post Exception: " + ex.Message;
                ApplicationInsightsHelper.Instance.LogException(applicationName, moduleName, fileName, methodName, ex, message);

                throw;
            }

            return totalRecords;
        }

        #region "Common Private Methods"

        private Uri GetUri(string workspaceId, string apiVersion)
        {
            return new Uri($"https://{workspaceId}.ods.opinsights.azure.com/api/logs?{apiVersion}");
        }

        private string GetSignature(string applicationName, string moduleName, string datestring, string logJsonData, string logAnalyticsWorkspaceId, string logAnalyticsSharedKey)
        {
            // Create a hash for the API signature            
            var jsonBytes = Encoding.UTF8.GetBytes(logJsonData);
            string stringToHash = "POST\n" + jsonBytes.Length + "\napplication/json\n" + "x-ms-date:" + datestring + "\n/api/logs";
            string hashedString = this.BuildSignature(applicationName, moduleName, stringToHash, logAnalyticsSharedKey);
            string signature = "SharedKey " + logAnalyticsWorkspaceId + ":" + hashedString;

            return signature;
        }

        private string BuildSignature(string applicationName, string moduleName, string message, string secret)
        {
            var encoding = new System.Text.ASCIIEncoding();
            byte[] keyByte = Convert.FromBase64String(secret);
            byte[] messageBytes = encoding.GetBytes(message);

            using (var hmacsha256 = new HMACSHA256(keyByte))
            {
                byte[] hash = hmacsha256.ComputeHash(messageBytes);
                return Convert.ToBase64String(hash);
            }
        }

        private async Task<T> SearchLogAnalytics<T>(string applicationName, string moduleName, System.Collections.Generic.IDictionary<string, string> settings, string jsonData)
        {
            T result = default(T);
            var message = string.Empty;
            var methodName = "SearchLogAnalytics";

            string endpoint = $"{Resources.LogAnalyticsApi.Name}/{settings.GetValue<string>("LogAnalyticsApiVersion")}/workspaces/{settings.GetValue<string>("LogAnalyticsWorkspaceId")}/query";

            var accessToken = await TokenHelper.Instance.GetLogAnalyticsAccessToken(applicationName, moduleName, settings);

            if (!string.IsNullOrEmpty(accessToken))
            {
                var apiResponse = await ApiHelper.Instance.ExecutePostRequest_PostAsync<T>(endpoint, accessToken, jsonData);

                if (apiResponse.IsSuccess)
                {
                    result = apiResponse.Data;
                }
                else
                {
                    message = $"Error: {apiResponse.Message} while executing {endpoint}";
                    ApplicationInsightsHelper.Instance.LogInformation(applicationName, moduleName, fileName, methodName, message, null);
                }
            }
            else
            {
                message = $"InValidToken: Invalid/Expired token while executing {endpoint}";
                ApplicationInsightsHelper.Instance.LogInformation(applicationName, moduleName, fileName, methodName, message, null);
            }

            return result;
        }

        #endregion
    }
}
