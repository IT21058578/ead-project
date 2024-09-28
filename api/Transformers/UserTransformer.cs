using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace api.Transformers
{
    public static class UserTransformer
    {
        public static Models.User ToModel(this DTOs.Requests.CreateUserRequestDto request)
        {
            return new Models.User
            {
                Email = request.Email,
                FirstName = request.FirstName,
                LastName = request.LastName,
            };
        }
    }
}