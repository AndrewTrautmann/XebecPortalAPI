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
    [ApiController]
    [Authorize]
    public class ApplicationPhaseHelperController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IApplicationPhaseHelperRepository applicationPhaseHelperRepository;
        private readonly IMapper mapper;

        public ApplicationPhaseHelperController(IUnitOfWork unitOfWork, IApplicationPhaseHelperRepository applicationPhaseHelperRepository, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            this.applicationPhaseHelperRepository = applicationPhaseHelperRepository;
            this.mapper = mapper;
        }

        // GET: api/<ApplicationPhaseHelpersController>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetApplicationPhaseHelpers()
        {
            try
            {
                var ApplicationPhaseHelpers = await _unitOfWork.ApplicationPhaseHelpers.GetAll();
             
                return Ok(ApplicationPhaseHelpers);

            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }
        }

        // GET api/<ApplicationPhaseHelpersController>/5
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetApplicationPhaseHelper(int id)
        {
            try
            {
                var ApplicationPhaseHelper = await _unitOfWork.ApplicationPhaseHelpers.GetT(q => q.Id == id);
                return Ok(ApplicationPhaseHelper);
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }
        }

        // POST api/<ApplicationPhaseHelpersController>
        [HttpPost]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> CreateApplicationPhaseHelper([FromBody] ApplicationPhaseHelper ApplicationPhaseHelper)
        {

            if (!ModelState.IsValid)
            {

                return BadRequest(ModelState);
            }


            try
            {

                await _unitOfWork.ApplicationPhaseHelpers.Insert(ApplicationPhaseHelper);
                await _unitOfWork.Save();

                return CreatedAtAction("GetApplicationPhaseHelper", new { id = ApplicationPhaseHelper.Id }, ApplicationPhaseHelper);

            }
            catch (Exception e)
            {

                return StatusCode(StatusCodes.Status500InternalServerError,
                    e.InnerException);
            }


        }


        // PUT api/<ApplicationPhaseHelpersController>/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateApplicationPhaseHelper(int id, [FromBody] ApplicationPhaseHelperDTO ApplicationPhaseHelper)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var originalApplicationPhaseHelper = await _unitOfWork.ApplicationPhaseHelpers.GetT(q => q.Id == id);

                if (originalApplicationPhaseHelper == null)
                {
                    return BadRequest("Submitted data is invalid");
                }
                mapper.Map(ApplicationPhaseHelper, originalApplicationPhaseHelper);
                _unitOfWork.ApplicationPhaseHelpers.Update(originalApplicationPhaseHelper);
                await _unitOfWork.Save();

                return NoContent();

            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }

        }


        // DELETE api/<ApplicationPhaseHelpersController>/5
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DeleteApplicationPhaseHelper(int id)
        {
            if (id < 1)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var ApplicationPhaseHelper = await _unitOfWork.ApplicationPhaseHelpers.GetT(q => q.Id == id);

                if (ApplicationPhaseHelper == null)
                {
                    return BadRequest("Submitted data is invalid");
                }

                await _unitOfWork.ApplicationPhaseHelpers.Delete(id);
                await _unitOfWork.Save();

                return NoContent();


            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }

        }
        // GET: api/<ApplicationPhaseHelpersController>
        [HttpGet("UserId={AppUserId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetApplicationPhaseInfo(int AppUserId)
        {
            try
            {
                var ApplicationPhaseHelpers = await applicationPhaseHelperRepository.GetApplicationPhaseInfo(AppUserId);

                return Ok(ApplicationPhaseHelpers);

            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }
        }

        // GET: api/<ApplicationPhaseHelpersController>
        [HttpGet("appPhase")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetApplicationPhaseDetails([FromQuery]int AppUserId, [FromQuery]int jobId)
        {
            try
            {
                var ApplicationPhaseHelpers = await applicationPhaseHelperRepository.GetApplicationPhaseInfoDetailed(AppUserId, jobId);

                return Ok(ApplicationPhaseHelpers);

            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }
        }

        // GET: api/<ApplicationPhaseHelpersController>
        [HttpGet("ApplicantPortal")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetApplicantPortalDetails([FromQuery] int JobId)
        {
            try
            {
                var ApplicationPhaseHelpers = await applicationPhaseHelperRepository.GetApplicationPhaseInfoForUser(0,JobId);
                ApplicationPhaseHelpers = ApplicationPhaseHelpers.GroupBy(p => p.Application).Select(x => x.Last()).ToList();
                return Ok(ApplicationPhaseHelpers);

            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }
        }

        // GET: api/<ApplicationPhaseHelpersController>
        [HttpGet("myJobs")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetApplicationPhaseDetailsUser([FromQuery] int AppUserId, [FromQuery] int JobId)
        {
            try
            {
                var ApplicationPhaseHelpers = await applicationPhaseHelperRepository.GetApplicationPhaseInfoForUser(AppUserId, JobId);

                return Ok(ApplicationPhaseHelpers);

            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }
        }
    }
}
