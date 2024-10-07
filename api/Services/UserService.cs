using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.DTOs.Requests;
using api.Exceptions;
using api.Models;
using api.Repositories;
using api.Utilities;

namespace api.Services
{
    public class UserService(UserRepository userRepository, ILogger<UserService> logger)
    {
        private readonly ILogger<UserService> _logger = logger;
        private readonly UserRepository _userRepository = userRepository;

        // This is a method for getting a user
        public User GetUser(string id)
        {
            _logger.LogInformation("Getting user {id}", id);
            var user = _userRepository.GetById(id) ?? throw new NotFoundException($"User with id {id} not found");
            _logger.LogInformation("User found with id {id}", user.Id);
            return user;
        }

        // This is a method for updating a user
        public User UpdateUser(string id, UpdateUserRequestDto request)
        {
            _logger.LogInformation("Updating user {id}", id);
            var user = GetUser(id);
            user.FirstName = request.FirstName;
            user.LastName = request.LastName;
            _userRepository.Update(user);
            _logger.LogInformation("User updated with first name {firstName} and last name {lastName}", user.FirstName, user.LastName);
            return user;
        }

        // This is a method for searching for users
        public Page<User> SearchUsers(PageRequest<User> request)
        {
            _logger.LogInformation("Searching users with page {page} and page size {pageSize}", request.Page, request.PageSize);
            var users = _userRepository.GetPage(request);
            _logger.LogInformation("Found {count} users", users.Data.Count());
            return users;
        }

        // This is a method for validating a user
        public bool IsUserValid(string id)
        {
            _logger.LogInformation("Checking whether user {id} is valid", id);
            var user = _userRepository.GetById(id);
            return user != null;
        }

        // This is a method for updating a user's rating
        public async Task<User> UpdateRating(string id, double rating)
        {
            _logger.LogInformation("Updating user rating for user {id}", id);
            var user = GetUser(id);
            user.Rating = rating;
            _userRepository.Update(user);
            _logger.LogInformation("User rating updated to {rating}", rating);
            return user;
        }
    }
}