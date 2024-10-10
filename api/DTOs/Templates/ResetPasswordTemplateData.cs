namespace api.DTOs.Templates
{
    /// <summary>
    /// The ResetPasswordTemplateData class represents the data required for the reset password email template.
    /// </summary>
    /// 
    /// <remarks>
    /// The ResetPasswordTemplateData class is used to store the data required for the reset password email template.
    /// It includes properties for the first name of the user, the reset password link, and the reset password code.
    /// </remarks>
    public class ResetPasswordTemplateData : ITemplateData
    {
        public string FirstName { get; set; } = "";
        public string ResetPasswordLink { get; set; } = "";
        public string ResetPasswordCode { get; set; } = "";
    }
}