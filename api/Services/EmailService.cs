using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Utilities;
using FluentEmail.Core;

namespace api.Services
{
    public class EmailService(IFluentEmailFactory emailFactory, ILogger<EmailService> logger)
    {
        private readonly ILogger<EmailService> _logger = logger;
        private readonly IFluentEmailFactory _emailFactory = emailFactory;

        public async Task SendEmail(EmailRequest emailRequest)
        {
            var email = _emailFactory.Create();
            Console.WriteLine("Sending email to " + emailRequest.To);
            email
                .To(emailRequest.To)
                .Subject(emailRequest.Subject)
                .UsingTemplateFromFile(
                    Path.Combine(
                        Directory.GetCurrentDirectory(),
                        "Assets",
                        "Templates",
                        emailRequest.TemplateName.ToString() + ".liquid"),
                    emailRequest.TemplateData);
            await email.SendAsync();
        }

        public async Task SendEmails(List<EmailRequest> emailRequests)
        {

            var tasks = emailRequests.Select(SendEmail).ToList();
            await Task.WhenAll(tasks);
        }
    }
}