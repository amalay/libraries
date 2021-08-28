using Amalay.Entities;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Amalay.Framework
{
    public class UserHelper
    {        
        private string fileName = "UserHelper.cs";
        private string message = string.Empty;

        #region "Singleton"

        private static readonly UserHelper instance = new UserHelper();

        private UserHelper() { }

        public static UserHelper Instance
        {
            get
            {
                return instance;
            }
        }

        #endregion

        #region "Get Users Using Microsoft Graph Api"

        public async Task<List<User>> GetAllUsers(string applicationName, string moduleName, IDictionary<string, string> settings)
        {
            return await UserHelper.Instance.GetUsers(applicationName, moduleName, settings, UserType.All);
        }

        public async Task<List<User>> GetGuestUsers(string applicationName, string moduleName, IDictionary<string, string> settings)
        {
            return await UserHelper.Instance.GetUsers(applicationName, moduleName, settings, UserType.Guest);
        }

        public async Task<List<User>> GetMemberUsers(string applicationName, string moduleName, IDictionary<string, string> settings)
        {
            return await UserHelper.Instance.GetUsers(applicationName, moduleName, settings, UserType.Member);
        }

        private async Task<List<User>> GetUsers(string applicationName, string moduleName, IDictionary<string, string> settings, UserType userType)
        {
            List<User> result = null;

            string methodName = $"users";
            string queryString = string.Empty;

            if (userType == UserType.Member || userType == UserType.Guest)
            {
                queryString = $"?filter=userType eq '{userType.ToString()}'";
            }

            var apiRsponse = await MicrosoftGraphApiHelper.Instance.ExecuteGetRequest_ODataResult_Paging<User>(applicationName, moduleName, settings, methodName, queryString);

            if(apiRsponse != null && apiRsponse.Data != null)
            {
                result = apiRsponse.Data;
            }

            return result;
        }

        public async Task<List<UserSignIn>> GetUserSignInDetails(string applicationName, string moduleName, IDictionary<string, string> settings, string queryString)
        {
            List<UserSignIn> result = null;

            var methodName = $"auditLogs/signIns";

            if (string.IsNullOrEmpty(queryString))
            {
                var utcDateTimeString = DateTimeHelper.Instance.GetStartDateTime(DateTime.UtcNow, OffsetType.Minute, 10);

                queryString = "?$filter=createdDateTime ge " + utcDateTimeString;
            }

            var apiResponse = await MicrosoftGraphApiHelper.Instance.ExecuteGetRequest_ODataResult_Paging<UserSignIn>(applicationName, moduleName, settings, methodName, queryString);

            if(apiResponse != null)
            {
                result = apiResponse.Data;
            }

            return result;
        }

        #endregion

        #region "Get User Mails Using Microsoft Graph Api"

        public async Task<List<T>> GetUserMailsFromSpecificFolder<T>(string applicationName, string moduleName, IDictionary<string, string> settings, string upn, string folderName, string queryString)
        {
            List<T> result = null;

            if (string.IsNullOrEmpty(queryString))
            {
                var utcDateTimeString = DateTimeHelper.Instance.GetStartDateTime(DateTime.UtcNow, OffsetType.Hour, 1);

                queryString = "?$filter=receivedDateTime ge " + utcDateTimeString;
            }

            string webApiMethod = $"users/{upn}/mailFolders/{folderName}/messages";

            var apiRsponse = await MicrosoftGraphApiHelper.Instance.ExecuteGetRequest_ODataResult_Paging<T>(applicationName, moduleName, settings, webApiMethod, queryString);

            if (apiRsponse != null && apiRsponse.Data != null)
            {
                result = apiRsponse.Data;
            }

            return result;
        }

        #endregion

        #region "Send Mail Using Microsoft Graph Api"

        public async Task<string> SendEmail(string applicationName, string moduleName, IDictionary<string, string> settings, string fromEmailAddress, List<string> toEmailAddresses, string subject, string mailBody, List<string> ccEmailAddresses)
        {
            string result = string.Empty;

            try
            {
                if (!string.IsNullOrEmpty(fromEmailAddress) && toEmailAddresses != null && toEmailAddresses.Count > 0 && !string.IsNullOrEmpty(subject) && !string.IsNullOrEmpty(mailBody))
                {
                    string apiMethodName = $"users/{fromEmailAddress}/sendMail";

                    MessageBody messageBody = new MessageBody()
                    {
                        Message = new Message()
                        {
                            Subject = subject,
                            Body = new ItemBody() { Content = mailBody, ContentType = "html" },
                            ToRecipients = new List<Recipient>()
                        }
                    };

                    foreach (var emailAddress in toEmailAddresses)
                    {
                        messageBody.Message.ToRecipients.Add(new Recipient() { EmailAddress = new EmailAddress() { Address = emailAddress } });
                    }

                    var apiResponse = await MicrosoftGraphApiHelper.Instance.ExecutePostRequest<string>(applicationName, moduleName, settings, apiMethodName, JsonConvert.SerializeObject(messageBody));

                    if(apiResponse != null)
                    {
                        result = apiResponse.Data;
                    }
                }
            }
            catch
            {
                throw;
            }

            return result;
        }

        #endregion        
    }
}
