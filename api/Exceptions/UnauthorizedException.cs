using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace api.Exceptions
{
    public class UnauthorizedException(string message) : BaseException(message, HttpStatusCode.Unauthorized, "Unauthorized")
    {

    }
}