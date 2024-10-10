using api.Exceptions;

namespace api.Utilities
{
    /// <summary>
    /// The AppUserRole enum represents the roles that can be assigned to users in the application.
    /// </summary>
    /// 
    /// <remarks>
    /// The AppUserRole enum defines the following roles:
    /// - Customer: Represents a customer role.
    /// - Vendor: Represents a vendor role.
    /// - Csr: Represents a customer service representative role.
    /// - Admin: Represents an admin role.
    /// </remarks>
    public enum AppUserRole
    {
        Customer,
        Vendor,
        Csr,
        Admin
    }

    public static class AppUserRoleHelper
    {
        // This is a method for converting a string to an AppUserRole
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