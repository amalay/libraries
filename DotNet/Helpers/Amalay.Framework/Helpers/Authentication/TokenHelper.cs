using Amalay.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Amalay.Framework
{
    public class TokenHelper
    {        
        private string fileName = "TokenHelper.cs";
        private string scope = "openid";

        #region "Singleton"

        private static readonly TokenHelper instance = new TokenHelper();

        private TokenHelper() { }

        public static TokenHelper Instance
        {
            get
            {
                return instance;
            }
        }

        #endregion

        #region "Generate Token"

        public async Task<OAuthTokenResponse> GetAccessTokenUsingServicePrincipal(string applicationName, string moduleName, string clientId, string clientSecret, string tenantId, string resource)
        {
            var endpoint = $"https://login.microsoftonline.com/{tenantId}/oauth2/token";
            HttpContent httpContent = new FormUrlEncodedContent(PayloadHelper.Instance.GetTokenPayload(clientId, clientSecret, resource));

            return (await ApiHelper.Instance.ExecutePostRequest_SendAsync<OAuthTokenResponse>(endpoint, httpContent)).Data;
        }

        public async Task<OAuthTokenResponse> GetAccessTokenUsingServiceAccount(string applicationName, string moduleName, string clientId, string tenantId, string userName, string password, string resource, string scope)
        {
            var endpoint = $"https://login.microsoftonline.com/{tenantId}/oauth2/token";
            HttpContent httpContent = new FormUrlEncodedContent(PayloadHelper.Instance.GetTokenPayload(clientId, userName, password, resource, scope));

            return (await ApiHelper.Instance.ExecutePostRequest_SendAsync<OAuthTokenResponse>(endpoint, httpContent)).Data;
        }        

        public async Task<string> GetAccessTokenUsingMSI(string applicationName, string moduleName, string resource)
        {
            return await new Microsoft.Azure.Services.AppAuthentication.AzureServiceTokenProvider().GetAccessTokenAsync(resource);
        }

        #endregion

        #region "Key Vault"

        public async Task<string> GetKeyVaultAccessTokenUsingMSI(string applicationName, string moduleName)
        {
            return await this.GetAccessTokenUsingMSI(applicationName, moduleName, Resources.KeyVaultApi.Name);
        }

        #endregion

        #region "Microsoft Graph Api"

        private OAuthTokenResponse microsoftGraphApiTokenResponseUsingSP = null;
        private OAuthTokenResponse microsoftGraphApiTokenResponseUsingSA = null;
        public async Task<string> GetMicrosoftGraphApiAccessToken(string applicationName, string moduleName, System.Collections.Generic.IDictionary<string, string> settings)
        {
            string accessToken = string.Empty;

            if (settings != null)
            {
                string generateTokenThrough = settings.GetValue<string>("GenerateTokenThrough");

                if (string.IsNullOrEmpty(generateTokenThrough))
                {
                    generateTokenThrough = GenerateTokenThrough.MSI.ToString();
                }

                if (generateTokenThrough == GenerateTokenThrough.MSI.ToString())
                {
                    accessToken = await this.GetAccessTokenUsingMSI(applicationName, moduleName, Resources.MicrosoftGraphApi.AppId);
                }
                else if (generateTokenThrough == GenerateTokenThrough.ServicePrincipal.ToString())
                {
                    if (this.microsoftGraphApiTokenResponseUsingSP == null || string.IsNullOrEmpty(this.microsoftGraphApiTokenResponseUsingSP.Access_Token) || (this.microsoftGraphApiTokenResponseUsingSP.TokenExpireTime < DateTime.UtcNow))
                    {
                        this.microsoftGraphApiTokenResponseUsingSP = await this.GetAccessTokenUsingServicePrincipal(applicationName, moduleName, settings.GetValue<string>("ClientId"), settings.GetValue<string>("ClientSecret"), settings.GetValue<string>("TenantId"), Resources.MicrosoftGraphApi.Name);
                    }

                    if (this.microsoftGraphApiTokenResponseUsingSP != null)
                    {
                        accessToken = this.microsoftGraphApiTokenResponseUsingSP.Access_Token;
                    }
                }
                else if (generateTokenThrough == GenerateTokenThrough.ServiceAccount.ToString())
                {
                    if (this.microsoftGraphApiTokenResponseUsingSA == null || string.IsNullOrEmpty(this.microsoftGraphApiTokenResponseUsingSA.Access_Token) || (this.microsoftGraphApiTokenResponseUsingSA.TokenExpireTime < DateTime.UtcNow))
                    {
                        this.microsoftGraphApiTokenResponseUsingSA = await this.GetAccessTokenUsingServiceAccount(applicationName, moduleName, settings.GetValue<string>("ClientId"), settings.GetValue<string>("TenantId"), settings.GetValue<string>("ExchangeEmail"), settings.GetValue<string>("ExchangePassword"), Resources.MicrosoftGraphApi.Name, scope);
                    }

                    if (this.microsoftGraphApiTokenResponseUsingSA != null)
                    {
                        accessToken = this.microsoftGraphApiTokenResponseUsingSA.Access_Token;
                    }
                }
            }

            return accessToken;
        }

        #endregion

        #region "AAD Graph Api"

        private OAuthTokenResponse aadGraphApiTokenResponseUsingSP = null;
        private OAuthTokenResponse aadGraphApiTokenResponseUsingSA = null;
        public async Task<string> GetAADGraphApiAccessToken(string applicationName, string moduleName, System.Collections.Generic.IDictionary<string, string> settings)
        {
            string accessToken = string.Empty;

            if (settings != null)
            {
                string generateTokenThrough = settings.GetValue<string>("GenerateTokenThrough");

                if (string.IsNullOrEmpty(generateTokenThrough))
                {
                    generateTokenThrough = GenerateTokenThrough.MSI.ToString();
                }

                if (generateTokenThrough == GenerateTokenThrough.MSI.ToString())
                {
                    accessToken = await this.GetAccessTokenUsingMSI(applicationName, moduleName, Resources.AADGraphApi.Name);
                }
                else if (generateTokenThrough == GenerateTokenThrough.ServicePrincipal.ToString())
                {
                    if (this.aadGraphApiTokenResponseUsingSP == null || string.IsNullOrEmpty(this.aadGraphApiTokenResponseUsingSP.Access_Token) || (this.aadGraphApiTokenResponseUsingSP.TokenExpireTime < DateTime.UtcNow))
                    {
                        this.aadGraphApiTokenResponseUsingSP = await this.GetAccessTokenUsingServicePrincipal(applicationName, moduleName, settings.GetValue<string>("ClientId"), settings.GetValue<string>("ClientSecret"), settings.GetValue<string>("TenantId"), Resources.AADGraphApi.Name);
                    }

                    if (this.aadGraphApiTokenResponseUsingSP != null)
                    {
                        accessToken = this.aadGraphApiTokenResponseUsingSP.Access_Token;
                    }
                }
                else if (generateTokenThrough == GenerateTokenThrough.ServiceAccount.ToString())
                {
                    if (this.aadGraphApiTokenResponseUsingSA == null || string.IsNullOrEmpty(this.aadGraphApiTokenResponseUsingSA.Access_Token) || (this.aadGraphApiTokenResponseUsingSA.TokenExpireTime < DateTime.UtcNow))
                    {
                        this.aadGraphApiTokenResponseUsingSA = await this.GetAccessTokenUsingServiceAccount(applicationName, moduleName, settings.GetValue<string>("ClientId"), settings.GetValue<string>("TenantId"), settings.GetValue<string>("ExchangeEmail"), settings.GetValue<string>("ExchangePassword"), Resources.AADGraphApi.Name, scope);
                    }

                    if (this.aadGraphApiTokenResponseUsingSA != null)
                    {
                        accessToken = this.aadGraphApiTokenResponseUsingSA.Access_Token;
                    }
                }
            }

            return accessToken;
        }

        #endregion

        #region "Office 365 Management Api"

        private OAuthTokenResponse office365ManagementApiTokenResponseUsingSP = null;
        private OAuthTokenResponse office365ManagementApiTokenResponseUsingSA = null;
        public async Task<string> GetOffice365ManagementApiAccessToken(string applicationName, string moduleName, System.Collections.Generic.IDictionary<string, string> settings)
        {
            string accessToken = string.Empty;

            if (settings != null)
            {
                string generateTokenThrough = settings.GetValue<string>("GenerateTokenThrough");

                if (string.IsNullOrEmpty(generateTokenThrough))
                {
                    generateTokenThrough = GenerateTokenThrough.MSI.ToString();
                }

                if (generateTokenThrough == GenerateTokenThrough.MSI.ToString())
                {
                    accessToken = await this.GetAccessTokenUsingMSI(applicationName, moduleName, Resources.Office365ManagementApi.Name);
                }
                else if (generateTokenThrough == GenerateTokenThrough.ServicePrincipal.ToString())
                {
                    if (this.office365ManagementApiTokenResponseUsingSP == null || string.IsNullOrEmpty(this.office365ManagementApiTokenResponseUsingSP.Access_Token) || (this.office365ManagementApiTokenResponseUsingSP.TokenExpireTime < DateTime.UtcNow))
                    {
                        this.office365ManagementApiTokenResponseUsingSP = await this.GetAccessTokenUsingServicePrincipal(applicationName, moduleName, settings.GetValue<string>("ClientId"), settings.GetValue<string>("ClientSecret"), settings.GetValue<string>("TenantId"), Resources.Office365ManagementApi.Name);
                    }

                    if (this.office365ManagementApiTokenResponseUsingSP != null)
                    {
                        accessToken = this.office365ManagementApiTokenResponseUsingSP.Access_Token;
                    }
                }
                else if (generateTokenThrough == GenerateTokenThrough.ServiceAccount.ToString())
                {
                    if (this.office365ManagementApiTokenResponseUsingSA == null || string.IsNullOrEmpty(this.office365ManagementApiTokenResponseUsingSA.Access_Token) || (this.office365ManagementApiTokenResponseUsingSA.TokenExpireTime < DateTime.UtcNow))
                    {
                        this.office365ManagementApiTokenResponseUsingSA = await this.GetAccessTokenUsingServiceAccount(applicationName, moduleName, settings.GetValue<string>("ClientId"), settings.GetValue<string>("TenantId"), settings.GetValue<string>("ExchangeEmail"), settings.GetValue<string>("ExchangePassword"), Resources.Office365ManagementApi.Name, scope);
                    }

                    if (this.office365ManagementApiTokenResponseUsingSA != null)
                    {
                        accessToken = this.office365ManagementApiTokenResponseUsingSA.Access_Token;
                    }
                }
            }

            return accessToken;
        }

        #endregion

        #region "Log Analytics"

        private OAuthTokenResponse logAnalyticsTokenResponseUsingSP = null;
        private OAuthTokenResponse logAnalyticsTokenResponseUsingSA = null;
        public async Task<string> GetLogAnalyticsAccessToken(string applicationName, string moduleName, System.Collections.Generic.IDictionary<string, string> settings)
        {
            string accessToken = string.Empty;

            if (settings != null)
            {
                string generateTokenThrough = settings.GetValue<string>("GenerateTokenThrough");

                if (string.IsNullOrEmpty(generateTokenThrough))
                {
                    generateTokenThrough = GenerateTokenThrough.MSI.ToString();
                }

                if (generateTokenThrough == GenerateTokenThrough.MSI.ToString())
                {
                    accessToken = await this.GetAccessTokenUsingMSI(applicationName, moduleName, Resources.LogAnalyticsApi.Name);
                }
                else if (generateTokenThrough == GenerateTokenThrough.ServicePrincipal.ToString())
                {
                    if (this.logAnalyticsTokenResponseUsingSP == null || string.IsNullOrEmpty(this.logAnalyticsTokenResponseUsingSP.Access_Token) || (this.logAnalyticsTokenResponseUsingSP.TokenExpireTime < DateTime.UtcNow))
                    {
                        this.logAnalyticsTokenResponseUsingSP = await this.GetAccessTokenUsingServicePrincipal(applicationName, moduleName, settings.GetValue<string>("ClientId"), settings.GetValue<string>("ClientSecret"), settings.GetValue<string>("TenantId"), Resources.LogAnalyticsApi.Name);
                    }

                    if (this.logAnalyticsTokenResponseUsingSP != null)
                    {
                        accessToken = this.logAnalyticsTokenResponseUsingSP.Access_Token;
                    }
                }
                else if (generateTokenThrough == GenerateTokenThrough.ServiceAccount.ToString())
                {
                    if (this.logAnalyticsTokenResponseUsingSA == null || string.IsNullOrEmpty(this.logAnalyticsTokenResponseUsingSA.Access_Token) || (this.logAnalyticsTokenResponseUsingSA.TokenExpireTime < DateTime.UtcNow))
                    {
                        this.logAnalyticsTokenResponseUsingSA = await this.GetAccessTokenUsingServiceAccount(applicationName, moduleName, settings.GetValue<string>("ClientId"), settings.GetValue<string>("TenantId"), settings.GetValue<string>("ExchangeEmail"), settings.GetValue<string>("ExchangePassword"), Resources.LogAnalyticsApi.Name, scope);
                    }

                    if (this.logAnalyticsTokenResponseUsingSA != null)
                    {
                        accessToken = this.logAnalyticsTokenResponseUsingSA.Access_Token;
                    }
                }
            }

            return accessToken;
        }

        #endregion

        #region "Azure DataLake"

        private OAuthTokenResponse azureDataLakeApiTokenResponseUsingSP = null;
        private OAuthTokenResponse azureDataLakeApiTokenResponseUsingSA = null;
        public async Task<string> GetAzureDataLakeApiAccessToken(string applicationName, string moduleName, System.Collections.Generic.IDictionary<string, string> settings)
        {
            string accessToken = string.Empty;

            if (settings != null)
            {
                string generateTokenThrough = settings.GetValue<string>("GenerateTokenThrough");

                if (string.IsNullOrEmpty(generateTokenThrough))
                {
                    generateTokenThrough = GenerateTokenThrough.MSI.ToString();
                }

                if (generateTokenThrough == GenerateTokenThrough.MSI.ToString())
                {
                    accessToken = await this.GetAccessTokenUsingMSI(applicationName, moduleName, Resources.AzureDataLakeApi.Name);
                }
                else if (generateTokenThrough == GenerateTokenThrough.ServicePrincipal.ToString())
                {
                    if (this.azureDataLakeApiTokenResponseUsingSP == null || string.IsNullOrEmpty(this.azureDataLakeApiTokenResponseUsingSP.Access_Token) || (this.azureDataLakeApiTokenResponseUsingSP.TokenExpireTime < DateTime.UtcNow))
                    {
                        this.azureDataLakeApiTokenResponseUsingSP = await this.GetAccessTokenUsingServicePrincipal(applicationName, moduleName, settings.GetValue<string>("ClientId"), settings.GetValue<string>("ClientSecret"), settings.GetValue<string>("TenantId"), Resources.AzureDataLakeApi.Name);
                    }

                    if (this.azureDataLakeApiTokenResponseUsingSP != null)
                    {
                        accessToken = this.azureDataLakeApiTokenResponseUsingSP.Access_Token;
                    }
                }
                else if (generateTokenThrough == GenerateTokenThrough.ServiceAccount.ToString())
                {
                    if (this.azureDataLakeApiTokenResponseUsingSA == null || string.IsNullOrEmpty(this.azureDataLakeApiTokenResponseUsingSA.Access_Token) || (this.azureDataLakeApiTokenResponseUsingSA.TokenExpireTime < DateTime.UtcNow))
                    {
                        this.azureDataLakeApiTokenResponseUsingSA = await this.GetAccessTokenUsingServiceAccount(applicationName, moduleName, settings.GetValue<string>("ClientId"), settings.GetValue<string>("TenantId"), settings.GetValue<string>("ExchangeEmail"), settings.GetValue<string>("ExchangePassword"), Resources.AzureDataLakeApi.Name, scope);
                    }

                    if (this.azureDataLakeApiTokenResponseUsingSA != null)
                    {
                        accessToken = this.azureDataLakeApiTokenResponseUsingSA.Access_Token;
                    }
                }
            }

            return accessToken;
        }

        #endregion

        #region "Azure Storage Account"

        private OAuthTokenResponse azureStorageAccountApiTokenResponseUsingSP = null;
        private OAuthTokenResponse azureStorageAccountApiTokenResponseUsingSA = null;
        public async Task<string> GetAzureStorageAccountApiAccessToken(string applicationName, string moduleName, System.Collections.Generic.IDictionary<string, string> settings)
        {
            string accessToken = string.Empty;

            if (settings != null)
            {
                string generateTokenThrough = settings.GetValue<string>("GenerateTokenThrough");

                if (string.IsNullOrEmpty(generateTokenThrough))
                {
                    generateTokenThrough = GenerateTokenThrough.MSI.ToString();
                }

                if (generateTokenThrough == GenerateTokenThrough.MSI.ToString())
                {
                    accessToken = await this.GetAccessTokenUsingMSI(applicationName, moduleName, Resources.AzureStorageAccountApi.Name);
                }
                else if (generateTokenThrough == GenerateTokenThrough.ServicePrincipal.ToString())
                {
                    if (this.azureStorageAccountApiTokenResponseUsingSP == null || string.IsNullOrEmpty(this.azureStorageAccountApiTokenResponseUsingSP.Access_Token) || (this.azureStorageAccountApiTokenResponseUsingSP.TokenExpireTime < DateTime.UtcNow))
                    {
                        this.azureStorageAccountApiTokenResponseUsingSP = await this.GetAccessTokenUsingServicePrincipal(applicationName, moduleName, settings.GetValue<string>("ClientId"), settings.GetValue<string>("ClientSecret"), settings.GetValue<string>("TenantId"), Resources.AzureStorageAccountApi.Name);
                    }

                    if (this.azureStorageAccountApiTokenResponseUsingSP != null)
                    {
                        accessToken = this.azureStorageAccountApiTokenResponseUsingSP.Access_Token;
                    }
                }
                else if (generateTokenThrough == GenerateTokenThrough.ServiceAccount.ToString())
                {
                    if (this.azureStorageAccountApiTokenResponseUsingSA == null || string.IsNullOrEmpty(this.azureStorageAccountApiTokenResponseUsingSA.Access_Token) || (this.azureStorageAccountApiTokenResponseUsingSA.TokenExpireTime < DateTime.UtcNow))
                    {
                        this.azureStorageAccountApiTokenResponseUsingSA = await this.GetAccessTokenUsingServiceAccount(applicationName, moduleName, settings.GetValue<string>("ClientId"), settings.GetValue<string>("TenantId"), settings.GetValue<string>("ExchangeEmail"), settings.GetValue<string>("ExchangePassword"), Resources.AzureStorageAccountApi.Name, scope);
                    }

                    if (this.azureStorageAccountApiTokenResponseUsingSA != null)
                    {
                        accessToken = this.azureStorageAccountApiTokenResponseUsingSA.Access_Token;
                    }
                }
            }

            return accessToken;
        }

        #endregion

        #region "Azure Management Api"

        private OAuthTokenResponse azureManagementApiTokenResponseUsingSP = null;
        private OAuthTokenResponse azureManagementApiTokenResponseUsingSA = null;
        public async Task<string> GetAzureManagementApiAccessToken(string applicationName, string moduleName, System.Collections.Generic.IDictionary<string, string> settings)
        {
            string accessToken = string.Empty;

            if (settings != null)
            {
                string generateTokenThrough = settings.GetValue<string>("GenerateTokenThrough");

                if (string.IsNullOrEmpty(generateTokenThrough))
                {
                    generateTokenThrough = GenerateTokenThrough.MSI.ToString();
                }

                if (generateTokenThrough == GenerateTokenThrough.MSI.ToString())
                {
                    accessToken = await this.GetAccessTokenUsingMSI(applicationName, moduleName, Resources.AzureManagementApi.Name);
                }
                else if (generateTokenThrough == GenerateTokenThrough.ServicePrincipal.ToString())
                {
                    if (this.azureManagementApiTokenResponseUsingSP == null || string.IsNullOrEmpty(this.azureManagementApiTokenResponseUsingSP.Access_Token) || (this.azureManagementApiTokenResponseUsingSP.TokenExpireTime < DateTime.UtcNow))
                    {
                        this.azureManagementApiTokenResponseUsingSP = await this.GetAccessTokenUsingServicePrincipal(applicationName, moduleName, settings.GetValue<string>("ClientId"), settings.GetValue<string>("ClientSecret"), settings.GetValue<string>("TenantId"), Resources.AzureManagementApi.Name);
                    }

                    if (this.azureManagementApiTokenResponseUsingSP != null)
                    {
                        accessToken = this.azureManagementApiTokenResponseUsingSP.Access_Token;
                    }
                }
                else if (generateTokenThrough == GenerateTokenThrough.ServiceAccount.ToString())
                {
                    if (this.azureManagementApiTokenResponseUsingSA == null || string.IsNullOrEmpty(this.azureManagementApiTokenResponseUsingSA.Access_Token) || (this.azureManagementApiTokenResponseUsingSA.TokenExpireTime < DateTime.UtcNow))
                    {
                        this.azureManagementApiTokenResponseUsingSA = await this.GetAccessTokenUsingServiceAccount(applicationName, moduleName, settings.GetValue<string>("ClientId"), settings.GetValue<string>("TenantId"), settings.GetValue<string>("ExchangeEmail"), settings.GetValue<string>("ExchangePassword"), Resources.AzureManagementApi.Name, scope);
                    }

                    if (this.azureManagementApiTokenResponseUsingSA != null)
                    {
                        accessToken = this.azureManagementApiTokenResponseUsingSA.Access_Token;
                    }
                }
            }

            return accessToken;
        }

        #endregion

        #region "Dynamics 365"

        private OAuthTokenResponse dynamic365TokenResponseUsingSP = null;
        private OAuthTokenResponse dynamic365TokenResponseUsingSA = null;
        public async Task<string> GetDynamic365ApiAccessToken(string applicationName, string moduleName, System.Collections.Generic.IDictionary<string, string> settings)
        {
            string accessToken = string.Empty;

            if (settings != null)
            {
                string generateTokenThrough = settings.GetValue<string>("GenerateTokenThrough");

                if (string.IsNullOrEmpty(generateTokenThrough))
                {
                    generateTokenThrough = GenerateTokenThrough.MSI.ToString();
                }

                if (generateTokenThrough == GenerateTokenThrough.MSI.ToString())
                {
                    accessToken = await this.GetAccessTokenUsingMSI(applicationName, moduleName, Resources.Dynamics365Api.Name);
                }
                else if (generateTokenThrough == GenerateTokenThrough.ServicePrincipal.ToString())
                {
                    if (this.dynamic365TokenResponseUsingSP == null || string.IsNullOrEmpty(this.dynamic365TokenResponseUsingSP.Access_Token) || (this.dynamic365TokenResponseUsingSP.TokenExpireTime < DateTime.UtcNow))
                    {
                        this.dynamic365TokenResponseUsingSP = await this.GetAccessTokenUsingServicePrincipal(applicationName, moduleName, settings.GetValue<string>("ClientId"), settings.GetValue<string>("ClientSecret"), settings.GetValue<string>("TenantId"), Resources.Dynamics365Api.Name);
                    }

                    if (this.dynamic365TokenResponseUsingSP != null)
                    {
                        accessToken = this.dynamic365TokenResponseUsingSP.Access_Token;
                    }
                }
                else if (generateTokenThrough == GenerateTokenThrough.ServiceAccount.ToString())
                {
                    if (this.dynamic365TokenResponseUsingSA == null || string.IsNullOrEmpty(this.dynamic365TokenResponseUsingSA.Access_Token) || (this.dynamic365TokenResponseUsingSA.TokenExpireTime < DateTime.UtcNow))
                    {
                        this.dynamic365TokenResponseUsingSA = await this.GetAccessTokenUsingServiceAccount(applicationName, moduleName, settings.GetValue<string>("ClientId"), settings.GetValue<string>("TenantId"), settings.GetValue<string>("ExchangeEmail"), settings.GetValue<string>("ExchangePassword"), Resources.Dynamics365Api.Name, scope);
                    }

                    if (this.dynamic365TokenResponseUsingSA != null)
                    {
                        accessToken = this.dynamic365TokenResponseUsingSA.Access_Token;
                    }
                }
            }

            return accessToken;
        }

        #endregion

        #region "Azure Information Protection"

        private OAuthTokenResponse aipTokenResponseUsingSP = null;
        private OAuthTokenResponse aipTokenResponseUsingSA = null;
        public async Task<string> GetAIPApiAccessToken(string applicationName, string moduleName, System.Collections.Generic.IDictionary<string, string> settings)
        {
            string accessToken = string.Empty;

            if (settings != null)
            {
                string generateTokenThrough = settings.GetValue<string>("GenerateTokenThrough");

                if (string.IsNullOrEmpty(generateTokenThrough))
                {
                    generateTokenThrough = GenerateTokenThrough.MSI.ToString();
                }

                if (generateTokenThrough == GenerateTokenThrough.MSI.ToString())
                {
                    accessToken = await this.GetAccessTokenUsingMSI(applicationName, moduleName, Resources.AzureRightsManagementApi.Name);
                }
                else if (generateTokenThrough == GenerateTokenThrough.ServicePrincipal.ToString())
                {
                    if (this.aipTokenResponseUsingSP == null || string.IsNullOrEmpty(this.aipTokenResponseUsingSP.Access_Token) || (this.aipTokenResponseUsingSP.TokenExpireTime < DateTime.UtcNow))
                    {
                        this.aipTokenResponseUsingSP = await this.GetAccessTokenUsingServicePrincipal(applicationName, moduleName, Resources.AzureRightsManagementApi.AppId, null, settings.GetValue<string>("ClientId"), Resources.AzureRightsManagementApi.Name);
                    }

                    if (this.aipTokenResponseUsingSP != null)
                    {
                        accessToken = this.aipTokenResponseUsingSP.Access_Token;
                    }
                }
                else if (generateTokenThrough == GenerateTokenThrough.ServiceAccount.ToString())
                {
                    if (this.aipTokenResponseUsingSA == null || string.IsNullOrEmpty(this.aipTokenResponseUsingSA.Access_Token) || (this.aipTokenResponseUsingSA.TokenExpireTime < DateTime.UtcNow))
                    {
                        this.aipTokenResponseUsingSA = await this.GetAccessTokenUsingServiceAccount(applicationName, moduleName, Resources.AzureRightsManagementApi.AppId, settings.GetValue<string>("ClientId"), settings.GetValue<string>("ExchangeEmail"), settings.GetValue<string>("ExchangePassword"), Resources.AzureRightsManagementApi.Name, null);
                    }

                    if (this.aipTokenResponseUsingSA != null)
                    {
                        accessToken = this.aipTokenResponseUsingSA.Access_Token;
                    }
                }
            }

            return accessToken;
        }

        #endregion

        #region "Windows Defender ATP"

        private OAuthTokenResponse windowsDefenderAtpTokenResponseUsingSP = null;
        private OAuthTokenResponse windowsDefenderAtpTokenResponseUsingSA = null;
        public async Task<string> GetWindowsDefenderATPApiAccessToken(string applicationName, string moduleName, System.Collections.Generic.IDictionary<string, string> settings)
        {
            string accessToken = string.Empty;

            if (settings != null)
            {
                string generateTokenThrough = settings.GetValue<string>("GenerateTokenThrough");

                if (string.IsNullOrEmpty(generateTokenThrough))
                {
                    generateTokenThrough = GenerateTokenThrough.MSI.ToString();
                }

                if (generateTokenThrough == GenerateTokenThrough.MSI.ToString())
                {
                    accessToken = await this.GetAccessTokenUsingMSI(applicationName, moduleName, Resources.WindowsDefenderATP.Name);
                }
                else if (generateTokenThrough == GenerateTokenThrough.ServicePrincipal.ToString())
                {
                    if (this.windowsDefenderAtpTokenResponseUsingSP == null || string.IsNullOrEmpty(this.windowsDefenderAtpTokenResponseUsingSP.Access_Token) || (this.windowsDefenderAtpTokenResponseUsingSP.TokenExpireTime < DateTime.UtcNow))
                    {
                        this.windowsDefenderAtpTokenResponseUsingSP = await this.GetAccessTokenUsingServicePrincipal(applicationName, moduleName, settings.GetValue<string>("ClientId"), settings.GetValue<string>("ClientSecret"), settings.GetValue<string>("TenantId"), Resources.WindowsDefenderATP.Name);
                    }

                    if (this.windowsDefenderAtpTokenResponseUsingSP != null)
                    {
                        accessToken = this.windowsDefenderAtpTokenResponseUsingSP.Access_Token;
                    }
                }
                else if (generateTokenThrough == GenerateTokenThrough.ServiceAccount.ToString())
                {
                    if (this.windowsDefenderAtpTokenResponseUsingSA == null || string.IsNullOrEmpty(this.windowsDefenderAtpTokenResponseUsingSA.Access_Token) || (this.windowsDefenderAtpTokenResponseUsingSA.TokenExpireTime < DateTime.UtcNow))
                    {
                        this.windowsDefenderAtpTokenResponseUsingSA = await this.GetAccessTokenUsingServiceAccount(applicationName, moduleName, settings.GetValue<string>("ClientId"), settings.GetValue<string>("TenantId"), settings.GetValue<string>("ExchangeEmail"), settings.GetValue<string>("ExchangePassword"), Resources.WindowsDefenderATP.Name, scope);
                    }

                    if (this.windowsDefenderAtpTokenResponseUsingSA != null)
                    {
                        accessToken = this.windowsDefenderAtpTokenResponseUsingSA.Access_Token;
                    }
                }
            }

            return accessToken;
        }

        #endregion

        #region "PowerBI Api"

        private OAuthTokenResponse powerBiTokenResponseUsingSP = null;
        private OAuthTokenResponse powerBiTokenResponseUsingSA = null;
        public async Task<string> GetPowerBIApiAccessToken(string applicationName, string moduleName, System.Collections.Generic.IDictionary<string, string> settings)
        {
            string accessToken = string.Empty;

            if (settings != null)
            {
                string generateTokenThrough = settings.GetValue<string>("GenerateTokenThrough");

                if (string.IsNullOrEmpty(generateTokenThrough))
                {
                    generateTokenThrough = GenerateTokenThrough.MSI.ToString();
                }

                if (generateTokenThrough == GenerateTokenThrough.MSI.ToString())
                {
                    accessToken = await this.GetAccessTokenUsingMSI(applicationName, moduleName, Resources.PowerBiApi.Name);
                }
                else if (generateTokenThrough == GenerateTokenThrough.ServicePrincipal.ToString())
                {
                    if (this.powerBiTokenResponseUsingSP == null || string.IsNullOrEmpty(this.powerBiTokenResponseUsingSP.Access_Token) || (this.powerBiTokenResponseUsingSP.TokenExpireTime < DateTime.UtcNow))
                    {
                        this.powerBiTokenResponseUsingSP = await this.GetAccessTokenUsingServicePrincipal(applicationName, moduleName, settings.GetValue<string>("ClientId"), settings.GetValue<string>("ClientSecret"), settings.GetValue<string>("TenantId"), Resources.PowerBiApi.Name);
                    }

                    if (this.powerBiTokenResponseUsingSP != null)
                    {
                        accessToken = this.powerBiTokenResponseUsingSP.Access_Token;
                    }
                }
                else if (generateTokenThrough == GenerateTokenThrough.ServiceAccount.ToString())
                {
                    if (this.powerBiTokenResponseUsingSA == null || string.IsNullOrEmpty(this.powerBiTokenResponseUsingSA.Access_Token) || (this.powerBiTokenResponseUsingSA.TokenExpireTime < DateTime.UtcNow))
                    {
                        this.powerBiTokenResponseUsingSA = await this.GetAccessTokenUsingServiceAccount(applicationName, moduleName, settings.GetValue<string>("ClientId"), settings.GetValue<string>("TenantId"), settings.GetValue<string>("ExchangeEmail"), settings.GetValue<string>("ExchangePassword"), Resources.PowerBiApi.Name, scope);
                    }

                    if (this.powerBiTokenResponseUsingSA != null)
                    {
                        accessToken = this.powerBiTokenResponseUsingSA.Access_Token;
                    }
                }
            }

            return accessToken;
        }

        #endregion

        #region "Dynamic CRM"

        #endregion

        #region "Azure Reghts Management"

        #endregion

        #region "Salesforce Token"

        public async Task<OAuthSalesforceTokenResponse> GetSalesforceApiAccessToken(string applicationName, string moduleName, System.Collections.Generic.IDictionary<string, string> settings)
        {
            OAuthSalesforceTokenResponse tokenResponse = null;

            try
            {
                if (settings != null)
                {
                    using (var client = new HttpClient())
                    {
                        FormUrlEncodedContent httpContent = null;

                        httpContent = new FormUrlEncodedContent(PayloadHelper.Instance.GetSalesforceAccessTokenPayload(settings.GetValue<string>("SalesforceClientId"), settings.GetValue<string>("SalesforceClientSecret"), settings.GetValue<string>("SalesforceUserName"), settings.GetValue<string>("SalesforceUserPassword"), settings.GetValue<string>("SalesforceSecurityToken")));

                        httpContent.Headers.Add("X-PrettyPrint", "1");

                        var endpoint = $"https://{settings.GetValue<string>("SalesforceEnvironment")}.salesforce.com/services/oauth2/token";

                        using (var response = await client.PostAsync(endpoint, httpContent))
                        {
                            if (response != null)
                            {
                                var result = await response.Content.ReadAsStringAsync();

                                if (!string.IsNullOrEmpty(result))
                                {
                                    tokenResponse = Newtonsoft.Json.JsonConvert.DeserializeObject<OAuthSalesforceTokenResponse>(result);
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                var message = $"Exception: Exception occured while generating Salesforce api access token for Environment {settings.GetValue<string>("SalesforceEnvironment")}";
                ApplicationInsightsHelper.Instance.LogException(applicationName, moduleName, fileName, "GetSalesforceApiAccessToken", ex, message);
            }

            return tokenResponse;
        }

        #endregion
    }
}
