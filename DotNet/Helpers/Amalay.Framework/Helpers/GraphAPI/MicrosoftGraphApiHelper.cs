using Amalay.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Amalay.Framework
{
    public class MicrosoftGraphApiHelper
    {        
        private string fileName = "MicrosoftGraphApiHelper.cs";
        private string message = string.Empty;

        #region "Singleton"

        private static readonly MicrosoftGraphApiHelper instance = new MicrosoftGraphApiHelper();

        private MicrosoftGraphApiHelper() { }

        public static MicrosoftGraphApiHelper Instance
        {
            get
            {
                return instance;
            }
        }

        #endregion

        #region "GET - GetAsync"

        public async Task<ApiResponse<T>> ExecuteGetRequest<T>(string applicationName, string moduleName, System.Collections.Generic.IDictionary<string, string> settings, string apiMethodName, string queryString)
        {            
            var methodName = "ExecuteGetRequest";
            var endpoint = $"{Resources.MicrosoftGraphApi.Name}/{settings.GetValue<string>("GraphApiVersion")}/{apiMethodName}";
            var accessToken = await TokenHelper.Instance.GetMicrosoftGraphApiAccessToken(applicationName, moduleName, settings);
            
            if (string.IsNullOrEmpty(accessToken))
            {
                message = $"InValidToken: Invalid/Expired token while executing {endpoint}";
                ApplicationInsightsHelper.Instance.LogInformation(applicationName, moduleName, fileName, methodName, message, null);

                throw new Exception(message);
            }

            if (!string.IsNullOrEmpty(queryString))
            {
                endpoint += queryString;
            }

            var apiResponse = await ApiHelper.Instance.ExecuteGetRequest_GetAsync<T>(endpoint, accessToken);

            return apiResponse;
        }

        public async Task<ApiResponse<T>> ExecuteGetRequest_ODataResult<T>(string applicationName, string moduleName, System.Collections.Generic.IDictionary<string, string> settings, string apiMethodName, string queryString)
        {            
            var methodName = "ExecuteGetRequest_ODataResult";                        
            var endpoint = $"{Resources.MicrosoftGraphApi.Name}/{settings.GetValue<string>("GraphApiVersion")}/{apiMethodName}";
            var accessToken = await TokenHelper.Instance.GetMicrosoftGraphApiAccessToken(applicationName, moduleName, settings);

            if (string.IsNullOrEmpty(accessToken))
            {
                message = $"InValidToken: Invalid/Expired token while executing {endpoint}";
                ApplicationInsightsHelper.Instance.LogInformation(applicationName, moduleName, fileName, methodName, message, null);

                throw new Exception(message);
            }

            if (!string.IsNullOrEmpty(queryString))
            {
                endpoint += queryString;
            }

            var apiResponse = await ApiHelper.Instance.ExecuteGetRequest_GetAsync_ODataResult<T>(endpoint, accessToken);

            return apiResponse;
        }

        public async Task<ApiResponse_Paging<T>> ExecuteGetRequest_ODataResult_Paging<T>(string applicationName, string moduleName, System.Collections.Generic.IDictionary<string, string> settings, string apiMethodName, string queryString)
        {
            var methodName = "ExecuteGetRequest_ODataResult_Paging";
            var endpoint = $"{Resources.MicrosoftGraphApi.Name}/{settings.GetValue<string>("GraphApiVersion")}/{apiMethodName}";
            var accessToken = await TokenHelper.Instance.GetMicrosoftGraphApiAccessToken(applicationName, moduleName, settings);

            if (string.IsNullOrEmpty(accessToken))
            {
                message = $"InValidToken: Invalid/Expired token while executing {endpoint}";
                ApplicationInsightsHelper.Instance.LogInformation(applicationName, moduleName, fileName, methodName, message, null);

                throw new Exception(message);
            }

            if (!string.IsNullOrEmpty(queryString))
            {
                endpoint += queryString;
            }

            var apiResponse = await ApiHelper.Instance.ExecuteGetRequest_GetAsync_ODataResult_Paging<T>(endpoint, accessToken);

            return apiResponse;
        }

        #endregion

        #region "POST - PostAsync"

        public async Task<ApiResponse<T>> ExecutePostRequest<T>(string applicationName, string moduleName, System.Collections.Generic.IDictionary<string, string> settings, string apiMethodName, string payload)
        {
            var methodName = "ExecutePostRequest";
            var endpoint = $"{Resources.MicrosoftGraphApi.Name}/{settings.GetValue<string>("GraphApiVersion")}/{apiMethodName}";
            var accessToken = await TokenHelper.Instance.GetMicrosoftGraphApiAccessToken(applicationName, moduleName, settings);

            if (string.IsNullOrEmpty(accessToken))
            {
                message = $"InValidToken: Invalid/Expired token while executing {endpoint}";
                ApplicationInsightsHelper.Instance.LogInformation(applicationName, moduleName, fileName, methodName, message, null);

                throw new Exception(message);
            }

            var apiResponse = await ApiHelper.Instance.ExecutePostRequest_PostAsync<T>(endpoint, accessToken, payload);            

            return apiResponse;
        }

        #endregion

        #region "DELETE"

        public async Task<ApiResponse<T>> ExecuteDeleteRequest<T>(string applicationName, string moduleName, System.Collections.Generic.IDictionary<string, string> settings, string apiMethodName, string queryString)
        {
            var methodName = "ExecuteDeleteRequest";
            var endpoint = $"{Resources.MicrosoftGraphApi.Name}/{settings.GetValue<string>("GraphApiVersion")}/{apiMethodName}";
            var accessToken = await TokenHelper.Instance.GetMicrosoftGraphApiAccessToken(applicationName, moduleName, settings);

            if (string.IsNullOrEmpty(accessToken))
            {
                message = $"InValidToken: Invalid/Expired token while executing {endpoint}";
                ApplicationInsightsHelper.Instance.LogInformation(applicationName, moduleName, fileName, methodName, message, null);

                throw new Exception(message);
            }

            if (!string.IsNullOrEmpty(queryString))
            {
                endpoint += queryString;
            }

            var apiResponse = await ApiHelper.Instance.ExecuteDeleteRequest_SendAsync<T>(endpoint, accessToken);

            return apiResponse;
        }

        #endregion

        #region "PATCH - SendAsync"

        public async Task<ApiResponse<T>> ExecutePatchRequest<T>(string applicationName, string moduleName, System.Collections.Generic.IDictionary<string, string> settings, string apiMethodName, string queryString, string payload)
        {
            var methodName = "ExecutePatchRequest";
            var endpoint = $"{Resources.MicrosoftGraphApi.Name}/{settings.GetValue<string>("GraphApiVersion")}/{apiMethodName}";
            var accessToken = await TokenHelper.Instance.GetMicrosoftGraphApiAccessToken(applicationName, moduleName, settings);

            if (string.IsNullOrEmpty(accessToken))
            {
                message = $"InValidToken: Invalid/Expired token while executing {endpoint}";
                ApplicationInsightsHelper.Instance.LogInformation(applicationName, moduleName, fileName, methodName, message, null);

                throw new Exception(message);
            }

            if (!string.IsNullOrEmpty(queryString))
            {
                endpoint += queryString;
            }

            var apiResponse = await ApiHelper.Instance.ExecutePatchRequest_SendAsync<T>(endpoint, accessToken, payload, AuthType.Bearer);

            return apiResponse;
        }

        #endregion
    }
}
