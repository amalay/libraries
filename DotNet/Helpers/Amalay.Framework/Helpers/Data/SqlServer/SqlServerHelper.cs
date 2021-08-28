using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Amalay.Framework
{
    public class SqlServerHelper
    {
        private string fileName = "SqlServerHelper.cs";
        private string message = string.Empty;

        #region "Singleton"

        private static readonly SqlServerHelper instance = new SqlServerHelper();

        private SqlServerHelper() { }

        public static SqlServerHelper Instance
        {
            get
            {
                return instance;
            }
        }

        #endregion
    }
}
