using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace api.Exceptions
{
    public class InternalServerErrorException(string message) : BaseException(message, HttpStatusCode.InternalServerError, "Internal Server Error")
    {

    }
}