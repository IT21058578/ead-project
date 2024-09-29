using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace api.Utilities
{
    public enum TokenStatus
    {
        Active,
        Revoked,
        Claimed,
        Expired,
    }
}