using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using api.Configurations;
using api.Models;
using api.Repositories;
using api.Utilities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace api.Services
{
    public class AppUserStore(UserRepository userRepository, CredentialRepository credentialRepository, AppDbContext dbContext)
    : IUserStore<User>, IUserPasswordStore<User>, IUserRoleStore<User>, IQueryableUserStore<User>
    {
        private readonly UserRepository _userRepository = userRepository;
        private readonly CredentialRepository _credentialRepository = credentialRepository;
        protected readonly DbSet<User> _dbSet = dbContext.Set<User>();
        public IQueryable<User> Users => _dbSet;

        public async Task<IdentityResult> CreateAsync(User user, CancellationToken cancellationToken)
        {
            await _userRepository.AddAsync(user);
            return IdentityResult.Success;
        }

        public Task<IdentityResult> DeleteAsync(User user, CancellationToken cancellationToken)
        {
            _userRepository.Delete(user.Id);
            return Task.FromResult(IdentityResult.Success);
        }

        public Task<User?> FindByIdAsync(string userId, CancellationToken cancellationToken)
        {
            var result = _userRepository.GetById(userId);
            return Task.FromResult(result);
        }

        public Task<User?> FindByNameAsync(string normalizedUserName, CancellationToken cancellationToken)
        {
            var result = _userRepository.FindByEmail(normalizedUserName);
            return Task.FromResult(result);
        }

        public Task<string?> GetNormalizedUserNameAsync(User user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.Email)!;
        }

        public Task<string?> GetPasswordHashAsync(User user, CancellationToken cancellationToken)
        {
            var credential = _credentialRepository.FindByUserId(user.Id);
            return Task.FromResult(credential?.Password);
        }

        public Task<string?> GetUserNameAsync(User user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.Email)!;
        }

        public Task<bool> HasPasswordAsync(User user, CancellationToken cancellationToken)
        {
            var credential = _credentialRepository.FindByUserId(user.Id);
            var hasPassword = credential?.Password.IsNullOrEmpty() ?? false;
            return Task.FromResult(hasPassword);
        }

        public Task<string> GetUserIdAsync(User user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.Id.ToString());
        }

        public Task SetNormalizedUserNameAsync(User user, string? normalizedName, CancellationToken cancellationToken)
        {
            if (normalizedName == null)
            {
                return Task.FromException(new Exception("Email / UserName cannot be empty"));
            }
            user.Email = normalizedName;
            // _userRepository.Update(user); TODO: Figure out why this throws in registration
            return Task.CompletedTask;
        }

        public Task SetPasswordHashAsync(User user, string? passwordHash, CancellationToken cancellationToken)
        {
            if (passwordHash == null)
            {
                return Task.FromException(new Exception("Password cannot be empty"));
            }
            var credential = _credentialRepository.FindByUserId(user.Id);
            if (credential != null)
            {
                credential.Password = passwordHash;
                _credentialRepository.Update(credential);
            }
            else
            {
                _credentialRepository.Add(new Credential { UserId = user.Id, Password = passwordHash });
            }

            return Task.CompletedTask;
        }

        public Task SetUserNameAsync(User user, string? userName, CancellationToken cancellationToken)
        {
            if (userName == null)
            {
                return Task.FromException(new Exception("Email / UserName cannot be empty"));
            }
            user.Email = userName;
            _userRepository.Update(user);
            return Task.CompletedTask;
        }

        public Task<IdentityResult> UpdateAsync(User user, CancellationToken cancellationToken)
        {
            _userRepository.Update(user);
            return Task.FromResult(IdentityResult.Success);
        }

        public Task AddToRoleAsync(User user, string roleName, CancellationToken cancellationToken)
        {
            var role = AppUserRoleHelper.ToValue(roleName);
            user.Role = role;
            _userRepository.Update(user);
            return Task.CompletedTask;
        }

        public Task RemoveFromRoleAsync(User user, string roleName, CancellationToken cancellationToken)
        {
            user.Role = AppUserRole.Customer;
            _userRepository.Update(user);
            return Task.CompletedTask;
        }

        public Task<IList<string>> GetRolesAsync(User user, CancellationToken cancellationToken)
        {
            var roles = new List<string> { user.Role.ToString() };
            return Task.FromResult<IList<string>>(roles);
        }

        public Task<bool> IsInRoleAsync(User user, string roleName, CancellationToken cancellationToken)
        {
            var role = AppUserRoleHelper.ToValue(roleName);
            return Task.FromResult(user.Role == role);
        }

        public Task<IList<User>> GetUsersInRoleAsync(string roleName, CancellationToken cancellationToken)
        {
            var role = AppUserRoleHelper.ToValue(roleName);
            var users = _userRepository.FindByRole(role);
            return Task.FromResult(users.ToList() as IList<User>);
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }
    }
}