using Amalay.Entities;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace Amalay.Framework
{
    public class ApiHelper
    {
        private static readonly ApiHelper instance = new ApiHelper();

        public ApiHelper() { }

        public static ApiHelper Instance
        {
            get
            {
                return instance;
            }
        }

        #region "Execute GET Request"

        #region "GET - GetAsync"

        public async Task<ApiResponse<T>> ExecuteGetRequest_GetAsync<T>(string endpoint)
        {
            if (string.IsNullOrEmpty(endpoint))
            {
                throw new Exception("Endpoint is not provided!");
            }

            var apiResponse = new ApiResponse<T>();

            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.GetAsync(endpoint))
                {
                    if (response.IsSuccessStatusCode)
                    {
                        var result = await response.Content.ReadAsStringAsync();

                        if (typeof(T) == typeof(string))
                        {
                            apiResponse.Data = result.ConvertTo<T>();
                        }
                        else
                        {
                            apiResponse.Data = JsonConvert.DeserializeObject<T>(result);
                        }
                    }

                    apiResponse.IsSuccess = response.IsSuccessStatusCode;
                    apiResponse.StatusCode = response.StatusCode;
                    apiResponse.Message = response.ReasonPhrase;
                    apiResponse.Endpoint = endpoint;
                }
            }

            return apiResponse;
        }

        public async Task<ApiResponse<T>> ExecuteGetRequest_GetAsync<T>(string endpoint, string accessToken, AuthType authType = AuthType.Bearer)
        {
            if (string.IsNullOrEmpty(endpoint))
            {
                throw new Exception("Endpoint is not provided!");
            }

            if (string.IsNullOrEmpty(accessToken))
            {
                throw new Exception("Access token is not provided!");
            }

            var apiResponse = new ApiResponse<T>();

            using (var httpClient = new HttpClient())
            {
                httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue(authType.ToString(), accessToken);

                using (var response = await httpClient.GetAsync(endpoint))
                {
                    if (response.IsSuccessStatusCode)
                    {
                        var result = await response.Content.ReadAsStringAsync();

                        if (!string.IsNullOrEmpty(result))
                        {
                            apiResponse = JsonConvert.DeserializeObject<ApiResponse<T>>(result);
                        }
                    }

                    apiResponse.IsSuccess = response.IsSuccessStatusCode;
                    apiResponse.StatusCode = response.StatusCode;
                    apiResponse.Message = response.ReasonPhrase;
                    apiResponse.Endpoint = endpoint;
                }
            }

            return apiResponse;
        }

        public async Task<ApiResponse<T>> ExecuteGetRequest_GetAsync_ODataResult<T>(string endpoint, string accessToken, AuthType authType = AuthType.Bearer)
        {
            if (string.IsNullOrEmpty(endpoint))
            {
                throw new Exception("Endpoint is not provided!");
            }

            if (string.IsNullOrEmpty(accessToken))
            {
                throw new Exception("Access token is not provided!");
            }

            var apiResponse = new ApiResponse<T>();

            using (var httpClient = new HttpClient())
            {
                httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue(authType.ToString(), accessToken);

                using (var response = await httpClient.GetAsync(endpoint))
                {
                    if (response.IsSuccessStatusCode)
                    {
                        var result = await response.Content.ReadAsStringAsync();

                        if (typeof(T) == typeof(string))
                        {
                            apiResponse.Data = result.ConvertTo<T>();
                        }
                        else
                        {
                            var odataResult = Newtonsoft.Json.JsonConvert.DeserializeObject<ODataResult<T>>(result);

                            if (odataResult != null && odataResult.Value != null)
                            {
                                apiResponse.Data = odataResult.Value;
                            }
                        }
                    }

                    apiResponse.IsSuccess = response.IsSuccessStatusCode;
                    apiResponse.StatusCode = response.StatusCode;
                    apiResponse.Message = response.ReasonPhrase;
                    apiResponse.Endpoint = endpoint;
                }
            }

            return apiResponse;
        }

        public async Task<ApiResponse_Paging<T>> ExecuteGetRequest_GetAsync_ODataResult_Paging<T>(string endpoint, string accessToken, AuthType authType = AuthType.Bearer)
        {
            if (string.IsNullOrEmpty(endpoint))
            {
                throw new Exception("Endpoint is not provided!");
            }

            if (string.IsNullOrEmpty(accessToken))
            {
                throw new Exception("Access token is not provided!");
            }

            var apiResponse = new ApiResponse_Paging<T>() { Data = new List<T>() };

            using (var httpClient = new HttpClient())
            {
                httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue(authType.ToString(), accessToken);

                ODataResult_Paging<T> odataResult = null;

                do
                {
                    using (var response = await httpClient.GetAsync(endpoint))
                    {
                        endpoint = null;    //Reset the endpoint.

                        if (response.IsSuccessStatusCode)
                        {
                            var result = response.Content.ReadAsStringAsync().Result;

                            if (typeof(T) == typeof(string))
                            {
                                apiResponse.Data.Add(result.ConvertTo<T>());
                            }
                            else
                            {
                                odataResult = Newtonsoft.Json.JsonConvert.DeserializeObject<ODataResult_Paging<T>>(result);

                                if (odataResult != null)
                                {
                                    endpoint = odataResult.ODataNextLink;

                                    if (odataResult.Value != null && odataResult.Value.Count > 0)
                                    {
                                        apiResponse.Data.AddRange(odataResult.Value);
                                    }
                                }
                            }
                        }

                        apiResponse.IsSuccess = response.IsSuccessStatusCode;
                        apiResponse.StatusCode = response.StatusCode;
                        apiResponse.Message = response.ReasonPhrase;
                        apiResponse.Endpoint = endpoint;
                    }
                } while (!string.IsNullOrEmpty(endpoint));
            }

            return apiResponse;
        }

        #endregion

        #region "GET - SendAsync"

        public async Task<ApiResponse<T>> ExecuteGetRequest_SendAsync<T>(string endpoint)
        {
            if (string.IsNullOrEmpty(endpoint))
            {
                throw new Exception("Endpoint is not provided!");
            }

            var apiResponse = new ApiResponse<T>();

            using (var client = new HttpClient())
            {
                using (var request = new HttpRequestMessage(HttpMethod.Get, endpoint))
                {
                    using (var response = await client.SendAsync(request))
                    {
                        if (response.IsSuccessStatusCode)
                        {
                            if (typeof(T) == typeof(string))
                            {
                                apiResponse.Data = response.Content.ReadAsStringAsync().Result.ConvertTo<T>();
                            }
                            else
                            {
                                apiResponse.Data = JsonConvert.DeserializeObject<T>(await response.Content.ReadAsStringAsync());
                            }
                        }

                        apiResponse.IsSuccess = response.IsSuccessStatusCode;
                        apiResponse.StatusCode = response.StatusCode;
                        apiResponse.Message = response.ReasonPhrase;
                        apiResponse.Endpoint = endpoint;
                    }
                }
            }

            return apiResponse;
        }

        public async Task<ApiResponse<T>> ExecuteGetRequest_SendAsync<T>(string endpoint, string accessToken, AuthType authType = AuthType.Bearer)
        {
            if (string.IsNullOrEmpty(endpoint))
            {
                throw new Exception("Endpoint is not provided!");
            }

            if (string.IsNullOrEmpty(accessToken))
            {
                throw new Exception("Access token is not provided!");
            }

            var apiResponse = new ApiResponse<T>();

            using (var client = new HttpClient())
            {
                using (var request = new HttpRequestMessage(HttpMethod.Get, endpoint))
                {
                    request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    request.Headers.Authorization = new AuthenticationHeaderValue(authType.ToString(), accessToken);

                    using (var response = await client.SendAsync(request))
                    {
                        if (response.IsSuccessStatusCode)
                        {
                            var result = await response.Content.ReadAsStringAsync();

                            if (typeof(T) == typeof(string))
                            {
                                apiResponse.Data = result.ConvertTo<T>();
                            }
                            else
                            {
                                apiResponse.Data = JsonConvert.DeserializeObject<T>(result);
                            }
                        }

                        apiResponse.IsSuccess = response.IsSuccessStatusCode;
                        apiResponse.StatusCode = response.StatusCode;
                        apiResponse.Message = response.ReasonPhrase;
                        apiResponse.Endpoint = endpoint;
                    }
                }
            }

            return apiResponse;
        }

        public async Task<ApiResponse<T>> ExecuteGetRequest_SendAsync_ODataResult<T>(string endpoint, string accessToken, AuthType authType = AuthType.Bearer)
        {
            if (string.IsNullOrEmpty(endpoint))
            {
                throw new Exception("Endpoint is not provided!");
            }

            if (string.IsNullOrEmpty(accessToken))
            {
                throw new Exception("Access token is not provided!");
            }

            var apiResponse = new ApiResponse<T>();

            using (var client = new HttpClient())
            {
                using (var request = new HttpRequestMessage(HttpMethod.Get, endpoint))
                {
                    request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    request.Headers.Authorization = new AuthenticationHeaderValue(authType.ToString(), accessToken);

                    using (var response = await client.SendAsync(request))
                    {
                        if (response.IsSuccessStatusCode)
                        {
                            var result = await response.Content.ReadAsStringAsync();

                            if (typeof(T) == typeof(string))
                            {
                                apiResponse.Data = result.ConvertTo<T>();
                            }
                            else
                            {
                                var odataResult = Newtonsoft.Json.JsonConvert.DeserializeObject<ODataResult<T>>(result);

                                if (odataResult != null && odataResult.Value != null)
                                {
                                    apiResponse.Data = odataResult.Value;
                                }
                            }
                        }

                        apiResponse.IsSuccess = response.IsSuccessStatusCode;
                        apiResponse.StatusCode = response.StatusCode;
                        apiResponse.Message = response.ReasonPhrase;
                        apiResponse.Endpoint = endpoint;
                    }
                }
            }

            return apiResponse;
        }

        public async Task<ApiResponse_Paging<T>> ExecuteGetRequest_SendAsync_Paging<T>(string endpoint, string accessToken, AuthType authType = AuthType.Bearer)
        {
            if (string.IsNullOrEmpty(endpoint))
            {
                throw new Exception("Endpoint is not provided!");
            }

            if (string.IsNullOrEmpty(accessToken))
            {
                throw new Exception("Access token is not provided!");
            }

            var apiResponse = new ApiResponse_Paging<T>() { Data = new List<T>() };

            if (!string.IsNullOrEmpty(accessToken) && !string.IsNullOrEmpty(endpoint))
            {
                using (var client = new HttpClient())
                {
                    using (var request = new HttpRequestMessage(HttpMethod.Get, endpoint))
                    {
                        request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                        request.Headers.Authorization = new AuthenticationHeaderValue(authType.ToString(), accessToken);
                        ODataResult_Paging<T> odataResult = null;

                        do
                        {
                            using (var response = await client.SendAsync(request))
                            {
                                endpoint = null;    //Reset the endpoint.

                                if (response.IsSuccessStatusCode)
                                {
                                    var result = response.Content.ReadAsStringAsync().Result;

                                    if (typeof(T) == typeof(string))
                                    {
                                        apiResponse.Data.Add(result.ConvertTo<T>());
                                    }
                                    else
                                    {
                                        odataResult = Newtonsoft.Json.JsonConvert.DeserializeObject<ODataResult_Paging<T>>(result);

                                        if (odataResult != null)
                                        {
                                            endpoint = odataResult.ODataNextLink;

                                            if (odataResult.Value != null && odataResult.Value.Count > 0)
                                            {
                                                apiResponse.Data.AddRange(odataResult.Value);
                                            }
                                        }
                                    }
                                }

                                apiResponse.IsSuccess = response.IsSuccessStatusCode;
                                apiResponse.StatusCode = response.StatusCode;
                                apiResponse.Message = response.ReasonPhrase;
                                apiResponse.Endpoint = endpoint;
                            }
                        } while (!string.IsNullOrEmpty(endpoint));
                    }
                }
            }

            return apiResponse;
        }

        #endregion

        #endregion

        #region "Execute POST Request"

        #region "POST - SendAsync"

        public async Task<ApiResponse<T>> ExecutePostRequest_SendAsync<T>(string endpoint, string payload)
        {
            if (string.IsNullOrEmpty(endpoint))
            {
                throw new Exception("Endpoint is not provided!");
            }

            if (string.IsNullOrEmpty(payload))
            {
                throw new Exception("Payload is not provided!");
            }

            var apiResponse = new ApiResponse<T>();

            using (var httpClient = new HttpClient())
            {
                using (HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, endpoint))
                {                    
                    request.Content = new StringContent(payload, Encoding.UTF8, "application/json");

                    using (var response = await httpClient.SendAsync(request))
                    {
                        var result = await response.Content.ReadAsStringAsync();

                        if (typeof(T) == typeof(string))
                        {
                            apiResponse.Data = result.ConvertTo<T>();
                        }
                        else
                        {
                            apiResponse.Data = Newtonsoft.Json.JsonConvert.DeserializeObject<T>(result);
                        }

                        apiResponse.IsSuccess = response.IsSuccessStatusCode;
                        apiResponse.StatusCode = response.StatusCode;
                        apiResponse.Message = response.ReasonPhrase;
                        apiResponse.Endpoint = endpoint;
                    }
                }
            }

            return apiResponse;
        }

        public async Task<ApiResponse<T>> ExecutePostRequest_SendAsync<T>(string endpoint, string accessToken, string payload, AuthType authType = AuthType.Bearer)
        {
            if (string.IsNullOrEmpty(endpoint))
            {
                throw new Exception("Endpoint is not provided!");
            }

            if (string.IsNullOrEmpty(accessToken))
            {
                throw new Exception("Access token is not provided!");
            }

            if (string.IsNullOrEmpty(payload))
            {
                throw new Exception("Payload is not provided!");
            }

            var apiResponse = new ApiResponse<T>();

            using (var httpClient = new HttpClient())
            {
                using (HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, endpoint))
                {
                    request.Headers.Authorization = new AuthenticationHeaderValue(authType.ToString(), accessToken);
                    request.Content = new StringContent(payload, Encoding.UTF8, "application/json");

                    using (var response = await httpClient.SendAsync(request))
                    {
                        var result = await response.Content.ReadAsStringAsync();
                        apiResponse = JsonConvert.DeserializeObject<ApiResponse<T>>(result);

                        apiResponse.IsSuccess = response.IsSuccessStatusCode;
                        apiResponse.StatusCode = response.StatusCode;
                        apiResponse.Message = response.ReasonPhrase;
                        apiResponse.Endpoint = endpoint;
                    }
                }
            }

            return apiResponse;
        }

        #endregion

        #region "POST - PostAsync"

        public async Task<ApiResponse<T>> ExecutePostRequest_PostAsync<T>(string endpoint)
        {
            if (string.IsNullOrEmpty(endpoint))
            {
                throw new Exception("Endpoint is not provided!");
            }

            var apiResponse = new ApiResponse<T>();

            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.PostAsync(endpoint, new StringContent(null, System.Text.Encoding.UTF8, "application/json")))
                {
                    var result = await response.Content.ReadAsStringAsync();

                    if (typeof(T) == typeof(string))
                    {
                        apiResponse.Data = result.ConvertTo<T>();
                    }
                    else
                    {
                        apiResponse.Data = Newtonsoft.Json.JsonConvert.DeserializeObject<T>(result);
                    }

                    apiResponse.IsSuccess = response.IsSuccessStatusCode;
                    apiResponse.StatusCode = response.StatusCode;
                    apiResponse.Message = response.ReasonPhrase;
                    apiResponse.Endpoint = endpoint;
                }
            }

            return apiResponse;
        }

        public async Task<ApiResponse<T>> ExecutePostRequest_PostAsync<T>(string endpoint, string payload)
        {
            if (string.IsNullOrEmpty(endpoint))
            {
                throw new Exception("Endpoint is not provided!");
            }

            if (string.IsNullOrEmpty(payload))
            {
                throw new Exception("Payload is not provided!");
            }

            var apiResponse = new ApiResponse<T>();

            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.PostAsync(endpoint, new StringContent(payload, System.Text.Encoding.UTF8, "application/json")))
                {
                    var result = await response.Content.ReadAsStringAsync();

                    if (typeof(T) == typeof(string))
                    {
                        apiResponse.Data = result.ConvertTo<T>();
                    }
                    else
                    {
                        apiResponse.Data = Newtonsoft.Json.JsonConvert.DeserializeObject<T>(result);
                    }

                    apiResponse.IsSuccess = response.IsSuccessStatusCode;
                    apiResponse.StatusCode = response.StatusCode;
                    apiResponse.Message = response.ReasonPhrase;
                    apiResponse.Endpoint = endpoint;
                }
            }

            return apiResponse;
        }

        public async Task<ApiResponse<T>> ExecutePostRequest_PostAsync<T>(string endpoint, string accessToken, string payload, AuthType authType = AuthType.Bearer)
        {
            if (string.IsNullOrEmpty(endpoint))
            {
                throw new Exception("Endpoint is not provided!");
            }

            if (string.IsNullOrEmpty(endpoint))
            {
                throw new Exception("Access token is not provided!");
            }

            if (string.IsNullOrEmpty(payload))
            {
                throw new Exception("Payload is not provided!");
            }

            var apiResponse = new ApiResponse<T>();

            using (var httpClient = new HttpClient())
            {
                httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue(authType.ToString(), accessToken);

                using (var response = await httpClient.PostAsync(endpoint, new StringContent(payload, System.Text.Encoding.UTF8, "application/json")))
                {
                    var result = await response.Content.ReadAsStringAsync();

                    if (typeof(T) == typeof(string))
                    {
                        apiResponse.Data = result.ConvertTo<T>();
                    }
                    else
                    {
                        apiResponse.Data = Newtonsoft.Json.JsonConvert.DeserializeObject<T>(result);
                    }

                    apiResponse.IsSuccess = response.IsSuccessStatusCode;
                    apiResponse.StatusCode = response.StatusCode;
                    apiResponse.Message = response.ReasonPhrase;
                    apiResponse.Endpoint = endpoint;
                }
            }

            return apiResponse;
        }

        public async Task<ApiResponse<T>> ExecutePostRequest_PostAsync_ODataResult<T>(string endpoint, string accessToken, string payload, AuthType authType = AuthType.Bearer)
        {
            if (string.IsNullOrEmpty(endpoint))
            {
                throw new Exception("Endpoint is not provided!");
            }

            if (string.IsNullOrEmpty(endpoint))
            {
                throw new Exception("Access token is not provided!");
            }

            if (string.IsNullOrEmpty(payload))
            {
                throw new Exception("Payload is not provided!");
            }

            var apiResponse = new ApiResponse<T>();

            using (var httpClient = new HttpClient())
            {
                httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue(authType.ToString(), accessToken);

                using (var response = await httpClient.PostAsync(endpoint, new StringContent(payload, System.Text.Encoding.UTF8, "application/json")))
                {
                    var result = await response.Content.ReadAsStringAsync();

                    if (typeof(T) == typeof(string))
                    {
                        apiResponse.Data = result.ConvertTo<T>();
                    }
                    else
                    {
                        var odataResult = Newtonsoft.Json.JsonConvert.DeserializeObject<ODataResult<T>>(result);

                        if (odataResult != null && odataResult.Value != null)
                        {
                            apiResponse.Data = odataResult.Value;
                        }
                    }

                    apiResponse.IsSuccess = response.IsSuccessStatusCode;
                    apiResponse.StatusCode = response.StatusCode;
                    apiResponse.Message = response.ReasonPhrase;
                    apiResponse.Endpoint = endpoint;
                }
            }

            return apiResponse;
        }

        #endregion

        #endregion


        #region "Execute DELETE Request"

        public async Task<ApiResponse<T>> ExecuteDeleteRequest_SendAsync<T>(string endpoint, string accessToken, AuthType authType = AuthType.Bearer)
        {
            if (string.IsNullOrEmpty(endpoint))
            {
                throw new Exception("Endpoint is not provided!");
            }

            if (string.IsNullOrEmpty(accessToken))
            {
                throw new Exception("Access token is not provided!");
            }

            var apiResponse = new ApiResponse<T>();

            using (var client = new HttpClient())
            {
                using (var request = new HttpRequestMessage(HttpMethod.Delete, endpoint))
                {
                    request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    request.Headers.Authorization = new AuthenticationHeaderValue(authType.ToString(), accessToken);

                    using (var response = await client.SendAsync(request))
                    {
                        if (response.IsSuccessStatusCode)
                        {
                            var result = await response.Content.ReadAsStringAsync();

                            if (!string.IsNullOrEmpty(result))
                            {
                                apiResponse = JsonConvert.DeserializeObject<ApiResponse<T>>(result);
                            }
                        }

                        apiResponse.IsSuccess = response.IsSuccessStatusCode;
                        apiResponse.StatusCode = response.StatusCode;
                        apiResponse.Message = response.ReasonPhrase;
                        apiResponse.Endpoint = endpoint;
                    }
                }
            }

            return apiResponse;
        }

        #endregion

        #region "Execute PUT Request"

        public async Task<ApiResponse<T>> ExecutePutRequest_SendAsync<T>(string endpoint, string accessToken, string payload, AuthType authType = AuthType.Bearer)
        {
            if (string.IsNullOrEmpty(endpoint))
            {
                throw new Exception("Endpoint is not provided!");
            }

            if (string.IsNullOrEmpty(accessToken))
            {
                throw new Exception("Access token is not provided!");
            }

            if (string.IsNullOrEmpty(payload))
            {
                throw new Exception("Payload is not provided!");
            }

            var apiResponse = new ApiResponse<T>();

            using (var client = new HttpClient())
            {
                using (var request = new HttpRequestMessage(HttpMethod.Put, endpoint))
                {
                    request.Headers.Authorization = new AuthenticationHeaderValue(authType.ToString(), accessToken);
                    request.Content = new StringContent(payload, Encoding.UTF8, "application/json");

                    using (var response = await client.SendAsync(request))
                    {
                        var result = await response.Content.ReadAsStringAsync();

                        if (!string.IsNullOrEmpty(result))
                        {
                            apiResponse = JsonConvert.DeserializeObject<ApiResponse<T>>(result);
                        }

                        apiResponse.IsSuccess = response.IsSuccessStatusCode;
                        apiResponse.StatusCode = response.StatusCode;
                        apiResponse.Message = response.ReasonPhrase;
                        apiResponse.Endpoint = endpoint;
                    }
                }
            }

            return apiResponse;
        }

        #endregion

        #region "Execute PATCH Request"

        public async Task<ApiResponse<T>> ExecutePatchRequest_SendAsync<T>(string endpoint, string accessToken, string payload, AuthType authType = AuthType.Bearer)
        {
            if (string.IsNullOrEmpty(endpoint))
            {
                throw new Exception("Endpoint is not provided!");
            }

            if (string.IsNullOrEmpty(accessToken))
            {
                throw new Exception("Access token is not provided!");
            }

            if (string.IsNullOrEmpty(payload))
            {
                throw new Exception("Payload is not provided!");
            }

            var apiResponse = new ApiResponse<T>();

            using (var client = new HttpClient())
            {
                using (var request = new HttpRequestMessage(new HttpMethod("PATCH"), endpoint))
                {
                    request.Headers.Authorization = new AuthenticationHeaderValue(authType.ToString(), accessToken);
                    request.Content = new StringContent(payload, Encoding.UTF8, "application/json");

                    using (var response = await client.SendAsync(request))
                    {
                        var result = await response.Content.ReadAsStringAsync();

                        if (!string.IsNullOrEmpty(result))
                        {
                            apiResponse = JsonConvert.DeserializeObject<ApiResponse<T>>(result);
                        }

                        apiResponse.IsSuccess = response.IsSuccessStatusCode;
                        apiResponse.StatusCode = response.StatusCode;
                        apiResponse.Message = response.ReasonPhrase;
                        apiResponse.Endpoint = endpoint;
                    }
                }
            }

            return apiResponse;
        }

        #endregion
    }
}
