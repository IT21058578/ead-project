using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace api.Exceptions
{
    public class NotFoundException(string message) : BaseException(message, HttpStatusCode.NotFound, "Not Found")
    {
        
    }
}