
namespace api.DTOs.Templates
{
    /// <summary>
    /// The EmailTemplate enum represents the different types of email templates available.
    /// </summary>
    /// 
    /// <remarks>
    /// The EmailTemplate enum is used to specify the type of email template to be used.
    /// The available options are Registration, ResetPassword, and PasswordChanged.
    /// </remarks>
    public enum EmailTemplate
    {
        Registration,
        ResetPassword,
        PasswordChanged
    }
}