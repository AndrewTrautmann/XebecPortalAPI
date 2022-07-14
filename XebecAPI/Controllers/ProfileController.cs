using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using XebecAPI.IRepositories.CustomIRepositories;

namespace XebecAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]


    public class ProfileController : ControllerBase
    {
        private readonly IUsersCustomRepo usersCustomRepo;

        public ProfileController( IUsersCustomRepo usersCustomRepo)
        {
          
            this.usersCustomRepo = usersCustomRepo;
        }

        [HttpGet ("{AppUserId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetApplications(int AppUserId)
        {
            try
            {
                var Profile = await usersCustomRepo.GetProfile(AppUserId); 
                return Ok(Profile);
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }
        }
    }
}
