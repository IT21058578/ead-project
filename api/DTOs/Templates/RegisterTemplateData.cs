namespace api.DTOs.Templates
{
    /// <summary>
    /// The RegisterTemplateData class represents the data required for registering a user.
    /// </summary>
    /// 
    /// <remarks>
    /// The RegisterTemplateData class is used to store the first name, verification code, and verification link for a user registration process.
    /// </remarks>
    public class RegisterTemplateData : ITemplateData
    {
        public string FirstName { get; set; } = "";
        public string VerificationCode { get; set; } = "";
        public string VerificationLink { get; set; } = "";
    }
}