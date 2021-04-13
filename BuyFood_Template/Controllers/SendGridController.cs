﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SendGrid;
using SendGrid.Helpers.Mail;
using Microsoft.Extensions.Configuration;

namespace BuyFood_Template.Controllers
{
    [Route("api/[controller]")]

    public class SendGridController : Controller
    {

        private readonly IConfiguration _configuration;

        public SendGridController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [Route("SendNotification")]
        public async Task PostMessage()
        {
            var apiKey = _configuration.GetSection("buyfood_API").Value;
            var client = new SendGridClient(apiKey);
            var from = new EmailAddress("sunfengmsit129@gmail.com", "擺腹buyfood");
            List<EmailAddress> tos = new List<EmailAddress>
          {
              new EmailAddress("always0537@gmail.com", "hihi"),
          };

            var subject = "Hello world email from Sendgrid ";
            var htmlContent = "<strong>Hello world with HTML content</strong>";
            var displayRecipients = false; // set this to true if you want recipients to see each others mail id 
            var msg = MailHelper.CreateSingleEmailToMultipleRecipients(from, tos, subject, "", htmlContent, false);
            var response = await client.SendEmailAsync(msg);


        }
    }

}
