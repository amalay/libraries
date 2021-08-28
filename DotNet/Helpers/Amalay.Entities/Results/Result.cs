using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Amalay.Entities
{
    public class Result
    {
        public string Status { get; set; }

        public string Message { get; set; }

        public object Data { get; set; }
    }

    public class Result<T>
    {
        public string Status { get; set; }
        
        public string Message { get; set; }        

        public T Data { get; set; }

        public DateTime? Date { get; set; }        
    }
}
