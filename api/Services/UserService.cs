using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.DTOs.Requests;
using api.Models;
using api.Repositories;
using api.Utilities;

namespace api.Services
{
    public class UserService(UserRepository userRepository, Logger<UserService> logger)
    {
        private readonly Logger<UserService> _logger = logger;
        private readonly UserRepository _userRepository = userRepository;

        public User GetUser(string id)
        {
            _logger.LogInformation("Getting user {id}", id);
            var user = _userRepository.GetById(id) ?? throw new Exception("User not found");
            _logger.LogInformation("User found with id {id}", user.Id);
            return user;
        }

        public Page<User> SearchUsers(PageRequest<User> request)
        {
            _logger.LogInformation("Searching users with page {page} and page size {pageSize}", request.Page, request.PageSize);
            var users = _userRepository.GetPage(request);
            _logger.LogInformation("Found {count} users", users.Data.Count());
            return users;
        }
    }
}