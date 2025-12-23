using Microsoft.AspNetCore.Identity.UI.Services;

namespace EmlakPortal2.Services
{
    // Sahte E-posta Servisi (Sistemi kandırmak için)
    public class EmailSender : IEmailSender
    {
        public Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            return Task.CompletedTask;
        }
    }
}