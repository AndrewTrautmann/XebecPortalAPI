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


// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace XebecAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [AllowAnonymous]
    public class JobPlatformHelperController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper mapper;

        public JobPlatformHelperController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            this.mapper = mapper;
        }

        // GET: api/<JobPlatformHelpersController>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetJobPlatformHelpers()
        {
            try
            {
                var JobPlatformHelpers = await _unitOfWork.JobPlatformHelpers.GetAll();
             
                return Ok(JobPlatformHelpers);

            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }
        }

        // GET api/<JobPlatformHelpersController>/5
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetJobPlatformHelper(int id)
        {
            try
            {
                var JobPlatformHelper = await _unitOfWork.JobPlatformHelpers.GetT(q => q.Id == id);
                return Ok(JobPlatformHelper);
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }
        }

        [HttpGet("job/{jobId}")]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetJobPlatformHelperbyJob(int jobId)
        {
            try
            {

                var JobPlatformHelpers = await _unitOfWork.JobPlatformHelpers.GetAll(q => q.JobId == jobId);

                return Ok(JobPlatformHelpers);

            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }
        }

        // POST api/<JobPlatformHelpersController>
        [HttpPost]
        [AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> CreateJobPlatformHelper([FromBody] JobPlatformHelper JobPlatformHelper)
        {

            if (!ModelState.IsValid)
            {

                return BadRequest(ModelState);
            }


            try
            {

                await _unitOfWork.JobPlatformHelpers.Insert(JobPlatformHelper);
                await _unitOfWork.Save();

                return CreatedAtAction("GetJobPlatformHelper", new { id = JobPlatformHelper.Id }, JobPlatformHelper);

            }
            catch (Exception e)
            {

                return StatusCode(StatusCodes.Status500InternalServerError,
                    e.InnerException);
            }


        }

        // POST api/<UsersController>
        [HttpPost("list")]
        [AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> CreateJobPlatformHelper([FromBody] List<JobPlatformHelper> platforms)
        {

            if (!ModelState.IsValid)
            {

                return BadRequest(ModelState);
            }

            try
            {
                await _unitOfWork.JobPlatformHelpers.InsertRange(platforms);

                await _unitOfWork.Save();

                return Ok(platforms);
            }
            catch (Exception e)
            {

                return StatusCode(StatusCodes.Status500InternalServerError,
                    e.InnerException);
            }


        }



        // PUT api/<JobPlatformHelpersController>/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateJobPlatformHelper(int id, [FromBody] JobPlatformHelperDTO JobPlatformHelper)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var originalJobPlatformHelper = await _unitOfWork.JobPlatformHelpers.GetT(q => q.Id == id);

                if (originalJobPlatformHelper == null)
                {
                    return BadRequest("Submitted data is invalid");
                }
                mapper.Map(JobPlatformHelper, originalJobPlatformHelper);
                _unitOfWork.JobPlatformHelpers.Update(originalJobPlatformHelper);
                await _unitOfWork.Save();

                return NoContent();

            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }

        }


        // DELETE api/<JobPlatformHelpersController>/5
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DeleteJobPlatformHelper(int id)
        {
            if (id < 1)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var JobPlatformHelper = await _unitOfWork.JobPlatformHelpers.GetT(q => q.Id == id);

                if (JobPlatformHelper == null)
                {
                    return BadRequest("Submitted data is invalid");
                }

                await _unitOfWork.JobPlatformHelpers.Delete(id);
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
