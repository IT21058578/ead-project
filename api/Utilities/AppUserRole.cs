using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Exceptions;

namespace api.Utilities
{
    public enum AppUserRole
    {
        Customer,
        Vendor,
        Csr,
        Admin
    }

    public static class AppUserRoleHelper
    {
        public static AppUserRole ToValue(string role)
        {
            return role.ToLower() switch
            {
                "customer" => AppUserRole.Customer,
                "vendor" => AppUserRole.Vendor,
                "csr" => AppUserRole.Csr,
                "admin" => AppUserRole.Admin,
                _ => throw new UnauthorizedException("Invalid role")
            };
        }
    }
}