using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Amalay.Entities
{
    public interface ISetting
    {
        public System.Collections.Generic.IDictionary<string, string> Settings { get; }
    }
}
