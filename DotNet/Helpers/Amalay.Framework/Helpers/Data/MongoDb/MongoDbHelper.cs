using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Amalay.Framework
{
    public class MongoDbHelper
    {
        private string fileName = "MongoDbHelper.cs";
        private string message = string.Empty;

        #region "Singleton"

        private static readonly MongoDbHelper instance = new MongoDbHelper();

        private MongoDbHelper() { }

        public static MongoDbHelper Instance
        {
            get
            {
                return instance;
            }
        }

        #endregion
    }
}
