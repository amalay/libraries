using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Amalay.Entities
{
    public enum AuthType
    {
        NoAuth,
        Basic,
        Digest,
        OAuth,
        Token,
        Bearer,
        MSOID
    }
}
