using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Models;

namespace api.Transformers
{
    public static class UserTransformer
    {
        public static User ToModel(this DTOs.Requests.CreateUserRequestDto request)
        {
            return new User
            {
                Email = request.Email,
                FirstName = request.FirstName,
                LastName = request.LastName,
            };
        }
    }
}