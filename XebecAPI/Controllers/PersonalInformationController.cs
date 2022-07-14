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
using XebecAPI.DTOs;
using Microsoft.AspNetCore.Authorization;
using XebecAPI.IRepositories.CustomIRepositories;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace XebecAPI.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    [ApiController]
    public class PersonalInformationController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IUsersCustomRepo usersCustomRepo;
        private readonly IMapper mapper;

        public PersonalInformationController(IUnitOfWork unitOfWork, IUsersCustomRepo _usersCustomRepo, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            usersCustomRepo = _usersCustomRepo;
            this.mapper = mapper;
        } 

        // GET: api/<PersonalInformationController>
        [HttpGet]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetPersonalInformations()
        {
            try
            {
                var PersonalInformation = await _unitOfWork.PersonalInformation.GetAll();
             
                return Ok(PersonalInformation);

            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }
        }

        // GET api/<PersonalInformationController>/5
        [HttpGet("single/{id}")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetPersonalInformation(int id)
        {
            try
            {
                var PersonalInformation = await _unitOfWork.PersonalInformation.GetT(q => q.Id == id);
                return Ok(PersonalInformation);
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }
        }

        //get by appuserId
        // GET api/<PersonalInformationController>/5
        [HttpGet("{id}")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetFirstPersonalInformationByUserID(int id)
        {
            try
            {
                var PersonalInformation = await _unitOfWork.PersonalInformation.GetT(q => q.AppUserId == id);
                return Ok(PersonalInformation);
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }
        }

        //get by appuserId
        // GET api/<PersonalInformationController>/5
        [HttpGet("all/{id}")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetPersonalInformationsByUserID(int id)
        {
            try
            {
                var PersonalInformation = await _unitOfWork.PersonalInformation.GetAll(q => q.AppUserId == id);
                return Ok(PersonalInformation);
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }
        }

        // GET api/<PersonalInformationController>/email=test@test.com
        [HttpGet("email={email}")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetPersonalInfoByEmail(string email)
        {
            try
            {
                var PersonalInformation = await _unitOfWork.PersonalInformation.GetT(q => q.Email == email);
                return Ok(PersonalInformation);
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }
        }

        //// GET api/<PersonalInformationController>candidates/15
        //[HttpGet("candidates/{jobId}")]
        //[ProducesResponseType(StatusCodes.Status200OK)]
        //[ProducesResponseType(StatusCodes.Status500InternalServerError)]
        //public async Task<IActionResult> SearchApplicants(string jobId, [FromQuery] string SearchQuery, [FromQuery] string ethnicityFiler, [FromQuery] string GenderFilter, [FromQuery] string disabilityFilter)
        //{
        //    try
        //    {
        //        int jobIdInt = int.Parse(jobId);
        //        var applicantsInfo = await usersCustomRepo.SearchApplicants(jobIdInt, SearchQuery, ethnicityFiler, GenderFilter, disabilityFilter);

        //        return Ok(applicantsInfo);

        //    }
        //    catch (Exception e)
        //    {
        //        return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
        //    }
        //}

        //// GET api/<PersonalInformationController>/additional?disability=disabled
        //[HttpGet("additional")]
        //[ProducesResponseType(StatusCodes.Status200OK)]
        //[ProducesResponseType(StatusCodes.Status500InternalServerError)]
        //public async Task<IActionResult> GetPersonalInfoByAdditionalInfo([FromQuery] string disability, [FromQuery] string gender, [FromQuery] string ehtnicity)
        //{
        //    try
        //    {
        //        var userInfo = await usersCustomRepo.GetPersonalByAdditional(disability,gender,ehtnicity);

        //        return Ok(userInfo);

        //    }
        //    catch (Exception e)
        //    {
        //        return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
        //    }
        //}

        //// GET api/<UserController>/role=candidate
        //[HttpGet("Test/{id}")]
        //[ProducesResponseType(StatusCodes.Status500InternalServerError)]
        //[ProducesResponseType(StatusCodes.Status200OK)]
        //public async Task<IActionResult> GetUsersByRole(int id)
        //{
        //    try
        //    {
        //        var users = await usersCustomRepo.GetCandidateDetails(id);
        //        return Ok(users);
        //    }
        //    catch (Exception e)
        //    {
        //        return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
        //    }
        //}

        // POST api/<PersonalInformationController>
        [HttpPost]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> CreatePersonalInformation([FromBody] PersonalInformation PersonalInformation)
        {

            if (!ModelState.IsValid)
            {

                return BadRequest(ModelState);
            }


            try
            {

                await _unitOfWork.PersonalInformation.Insert(PersonalInformation);
                await _unitOfWork.Save();

                return CreatedAtAction("GetPersonalInformation", new { id = PersonalInformation.Id }, PersonalInformation);

            }
            catch (Exception e)
            {

                return StatusCode(StatusCodes.Status500InternalServerError,
                    e.InnerException);
            }


        }


        // PUT api/<PersonalInformationController>/5
        [HttpPut("{id}")]
        [Authorize]
        public async Task<IActionResult> UpdatePersonalInformation(int id, [FromBody] PersonalInformationDTO PersonalInformation)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var originalPersonalInformation = await _unitOfWork.PersonalInformation.GetT(q => q.Id == id);

                if (originalPersonalInformation == null)
                {
                    return BadRequest("Submitted data is invalid");
                }
                mapper.Map(PersonalInformation, originalPersonalInformation);
                _unitOfWork.PersonalInformation.Update(originalPersonalInformation);
                await _unitOfWork.Save();

                return NoContent();

            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }

        }


        // DELETE api/<PersonalInformationController>/5
        [HttpDelete("{id}")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DeletePersonalInformation(int id)
        {
            if (id < 1)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var PersonalInformation = await _unitOfWork.PersonalInformation.GetT(q => q.Id == id);

                if (PersonalInformation == null)
                {
                    return BadRequest("Submitted data is invalid");
                }

                await _unitOfWork.PersonalInformation.Delete(id);
                await _unitOfWork.Save();

                return NoContent();
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }
        }
    }
}
