using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Amalay.Framework
{
    public class PayloadHelper
    {
        private string fileName = "PayloadHelper.cs";

        #region "Singleton"

        private static readonly PayloadHelper instance = new PayloadHelper();

        private PayloadHelper() { }

        public static PayloadHelper Instance
        {
            get
            {
                return instance;
            }
        }

        #endregion

        /// <summary>
        /// Get payload using app id and secret
        /// </summary>
        /// <param name="clientId"></param>
        /// <param name="clientSecret"></param>
        /// <param name="resource"></param>
        /// <returns></returns>
        public Dictionary<string, string> GetTokenPayload(string clientId, string clientSecret, string resource)
        {
            var payLoad = new Dictionary<string, string>
            {
                {"client_id", clientId},
                {"client_secret", clientSecret},
                {"grant_type", "client_credentials"},
                {"resource", resource}
            };

            return payLoad;
        }

        public string GetTokenPayload(string clientId, string clientSecret, string resource, string userName, string password, string scope = "")
        {
            var payLoad = new
            {
                client_id = clientId,
                client_secret = clientSecret,
                username = userName,
                password,
                grant_type = "password",
                resource,
                scope
            };

            return JsonConvert.SerializeObject(payLoad, Formatting.Indented);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="clientId"></param>
        /// <param name="resource"></param>
        /// <param name="userName"></param>
        /// <param name="password"></param>
        /// <param name="scope">openid</param>
        /// <returns></returns>
        public Dictionary<string, string> GetTokenPayload(string clientId, string userName, string password, string resource, string scope = "openid")
        {
            var payLoad = new Dictionary<string, string>
            {
                {"client_id", clientId},
                {"username", userName},
                {"password", password},
                {"grant_type", "password"},
                {"resource", resource},
                {"scope", scope}
            };

            return payLoad;
        }                

        /// <summary>
        /// Use for creating salesforce payload for access token using clientid, clientSecret, userName, password and securityToken
        /// </summary>
        /// <param name="clientId"></param>
        /// <param name="clientSecret"></param>
        /// <param name="userName"></param>
        /// <param name="password"></param>
        /// <param name="securityToken"></param>
        /// <returns></returns>
        public Dictionary<string, string> GetSalesforceAccessTokenPayload(string clientId, string clientSecret, string userName, string password, string securityToken)
        {
            var payLoad = new Dictionary<string, string>
            {
                {"grant_type", "password"},
                {"client_id", clientId},
                {"client_secret", clientSecret},
                {"username", userName},
                {"password", password + securityToken}
            };

            return payLoad;
        }

        public string GetMCASActivityPayload(double start, int startIndex, int length, string sortBy = "date", string direction = "asc")
        {
            var payLoad = new
            {
                filters = new
                {
                    created = new
                    {
                        gte = start
                    }
                },
                sortField = sortBy,
                sortDirection = direction,
                skip = startIndex,
                limit = length
            };

            return JsonConvert.SerializeObject(payLoad, Formatting.Indented);
        }

        public string GetMCASAlertPayload(double start, int startIndex, int length, string sortBy = "date", string direction = "asc")
        {
            var payLoad = new
            {
                filters = new
                {
                    date = new
                    {
                        gte = start
                    }
                },
                sortField = sortBy,
                sortDirection = direction,
                skip = startIndex,
                limit = length
            };

            return JsonConvert.SerializeObject(payLoad, Formatting.Indented);
        }

        public string GetUsersPayload(bool enable)
        {
            var payLoad = new
            {
                accountEnabled = enable
            };

            return JsonConvert.SerializeObject(payLoad, Formatting.Indented);
        }        
    }
}
