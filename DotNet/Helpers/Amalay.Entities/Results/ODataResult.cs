using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Amalay.Entities
{
    public class ODataResultBase<T>
    {
        [JsonProperty("@odata.context")]
        public string ODataContext { get; set; }

        [JsonProperty("@odata.nextLink")]
        public string ODataNextLink { get; set; }
    }

    public class ODataResult<T> : ODataResultBase<T>
    {
        public T Value { get; set; }
    }

    public class ODataResult_Paging<T> : ODataResultBase<T>
    {
        public List<T> Value { get; set; }
    }
}
