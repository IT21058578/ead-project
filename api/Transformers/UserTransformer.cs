using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.DTOs.Models;
using api.Models;

namespace api.Transformers
{
    public static class UserTransformer
    {
        // This is a method for transforming a CreateUserRequestDto to a User model
        public static User ToModel(this DTOs.Requests.CreateUserRequestDto request)
        {
            return new User
            {
                Email = request.Email,
                FirstName = request.FirstName,
                LastName = request.LastName,
            };
        }

        // This is a method for transforming a User model to a UserDto
        public static UserDto ToDto(this User model)
        {
            return new UserDto
            {
                Id = model.Id.ToString(),
                Email = model.Email,
                FirstName = model.FirstName,
                LastName = model.LastName,
                CreatedAt = model.CreatedAt,
                CreatedBy = model.CreatedBy.ToString(),
                UpdatedAt = model.UpdatedAt,
                UpdatedBy = model.UpdatedBy.ToString(),
                IsApproved = model.IsApproved,
                IsVerified = model.IsVerified,
                Rating = model.Rating,
                Role = model.Role,
            };
        }

        // This is a method for transforming an UpdateUserRequestDto to a User model
        public static User ToModel(this DTOs.Requests.UpdateUserRequestDto request, User model)
        {
            return new User
            {
                Id = model.Id,
                Email = model.Email,
                FirstName = request.FirstName,
                LastName = request.LastName,
                CreatedAt = model.CreatedAt,
                CreatedBy = model.CreatedBy,
                UpdatedAt = DateTime.UtcNow,
                UpdatedBy = model.UpdatedBy,
                IsApproved = model.IsApproved,
                IsVerified = model.IsVerified,
                Rating = model.Rating,
                Role = model.Role,
            };
        }
    }
}