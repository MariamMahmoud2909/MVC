﻿using System.Threading.Tasks;

namespace MVC_3PL.Services.EmailSender
{
	public interface IEmailSender
	{
		Task SendAsync(string from, string recipients, string subject, string body);
	}
}