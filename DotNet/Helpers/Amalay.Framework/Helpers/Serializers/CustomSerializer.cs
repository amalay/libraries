using Azure.Core.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Amalay.Framework
{
    public class CustomSerializer
    {
        private string fileName = "CustomSerializer.cs";

        #region "Singleton"

        private static readonly CustomSerializer instance = new CustomSerializer();

        private CustomSerializer() { }

        public static CustomSerializer Instance
        {
            get
            {
                return instance;
            }
        }

        #endregion
               
        public JsonObjectSerializer CamelCaseSerializer 
        { 
            get
            {
                return new JsonObjectSerializer(new JsonSerializerOptions()
                {
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                });
            }
        }
    }
}
