using AlertService.Services.Interfaces;
using SendGrid;
using SendGrid.Helpers.Mail;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AlertService.Services
{
    class AlertSenderService : IAlertSenderService
    {
        private readonly string sendgridApiKey;
        private readonly string fromAddress;
        private readonly string fromName;

        public AlertSenderService(string sendgridApiKey, string fromAddress, string fromName)
        {
            this.sendgridApiKey = sendgridApiKey;
            this.fromAddress = fromAddress;
            this.fromName = fromName;
        }
        public async Task SendEmail(string toAddress) 
        {
            var client = new SendGridClient(this.sendgridApiKey);
            var from = new EmailAddress(this.fromAddress, fromName);
            var subject = $"{DateTime.Now.Date.ToString("yyyy/MM/dd")} Stock Updates";
            var to = new EmailAddress(toAddress);
            var plainTextContent = "Here is a button";
            var htmlContent = "<button type='button'>Click Me!</button>";
            var msg = MailHelper.CreateSingleEmail(from, to, subject, plainTextContent, htmlContent);
            var response = await client.SendEmailAsync(msg);
        }
    }
}
