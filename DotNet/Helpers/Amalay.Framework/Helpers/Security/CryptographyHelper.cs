using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Amalay.Framework
{
    public class CryptographyHelper
    {
        private string fileName = "CryptographyHelper.cs";

        #region "Singleton"

        private static readonly CryptographyHelper instance = new CryptographyHelper();

        private CryptographyHelper() { }

        public static CryptographyHelper Instance
        {
            get
            {
                return instance;
            }
        }

        #endregion

        #region "Public Methods"

        /// <summary>
        /// Method to return the masked value of a string.
        /// </summary>
        /// <param name="inputString"></param>
        /// <returns></returns>
        public string GetMaskedValue(string inputString)
        {
            StringBuilder sb = new StringBuilder();

            if (!string.IsNullOrEmpty(inputString))
            {
                var algorithm = System.Security.Cryptography.SHA512.Create();  //or use SHA256.Create();
                var hashedValue = algorithm.ComputeHash(Encoding.UTF8.GetBytes(inputString));

                if (hashedValue != null && hashedValue.Length > 0)
                {
                    foreach (byte b in hashedValue)
                    {
                        sb.Append(b.ToString("X2"));
                    }
                }
            }

            return sb.ToString();
        }

        /// <summary>
        /// Method to return list of masked value. It accepts key & its value in plain text and return the same key with its masked value.
        /// </summary>
        /// <param name="inputList"></param>
        /// <returns></returns>
        public Dictionary<string, string> GetMaskedValue(Dictionary<string, string> inputList)
        {
            Dictionary<string, string> maskedList = null;

            if (inputList != null && inputList.Count > 0)
            {
                maskedList = new Dictionary<string, string>();

                foreach (var item in inputList)
                {
                    if (!string.IsNullOrEmpty(item.Value))
                    {
                        var algorithm = System.Security.Cryptography.SHA512.Create();  //or use SHA256.Create();
                        var hashedValue = algorithm.ComputeHash(Encoding.UTF8.GetBytes(item.Value));

                        if (hashedValue != null && hashedValue.Length > 0)
                        {
                            StringBuilder sb = new StringBuilder();

                            foreach (byte b in hashedValue)
                            {
                                sb.Append(b.ToString("X2"));
                            }

                            maskedList.Add(item.Key, sb.ToString());
                        }
                    }
                }
            }

            return maskedList;
        }

        #endregion
    }
}
