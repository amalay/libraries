using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Amalay.Framework
{
    public class AuthenticationHelper
    {                
        private string message = string.Empty;

        #region "Singleton"

        private static readonly AuthenticationHelper instance = new AuthenticationHelper();

        public AuthenticationHelper() { }

        public static AuthenticationHelper Instance
        {
            get
            {
                return instance;
            }
        }

        #endregion
    }
}
