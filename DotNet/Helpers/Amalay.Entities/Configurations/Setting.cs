using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Amalay.Entities
{
    public class Setting
    {
        private System.Collections.Generic.IDictionary<string, string> _AppSettings = null;
        private System.Collections.Generic.IDictionary<string, string> _EnvironmentSettings = null;
        private System.Collections.Generic.IDictionary<string, string> _CustomerSettings = null;

        #region "Singleton"

        private static readonly Setting instance = new Setting();

        private Setting()
        {
            this._AppSettings = new Dictionary<string, string>();
            this._EnvironmentSettings = new Dictionary<string, string>();
            this._CustomerSettings = new Dictionary<string, string>();
        }

        public static Setting Instance
        {
            get
            {
                return instance;
            }
        }

        #endregion

        public System.Collections.Generic.IDictionary<string, string> AppSettings
        {
            get
            {
                return this._AppSettings;
            }
        }

        public System.Collections.Generic.IDictionary<string, string> EnvironmentSettings
        {
            get
            {
                return this._EnvironmentSettings;
            }
        }

        public System.Collections.Generic.IDictionary<string, string> CustomerSettings
        {
            get
            {
                return this._CustomerSettings;
            }
        }
    }
}
