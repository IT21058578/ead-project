using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.DTOs.Templates;
using api.Utilities;
using FluentEmail.Core;

namespace api.Services
{
    public class EmailService(IFluentEmailFactory emailFactory, ILogger<EmailService> logger)
    {
        private readonly ILogger<EmailService> _logger = logger;
        private readonly IFluentEmailFactory _emailFactory = emailFactory;

        public async Task SendEmail<T>(EmailRequest<T> emailRequest) where T : ITemplateData
        {
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
        }

        public async Task SendEmails(List<EmailRequest<ITemplateData>> emailRequests)
        {

            var tasks = emailRequests.Select(SendEmail).ToList();
            await Task.WhenAll(tasks);
        }
    }
}