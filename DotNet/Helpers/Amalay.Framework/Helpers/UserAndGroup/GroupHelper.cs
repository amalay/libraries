using Amalay.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Amalay.Framework
{
    public class GroupHelper
    {
        private string fileName = "GroupHelper.cs";
        private string message = string.Empty;

        #region "Singleton"

        private static readonly GroupHelper instance = new GroupHelper();

        private GroupHelper() { }

        public static GroupHelper Instance
        {
            get
            {
                return instance;
            }
        }

        #endregion

        public async Task<List<Group>> GetTeamsGroups(string applicationName, string moduleName, IDictionary<string, string> settings)
        {
            List<Group> result = null;

            //https://graph.microsoft.com/beta/groups?$filter=resourceProvisioningOptions/Any(x:x eq 'Team')
            var apiMethod = $"groups";
            var queryString = $"?$filter=resourceProvisioningOptions/Any(x:x eq 'Team')";

            var apiResponse = await MicrosoftGraphApiHelper.Instance.ExecuteGetRequest_ODataResult<List<Group>>(applicationName, moduleName, settings, apiMethod, queryString);

            if(apiResponse != null)
            {
                result = apiResponse.Data;
            }

            return result;
        }

        public async Task<List<Group>> GetUserGroups(string applicationName, string moduleName, IDictionary<string, string> settings, string userId)
        {
            List<Group> result = null;

            var apiMethod = $"users/" + userId;
            var queryString = $"/memberOf";

            var apiResponse = await MicrosoftGraphApiHelper.Instance.ExecuteGetRequest_ODataResult<List<Group>>(applicationName, moduleName, settings, apiMethod, queryString);

            if (apiResponse != null)
            {
                result = apiResponse.Data;
            }

            return result;
        }

        public async Task<List<Group>> GetGroupsInOrg(string applicationName, string moduleName, IDictionary<string, string> settings)
        {
            List<Group> result = null;
            var apiMethod = $"groups";
            var queryString = $"";

            var apiResponse = await MicrosoftGraphApiHelper.Instance.ExecuteGetRequest_ODataResult<List<Group>>(applicationName, moduleName, settings, apiMethod, queryString);

            if (apiResponse != null)
            {
                result = apiResponse.Data;
            }

            return result;
        }

        public async Task<List<User>> GetUsersInGroup(string applicationName, string moduleName, IDictionary<string, string> settings, string groupId)
        {
            List<User> result = null;

            var apiMethod = $"groups/{groupId}";
            var queryString = $"/members";

            var apiResponse = await MicrosoftGraphApiHelper.Instance.ExecuteGetRequest_ODataResult<List<User>>(applicationName, moduleName, settings, apiMethod, queryString);

            if (apiResponse != null)
            {
                result = apiResponse.Data;
            }

            return result;
        }
    }
}
