using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Amalay.Framework
{
    public class StorageAccountHelper
    {
        private string fileName = "StorageAccountHelper.cs";
        private string message = string.Empty;

        #region "Singleton"

        private static readonly StorageAccountHelper instance = new StorageAccountHelper();

        private StorageAccountHelper() { }

        public static StorageAccountHelper Instance
        {
            get
            {
                return instance;
            }
        }

        #endregion
    }
}
