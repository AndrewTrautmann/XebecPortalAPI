using AutoMapper;
using XebecAPI.Shared.Security;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using XebecAPI.IRepositories;
using Microsoft.AspNetCore.Authentication;
using System.Net.Http;
using Newtonsoft.Json;
using System.Net.Http.Json;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Authentication.Google;

namespace XebecAPI.Controllers.Security
{
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IUserDb userDb;
        private readonly IUnitOfWork unitOfWork;
        private readonly IEmailRepo emailrepo;

        private readonly IConfiguration config;

        public AuthController(IUserDb userDb, IUnitOfWork unitOfWork, IEmailRepo emailrepo, IConfiguration _config)
        {
            this.userDb = userDb;
            this.unitOfWork = unitOfWork;
            this.emailrepo = emailrepo;
            config = _config;
        }

        private string CreateJWT(AppUser user)
        {
            var secretkey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["JWT:Key"]));
            // NOTE: SAME KEY AS USED IN Startup.cs FILE

            var credentials = new SigningCredentials(secretkey, SecurityAlgorithms.HmacSha256);

            var claims = new[] // NOTE: could also use List<Claim> here
			{
                new Claim(ClaimTypes.Name, user.Name), // NOTE: this will be the "User.Identity.Name" value
				new Claim(ClaimTypes.SerialNumber, user.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.Sub, user.Email),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim(ClaimTypes.Role, user.Role),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()) 
				// NOTE: this could a unique ID assigned to the user by a database
			};

            var token = new JwtSecurityToken(issuer: config["JWT:Issuer"], audience: config["JWT:Audience"], claims: claims, expires: DateTime.Now.AddMinutes(60), signingCredentials: credentials);
            return new JwtSecurityTokenHandler().WriteToken(token);
        }




        [HttpPost]
        [Route("api/auth/register")]
        public async Task<LoginResult> Post([FromBody] RegisterModel reg)
        {

            AppUser newuser = await userDb.AddUser(reg.Email, reg.Password, reg.Role);

            if (newuser != null)

                return new LoginResult
                {
                    Message = "Registration successful.",
                    JwtBearer = CreateJWT(newuser),
                    Email = reg.Email,
                    Role = reg.Role,
                    Success = true,
                    AppUserId = newuser.Id
                };

            return new LoginResult { Message = "User already exists.", Success = false };

        }
        //
        [HttpPost]
        [Route("api/auth/login")]
        public async Task<LoginResult> Post([FromBody] LoginModel log)
        {
            AppUser user = await userDb.AuthenticateUser(log.Email, log.Password);

            if (user != null)
                return new LoginResult
                {
                    AppUserId = user.Id,//<-newly added
                    Message = "Login successful.",
                    JwtBearer = CreateJWT(user),
                    Email = log.Email,
                    Role = user.Role,
                    Success = true
                };

            return new LoginResult { Message = "User/password not found.", Success = false };

        }

        //The follwoing endpoints below are being used

        [HttpPost]
        [Route("api/auth/registernew")]
        public async Task<LoginResult> Register([FromBody] RegisterModel reg)
        {
            try
            {
                var userId = await userDb.CheckExistingUser(reg.Email);
                if (userId > 0)
                {
                    return new LoginResult { Message = "User already exists.", Success = false };
                }
                else if (userId == 0)
                {
                    //Add user to db if it doesn't return null
                    AppUser newuser = await userDb.AddUserModified(reg.Email, reg.Password, reg.Role, reg.Name, reg.Surname);
                    if (newuser.Id == 0)
                    {
                        return new LoginResult { Message = "Email or password is empty", Success = false };
                    }
                    if (newuser != null && newuser.Id != 0)
                    {
                        //bool emailKey = false;
                        //bool adminSend = false;
                        if (newuser.Role != "Candidate")
                        {
                            //emailKey = await RegisterKey(newuser, reg.Url);

                            await emailrepo.PowerAutomateAsync(newuser, reg.Link);
                        }
                        //if (!emailKey )
                        //{
                        //	return new LoginResult { Message = "failed to send email to user", Success = false };
                        //}
                        //else

                        //if (!adminSend)
                        //{
                        //	return new LoginResult { Message = "failed to send email to admin", Success = false };
                        //}
                        return new LoginResult
                        {
                            Message = "Registration successful",
                            JwtBearer = CreateJWT(newuser),// fix
                            Email = newuser.Email,
                            Role = newuser.Role,
                            Name = newuser.Name,
                            Surname = newuser.Surname,
                            Success = true,
                            AppUserId = newuser.Id
                        };
                    }
                }

                return new LoginResult { Message = "Failed to register user .", Success = false };

            }
            catch
            {
                return new LoginResult { Message = "Something went wrong with your request. Please reload and try again.", Success = false };
            }

        }

        [HttpPost]
        [Route("api/auth/loginnew")]
        public async Task<LoginResult> Login([FromBody] LoginModel log)
        {
            AppUser user = await userDb.AuthenticateUserModified(log.Email, log.Password);

            if (user.Id <= 1 || user == null)
            {
                if (user.Id == 0)
                {
                    return new LoginResult { Message = "Email/Password is Empty", Success = false };
                }

                if (user.Id == -1)
                {
                    return new LoginResult { Message = "User/password not found.", Success = false };
                }

                if (user.Id == -2)
                {
                    return new LoginResult { Message = "User not registered.", Success = false };
                }
                if (user.Id == -3)
                {
                    return new LoginResult { Message = "Password does not match.", Success = false };
                }
                return new LoginResult { Message = "User/password not found.", Success = false };
            }

            if (user != null)
                return new LoginResult
                {
                    AppUserId = user.Id,//<-newly added
                    Message = "Login successful.",
                    JwtBearer = CreateJWT(user),
                    Email = user.Email,
                    Role = user.Role,
                    Name = user.Name,
                    Surname = user.Surname,
                    Avatar = user.ImageUrl,
                    Success = true,
                };

            return new LoginResult { Message = "User/password not found.", Success = false };
        }


        [HttpPost]
        [Route("api/auth/changepassword")]
        public async Task<string> ChangePassword([FromBody] LoginModel log)
        {
            AppUser user = await userDb.UpdateUserModified(log.Email, log.Password);

            if (user != null)
                return "true";

            return "password could not be changed.";

        }

        [HttpPost]
        [Route("api/auth/AdminConfirm")]
        public async Task<ActionResult> ConfirmationByAdmin([FromQuery] string email)
        {
            try
            {
                var userId = await userDb.CheckExistingUser(email);
                if (userId == 0)
                {
                    return NotFound();
                }

                var user = await unitOfWork.AppUsers.GetT(q => q.Email.Equals(email));
                user.Registered = true;
                unitOfWork.AppUsers.Update(user);
                await unitOfWork.Save();
                return Accepted();
            }
            catch
            {

                return StatusCode(500);
            }
        }

        [HttpPost]
        [Route("api/auth/KeyChange")]
        public async Task<string> KeyChange([FromBody] AppUser userguy)
        {
            try
            {
                AppUser newuser = await unitOfWork.AppUsers.GetT(q => q.Email.Equals(userguy.Email));

                if (newuser != null)
                {
                    if (newuser.UserKey == userguy.UserKey)
                    {
                        newuser.LinkVisits = 1;
                        unitOfWork.AppUsers.Update(newuser);
                        await unitOfWork.Save();
                        return "true";
                    }
                    return "user key does not match";
                }
                return "user not found";

            }
            catch (Exception e)
            {

                return e.Message;
            }
        }

        [HttpPost("key")]
        public async Task<bool> RegisterKey([FromBody] AppUser user, string Url)
        {
            try
            {
                var fd = await emailrepo.ConfrimRegisterKey(user, Url);
                if (fd)
                {
                    unitOfWork.AppUsers.Update(user);
                    await unitOfWork.Save();
                    return true;
                }

                return false;

            }
            catch (Exception)
            {

                return false;
            }

        }

        [HttpPost("keyForgot")]
        public async Task<string> ForgotPasswordKey([FromBody] RegisterModel registerModel)
        {

            try
            {
                var userId = await userDb.CheckExistingUser(registerModel.Email);
                if (userId == 0)
                {
                    return "user does not exist";
                }
                var user = await unitOfWork.AppUsers.GetT(q => q.Email.Equals(registerModel.Email));
                string key = Guid.NewGuid().ToString().Substring(0, 6); //create new key
                user.UserKey = key;

                unitOfWork.AppUsers.Update(user);
                await unitOfWork.Save();
                //look ma no hands
                await emailrepo.PowerAutomateForgotAsync(user, registerModel.Link);
                return "true";

            }
            catch (Exception e)
            {

                return e.Message;
            }
        }

        //[HttpPost]
        //[Route("api/auth/forgotguy")]
        //public async Task<ActionResult> ForgotPasswordGUy([FromQuery] string email)
        //{
        //	try
        //	{
        //		var userId = await userDb.CheckExistingUser(email);
        //		if (userId == 0)
        //		{
        //			return NotFound();
        //		}


        //		unitOfWork.AppUsers.Update(user);
        //		await unitOfWork.Save();
        //		return Accepted();
        //	}
        //	catch (Exception e)
        //	{

        //		return StatusCode(500);
        //	}
        //}

        [HttpPost("keyConfirm")]
        public async Task<string> ConfirmrKey([FromBody] AppUser user)
        {

            try
            {
                AppUser newuser = await unitOfWork.AppUsers.GetT(q => q.Email.Equals(user.Email));

                if (newuser != null)
                {
                    if (newuser.UserKey == user.UserKey)
                    {
                        newuser.LinkVisits = 1;
                        unitOfWork.AppUsers.Update(newuser);
                        await unitOfWork.Save();
                        return "true";
                    }
                    return $"user key does not match because db is {newuser.UserKey} and UI is {user.UserKey}";
                }
                return "user not found";

            }
            catch (Exception e)
            {

                return e.Message;
            }

        }

        [HttpGet]
        [Route("newgoogle-login")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult googleLogin(string returnURL)
        {
            try
            {
                return Challenge(new AuthenticationProperties
                {// Once the login in is successful it will redirect to the callback method
                    RedirectUri = Url.Action(nameof(googleAccountLoginCallback), new { returnURL })
                }, GoogleDefaults.AuthenticationScheme);
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error at confirming " + e.Message);
            }
        }

        [HttpGet("newgoogle-login-callback")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> googleAccountLoginCallback(string returnURL)
        {

            try
            {
                var authenticationResult = await HttpContext.AuthenticateAsync(GoogleDefaults.AuthenticationScheme); // This will mainly be used to check if the authentication is successful or not	

                if (authenticationResult.Succeeded) // if it succeed then it needs to check if the user exists otherwise it has to add it
                {
                    string email = HttpContext.User.Claims
                        .Where(x => x.Type == ClaimTypes.Email)
                        .Select(p => p.Value)
                        .FirstOrDefault();

                    string firstname = HttpContext.User.Claims
                        .Where(x => x.Type == ClaimTypes.GivenName)
                        .Select(p => p.Value)
                        .FirstOrDefault();

                    string lastName = HttpContext.User.Claims
                        .Where(x => x.Type == ClaimTypes.Surname)
                        .Select(p => p.Value)
                        .FirstOrDefault();
                    return Redirect($"{returnURL}/main");
                }
                return Redirect($"{returnURL}");
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Throwback method " + e.Message);
                // Test
            }

            // just add the info then to the method that adds users to it
        }
    }
}
