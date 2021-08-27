using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Amalay.Entities
{
    public class OAuthTokenResponse
    {
        public OAuthTokenResponse()
        {
            this.TokenGeneratedTime = DateTime.UtcNow;
            this.TokenExpireTime = this.TokenGeneratedTime;
        }

        public string Access_Token { get; set; }

        public string Token_Type { get; set; }

        public string Expires_On { get; set; }

        public string _Expires_In = string.Empty;
        public string Expires_In
        {
            get
            {
                return this._Expires_In;
            }
            set
            {
                this._Expires_In = value;

                if (!string.IsNullOrWhiteSpace(this._Expires_In))
                {
                    this.TokenExpireTime = this.TokenExpireTime.AddSeconds(double.Parse(this.Expires_In) - 120); //Set token expire time before 2 minute.
                }
            }
        }

        public string Ext_Expires_In { get; set; }

        public string Not_Before { get; set; }

        public string Resource { get; set; }

        //-----------------------
        public DateTime TokenGeneratedTime { get; private set; }

        public DateTime TokenExpireTime { get; private set; }
    }

    public class OAuthSalesforceTokenResponse
    {
        public OAuthSalesforceTokenResponse()
        {
            this.TokenGeneratedTime = DateTime.UtcNow;
        }

        public string Id { get; set; }

        public string Access_Token { get; set; }

        public string Token_Type { get; set; }

        public string Instance_Url { get; set; }

        public double Issued_At { get; set; } //Timestamp in second since 1970/01/01 Midnight (e.g 1970, 1, 1, 0, 0, 0, 0)

        public string Signature { get; set; }

        public DateTime TokenGeneratedTime { get; private set; }

        public DateTime TokenExpireTime { get; set; }

        public string Environment { get; set; }
    }
}
