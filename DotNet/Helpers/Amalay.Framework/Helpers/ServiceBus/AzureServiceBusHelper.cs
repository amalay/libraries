using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Amalay.Framework
{
    public class AzureServiceBusHelper
    {
        private string fileName = "AzureServiceBusHelper.cs";

        #region "Singleton"

        private static readonly AzureServiceBusHelper instance = new AzureServiceBusHelper();

        private AzureServiceBusHelper() { }

        public static AzureServiceBusHelper Instance
        {
            get
            {
                return instance;
            }
        }

        #endregion
    }
}
