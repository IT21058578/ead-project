
using api.DTOs.Templates;
using api.Utilities;
using FluentEmail.Core;

namespace api.Services
{
    /// <summary>
    /// The EmailService class provides functionality for sending emails.
    /// </summary>
    /// 
    /// <remarks>
    /// The EmailService class is responsible for sending emails using the provided email factory and logger.
    /// It contains methods for sending a single email and multiple emails.
    /// The SendEmail method sends an email using the specified email request, including the recipient, subject, template, and template data.
    /// The SendEmails method sends multiple emails using a list of email requests.
    /// </remarks>
    public class EmailService(IFluentEmailFactory emailFactory, ILogger<EmailService> logger)
    {
        private readonly ILogger<EmailService> _logger = logger;
        private readonly IFluentEmailFactory _emailFactory = emailFactory;

        // This method sends an email
        public async Task SendEmail<T>(EmailRequest<T> emailRequest) where T : ITemplateData
        {
            _logger.LogInformation("Sending email to {To} with subject {Subject}", emailRequest.To, emailRequest.Subject);
            var email = _emailFactory.Create();
            email
                .To(emailRequest.To)
                .Subject(emailRequest.Subject)
                .UsingTemplateFromFile(
                    Path.Combine(
                        Directory.GetCurrentDirectory(),
                        "Assets",
                        "Templates",
                        emailRequest.TemplateName.ToString() + ".cshtml"),
                    emailRequest.TemplateData);
            await email.SendAsync();
            _logger.LogInformation("Email to {To} with subject {Subject} has been sent", emailRequest.To, emailRequest.Subject);
        }

        // This method sends multiple emails
        public async Task SendEmails(List<EmailRequest<ITemplateData>> emailRequests)
        {
            _logger.LogInformation("Sending {Count} emails", emailRequests.Count);
            var tasks = emailRequests.Select(SendEmail).ToList();
            await Task.WhenAll(tasks);
            _logger.LogInformation("{Count} emails have been sent", emailRequests.Count);
        }
    }
}