using Microsoft.Extensions.Configuration;
using PrintMatic.Core.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using MimeKit;
using Microsoft.AspNetCore.Identity;
using PrintMatic.Core.Entities.Identity;

namespace PrintMatic.Services
{
	public class EmailService : IEmailService
	{
	
		private readonly IConfiguration _configuration;
		private readonly UserManager<AppUser> _userManager;

		public EmailService(IConfiguration configuration,UserManager<AppUser> userManager)
		{
			_configuration = configuration;
			_userManager = userManager;
		}

		public async Task SendEmailAsync(string email, string subject, string message)
		{
			var parts = email.Split('@');
			var domain=parts.Length > 1 ? parts[1] : string.Empty;
			//if (domain == "hotmail.com")
			//{
			//	var smtpClient = new SmtpClient
			//	{
			//		Host = _configuration["SmtpOffice:Host"],        
			//		Port = int.Parse(_configuration["SmtpOffice:Port"]), 
			//		EnableSsl = true,                          
			//		Credentials = new NetworkCredential(
			//   _configuration["SmtpOffice:Username"],
			//   _configuration["SmtpOffice:Password"]        
			//  )
			//	};

			//	var mailMessage = new MailMessage
			//	{
			//		From = new MailAddress(_configuration["SmtpOffice:From"]),
			//		Subject = subject,
			//		Body = message,
			//		IsBodyHtml = true
			//	};

			//	mailMessage.To.Add(email);

			//	await smtpClient.SendMailAsync(mailMessage);

			//}
			var user = await _userManager.FindByEmailAsync(email);
			var smtpClient = new SmtpClient(_configuration["Smtp:Host"])
			{
				Port = int.Parse(_configuration["Smtp:Port"]),
				Credentials = new NetworkCredential(_configuration["Smtp:Username"], _configuration["Smtp:Password"]),
				EnableSsl = true,
			};

			var mailMessage = new MailMessage
			{
				From = new MailAddress(_configuration["Smtp:From"]),
				Subject = subject,
				Body =$"<div class=\"jumbotron\">\r\n  <h1 class=\"display-4\">Hello,{user.UserName}!</h1>\r\n <p>We received a request to reset your password for your Giftly account. To proceed with changing your password, please use the verification code below:</p>\r\n <h1 class=\"lead\">Your Verification Code: {message}</h1>" +
				$"\r\n</div><p>This code is valid for the next 10 minutes. If you did not request a password reset, please ignore this email or contact our support team immediately.</p> <hr/> \r\n" +
				$"<p>Thank you for using Giftly!\r\n\r\n</p> \r\n <p>Best regards,</p>\r\n**Giftly Support Team**\r\n\r\n**Contact Us**: support@giftly | Visit our [Help Center](https://giftly.comp>" ,
				IsBodyHtml = true,
			};
			mailMessage.To.Add(email);

			await smtpClient.SendMailAsync(mailMessage);

			}
		
			
		

		
	}
}
