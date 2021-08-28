using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Amalay.Framework
{
    public class MySqlHelper
    {
        private string fileName = "MySqlHelper.cs";
        private string message = string.Empty;

        #region "Singleton"

        private static readonly MySqlHelper instance = new MySqlHelper();

        private MySqlHelper() { }

        public static MySqlHelper Instance
        {
            get
            {
                return instance;
            }
        }

        #endregion
    }
}
