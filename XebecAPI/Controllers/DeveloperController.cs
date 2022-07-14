using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using XebecAPI.IRepositories;
using XebecAPI.IRepositories.CustomIRepositories;
// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace XebecAPI.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class DeveloperController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IUsersCustomRepo usersCustomRepo;

        public DeveloperController(IUnitOfWork unitOfWork, IUsersCustomRepo _usersCustomRepo)
        {
            _unitOfWork = unitOfWork;
            usersCustomRepo = _usersCustomRepo;
        }
        // GET: api/<DeveloperController>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetDeveloper()
        {
            try
            {
                var Developer = await _unitOfWork.AppUsers.GetAll(q => q.Role == "Developer");

                return Ok(Developer);

            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }
        }
        // GET api/<DeveloperController>/5
        // Get Single developer
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetDeveloper(int id)
        {
            try
            {
                var developer = await _unitOfWork.AppUsers.GetT(q => q.Id == id && q.Role == "Developer");
                return Ok(developer);
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }
        }

        // GET api/<DeveloperController>search?name="Dave"
        [HttpGet("search")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> SearchDeveloper([FromQuery] string name)
        {
            try
            {
                var DeveloperInfo = await usersCustomRepo.SearchUser("Developer", name);
                return Ok(DeveloperInfo);
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }
        }
    }
}
