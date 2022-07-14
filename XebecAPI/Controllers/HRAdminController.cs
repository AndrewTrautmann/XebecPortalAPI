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
    public class HRAdminController : ControllerBase
    {

        private readonly IUnitOfWork _unitOfWork;
        private readonly IUsersCustomRepo usersCustomRepo;
        private readonly IMapper mapper;

        public HRAdminController(IUnitOfWork unitOfWork, IUsersCustomRepo _usersCustomRepo)
        {
            _unitOfWork = unitOfWork;
            usersCustomRepo = _usersCustomRepo;
            usersCustomRepo = _usersCustomRepo;
        }
        // GET: api/<HRAdminController>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetHRAdmin()
        {
            try
            {
                var HRAdmin = await _unitOfWork.AppUsers.GetAll(q => q.Role == "HRAdmin");

                return Ok(HRAdmin);

            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }
        }
        // GET api/<HRAdminController>/5
        // Get Single HRAdmin
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetHRAdmin(int id)
        {
            try
            {
                var HRAdmin = await _unitOfWork.AppUsers.GetT(q => q.Id == id && q.Role == "HRAdmin");
                return Ok(HRAdmin);
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }
        }

        // GET api/<HRAdminController>search?name="Dave"
        [HttpGet("search")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> SearchHRAdmin([FromQuery] string name)
        {
            try
            {

                var HRAdminInfo = await usersCustomRepo.SearchUser("HRAdmin", name);

                return Ok(HRAdminInfo);

            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }
        }
    }
}
