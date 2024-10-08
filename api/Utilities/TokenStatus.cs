using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace api.Utilities
{
    /// <summary>
    /// The TokenStatus enum represents the status of a token.
    /// </summary>
    /// 
    /// <remarks>
    /// The TokenStatus enum is used to indicate the status of a token.
    /// It can have the following values: Active, Revoked, Claimed, Expired.
    /// </remarks>
    public enum TokenStatus
    {
        Active,
        Revoked,
        Claimed,
        Expired,
    }
}