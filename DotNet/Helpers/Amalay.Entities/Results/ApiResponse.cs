using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Amalay.Entities
{
    public class ApiResponse<T>
    {
        public System.Net.HttpStatusCode StatusCode { get; set; }

        public T Data { get; set; }

        public string Message { get; set; }

        public string Endpoint { get; set; }

        public bool IsSuccess { get; set; }

        public bool HasNext { get; set; }

        public int Total { get; set; }
    }

    public class ApiResponse_Paging<T>
    {
        public System.Net.HttpStatusCode StatusCode { get; set; }

        public List<T> Data { get; set; }

        public string Message { get; set; }

        public string Endpoint { get; set; }

        public bool IsSuccess { get; set; }

        public bool HasNext { get; set; }

        public int Total { get; set; }
    }
}
