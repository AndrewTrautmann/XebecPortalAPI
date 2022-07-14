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
    public class JobController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IJobsCustomRepo jobsCustomRepo;
        private readonly IMapper mapper;

        public JobController(IUnitOfWork unitOfWork, IJobsCustomRepo jobsCustomRepo, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            this.jobsCustomRepo = jobsCustomRepo;
            this.mapper = mapper;
        }

        // GET: api/<JobsController>
        [HttpGet]
        [AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetJobs()
        {
            try
            {
                var Jobs = await jobsCustomRepo.GetAllJobsFullDetails();
                Jobs = Jobs.GroupBy(p => p.Id).Select(x => x.First()).ToList();
                return Ok(Jobs);

            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }
        }

        // GET api/<JobsController>/5
        [HttpGet("{id}")]
        [AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetJob(int id)
        {
            try
            {
 
                var Job = await jobsCustomRepo.GetJobTDetails(id);
                return Ok(Job);
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }
        }

        // POST api/<JobsController>
        [HttpPost]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> CreateJob([FromBody] Job Job)
        {

            if (!ModelState.IsValid)
            {

                return BadRequest(ModelState);
            }


            try
            {
                Job.CreationDate = DateTime.Now;
                Job.DueDate = DateTime.Now;

                await _unitOfWork.Jobs.Insert(Job);
                await _unitOfWork.Save();

                return CreatedAtAction("GetJob", new { id = Job.Id }, Job);

            }
            catch (Exception e)
            {

                return StatusCode(StatusCodes.Status500InternalServerError,
                    e.InnerException);
            }

        }


        // PUT api/<JobsController>/5
        [HttpPut("{id}")]
        [Authorize]
        public async Task<IActionResult> UpdateJob(int id, [FromBody] JobDTO Job)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var originalJob = await _unitOfWork.Jobs.GetT(q => q.Id == id);

                if (originalJob == null)
                {
                    return BadRequest("Submitted data is invalid");
                }

                mapper.Map(Job, originalJob);
                _unitOfWork.Jobs.Update(originalJob);
                await _unitOfWork.Save();

                return NoContent();

            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }

        }


        // DELETE api/<JobsController>/5
        [HttpDelete("{id}")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DeleteJob(int id)
        {
            if (id < 1)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var Job = await _unitOfWork.Jobs.GetT(q => q.Id == id);

                if (Job == null)
                {
                    return BadRequest("Submitted data is invalid");
                }

                await _unitOfWork.Jobs.Delete(id);
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
