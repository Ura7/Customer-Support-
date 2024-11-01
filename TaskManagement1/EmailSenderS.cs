using Microsoft.AspNetCore.Identity.UI.Services;

namespace TaskManagement1
{
	public class EmailSenderS : IEmailSender
	{
		public Task SendEmailAsync(string email, string subject, string htmlMessage)
		{
			return Task.CompletedTask;
		}
	}
}
