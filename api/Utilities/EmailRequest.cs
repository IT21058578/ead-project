using api.DTOs.Templates;

namespace api.Utilities
{
    /// <summary>
    /// The EmailRequest<T> class represents an email request.
    /// </summary>
    /// 
    /// <typeparam name="T">The type of template data.</typeparam>
    /// 
    /// <remarks>
    /// The EmailRequest<T> class is used to send an email with the specified template and template data.
    /// It contains properties for the recipient email address, subject, sender email address,
    /// template name, and template data.
    /// </remarks>
    public class EmailRequest<T> where T : ITemplateData
    {
        public string To { get; set; } = "";
        public string Subject { get; set; } = "";
        public string From { get; set; } = "";
        public EmailTemplate TemplateName { get; set; } = EmailTemplate.Registration;
        public T? TemplateData { get; set; }
    }
}