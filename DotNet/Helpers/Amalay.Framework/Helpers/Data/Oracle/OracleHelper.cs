using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Amalay.Framework
{
    public class  OracleHelper
    {
        private string fileName = "OracleHelper.cs";
        private string message = string.Empty;

        #region "Singleton"

        private static readonly OracleHelper instance = new OracleHelper();

        private OracleHelper() { }

        public static OracleHelper Instance
        {
            get
            {
                return instance;
            }
        }

        #endregion
    }
}
