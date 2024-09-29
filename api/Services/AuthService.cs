using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Models;
using Microsoft.AspNetCore.Identity;

namespace api.Services
{
    public class AuthService(UserManager<User> userManager, ILogger<NotificationService> logger)
    {
        private readonly ILogger<NotificationService> _logger = logger;
        private readonly UserManager<User> _userManager = userManager;
    }
}