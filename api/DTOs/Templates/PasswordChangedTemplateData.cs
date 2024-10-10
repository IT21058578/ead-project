namespace api.DTOs.Templates
{
    /// <summary>
    /// The PasswordChangedTemplateData class represents the data required for the PasswordChanged email template.
    /// </summary>
    /// 
    /// <remarks>
    /// The PasswordChangedTemplateData class is used to provide the necessary data for the PasswordChanged email template.
    /// It contains the FirstName property which represents the first name of the user.
    /// </remarks>
    public class PasswordChangedTemplateData : ITemplateData
    {
        public string FirstName { get; set; } = "";
    }
}