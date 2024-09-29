using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Utilities;
using FluentEmail.Core;

namespace api.Services
{
    public class EmailService(IFluentEmailFactory emailFactory)
    {
        private readonly IFluentEmailFactory _emailFactory = emailFactory;

        public async void SendEmail(EmailRequest emailRequest)
        {
            var email = _emailFactory.Create();
            email.To(emailRequest.To)
                .Subject(emailRequest.Subject)
                .UsingTemplateFromFile(emailRequest.Template, emailRequest.TemplateData)
                .Send();
            await email.SendAsync();
        }

        public async void SendEmails(List<EmailRequest> emailRequests)
        {
            foreach (var emailRequest in emailRequests)
            {
                SendEmail(emailRequest);
            }
        }
    }
}