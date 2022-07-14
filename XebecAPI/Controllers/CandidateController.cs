using AutoMapper;
using XebecAPI.Data;
using XebecAPI.IRepositories;
using XebecAPI.Shared;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using XebecAPI.IRepositories.CustomIRepositories;


// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace XebecAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class CandidateController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IUsersCustomRepo usersCustomRepo;
        private readonly IMapper mapper;

        public CandidateController(IUnitOfWork unitOfWork, IUsersCustomRepo _usersCustomRepo, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            usersCustomRepo = _usersCustomRepo;
            this.mapper = mapper;
        }

        // GET: api/<CandidateConroller>
        // Get all Role == Candidate
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetCandidates()
        {
            try
            {
                var Candidate = await _unitOfWork.AppUsers.GetAll(q => q.Role == "Candidate");

                return Ok(Candidate);

            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }
        }

        // GET api/<Candidate>/5
        // Get Single Candidate
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetCandidate(int id)
        {
            try
            {
                var Candidate = await _unitOfWork.AppUsers.GetT(q => q.Id == id && q.Role == "Candidate");
                return Ok(Candidate);
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }
        }        

        // GET api/<CandidateController>search?name="Dave"
        [HttpGet("search")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> SearchCandidate([FromQuery] string name )
        {
            try
            {
                
                var candidateInfo = await usersCustomRepo.SearchUser("Candidate", name);

                return Ok(candidateInfo);

            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }
        }

       

       

       


       

       
    }
}
