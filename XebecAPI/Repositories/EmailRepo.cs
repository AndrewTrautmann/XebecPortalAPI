﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using XebecAPI.IRepositories;
using XebecAPI.Shared.Security;

namespace XebecAPI.Repositories
{
    public class EmailRepo : IEmailRepo
    {
        private readonly IUnitOfWork unitofWork;
        HttpClient client = new HttpClient();

        public EmailRepo(IUnitOfWork unitofWork)
        {
            this.unitofWork = unitofWork;
        }

        public async Task<bool> ConfrimRegisterKey(AppUser user, string stringUrl)
		{

			try
			{

				EmailModel model = new EmailModel()
				{
					Id = user.Id.ToString(),
					ToEmail = user.Email,
					ToName = user.Name,
					PlainText = $@" Hi there {user.Name}, 

Please note that your key is {user.UserKey}. 

You can use this link to insert the key {stringUrl}

If you have any questions, please email admin.

Regards, 
Xebec Team",
					Subject = "Registration Confirmation key"
				};

				var emailSending = await SendEmail(model);

				if (emailSending)
					return true;
				return false;
				
			}
			catch (Exception)
			{

				return false;
			}
		}

        public async Task<bool> ForgotPasswordKey(AppUser user, string stringUrl)
		{
            try
            {
				EmailModel model = new EmailModel()
				{
					Id = user.Id.ToString(),
					ToEmail = user.Email,
					ToName = user.Name,
					PlainText = $@" Hi there {user.Name}, 

Please note that your key is {user.UserKey}. 

You can use this link to insert the key {stringUrl}

If you have any questions, please email admin.

Regards, 
Xebec Team",
					Subject = "Forgot Password key"
				};

				var emailSending = await SendEmail(model);

				if (emailSending)
					return true;
				return false;
			}
            catch (Exception)
            {

				return false;
            }
        }
        public async Task PowerAutomateAsync(AppUser mod, string URL)
        {
			var admins = await unitofWork.Admins.GetAll();
            for (int i = 0; i < admins.Count ; i++)
            {
				PowerAutomate automate = new PowerAutomate()
				{
					Name = mod.Name,
					Surname = mod.Surname,
					Email = mod.Email,
					Role = mod.Role,
					UserKey = mod.UserKey,
					Link = URL,
					AuthorizerEmail = admins[i].Email
				};
				var jsonInString = JsonConvert.SerializeObject(automate);
				using (var msg = await client.PostAsync("https://prod-06.westeurope.logic.azure.com:443/workflows/d3868b8230194f7e8114e9a60c87fa68/triggers/manual/paths/invoke?api-version=2016-06-01&sp=%2Ftriggers%2Fmanual%2Frun&sv=1.0&sig=XhSdN1NSepuD65aL7jSdHIky1NHKQHrGp86y3RQadgs", new StringContent(jsonInString, Encoding.UTF8, "application/json"), System.Threading.CancellationToken.None))
				{
				}
			}
			
		}

		public async Task PowerAutomateForgotAsync(AppUser mod, string URL)
		{
			PowerAutomate automate = new PowerAutomate()
			{
				Name = mod.Name,
				Surname = mod.Surname,
				Email = mod.Email,
				Role = mod.Role,
				UserKey = mod.UserKey,
				Link = URL,
			};
			var jsonInString = JsonConvert.SerializeObject(automate);
			using (var msg = await client.PostAsync("https://prod-108.westeurope.logic.azure.com:443/workflows/de81ed8fb5fa4518b706556d29fb9510/triggers/manual/paths/invoke?api-version=2016-06-01&sp=%2Ftriggers%2Fmanual%2Frun&sv=1.0&sig=lNKaSExY5pAaQot_AFTmpbZeskMZ1MctzosKfGY5zg8", new StringContent(jsonInString, Encoding.UTF8, "application/json"), System.Threading.CancellationToken.None))
			{
				if (msg.IsSuccessStatusCode)
				{

				}
			}
		}

		public async Task<bool> SendAdminNotification(AppUser user, string stringUrl)
        {
			try
			{
				EmailModel model = new EmailModel()
				{
					Id = Guid.NewGuid().ToString(),
					ToEmail = "iviwe@nebula.co.za",
					ToName = "Iviwe Malotana",
					PlainText = $@" Hi there, 

Please note that there is a new HR registration on the site.

First Name {user.Name}
Surname {user.Surname}
Email {user.Email}

This person currently does not have access to the site. Please confirm that they are able to access the site by clicking the link below:

{stringUrl}

You can contact the person for more information if needed.

Regards, 
Xebec Team",
					Subject = "New HR admin request"
				};

				var emailSending = await SendEmail(model);

				if (emailSending)
					return true;
				return false;
			}
			catch (Exception)
			{

				return false;
			}
		}

        private async Task<bool> SendEmail(EmailModel model)
        {
			var jsonInString = JsonConvert.SerializeObject(model);
			using (var msg = await client.PostAsync("https://mailingservice2022.azurewebsites.net/api/email/sendgrid", new StringContent(jsonInString, Encoding.UTF8, "application/json"), System.Threading.CancellationToken.None))
			{
				if (msg.IsSuccessStatusCode)
				{
					return true;
				}
			}
			return false;
		}

		public class PowerAutomate: RegisterModel
        {

            public string UserKey { get; set; }

            public string AuthorizerEmail { get; set; }

		}
    }
}
