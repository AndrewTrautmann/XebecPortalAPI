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
    public class JobTypeController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper mapper;

        public JobTypeController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            this.mapper = mapper;
        }

        // GET: api/<JobTypesController>
        [HttpGet]
        [AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetJobType()
        {
            try
            {
                var JobTypes = await _unitOfWork.JobTypes.GetAll();
             
                return Ok(JobTypes);

            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }
        }

        // GET api/<JobTypesController>/5
        [HttpGet("{id}")]
        [AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetJobType(int id)
        {
            try
            {
                var JobType = await _unitOfWork.JobTypes.GetT(q => q.Id == id);
                return Ok(JobType);
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }
        }

        // POST api/<JobTypesController>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> CreateJobType([FromBody] JobType JobType)
        {

            if (!ModelState.IsValid)
            {

                return BadRequest(ModelState);
            }


            try
            {

                await _unitOfWork.JobTypes.Insert(JobType);
                await _unitOfWork.Save();

                return CreatedAtAction("GetJobType", new { id = JobType.Id }, JobType);

            }
            catch (Exception e)
            {

                return StatusCode(StatusCodes.Status500InternalServerError,
                    e.InnerException);
            }


        }


        // PUT api/<JobTypesController>/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateJobType(int id, [FromBody] JobTypeDTO JobType)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var originalJobType = await _unitOfWork.JobTypes.GetT(q => q.Id == id);

                if (originalJobType == null)
                {
                    return BadRequest("Submitted data is invalid");
                }
                mapper.Map(JobType, originalJobType);
                _unitOfWork.JobTypes.Update(originalJobType);
                await _unitOfWork.Save();

                return NoContent();

            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }

        }


        // DELETE api/<JobTypesController>/5
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DeleteJobType(int id)
        {
            if (id < 1)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var JobType = await _unitOfWork.JobTypes.GetT(q => q.Id == id);

                if (JobType == null)
                {
                    return BadRequest("Submitted data is invalid");
                }

                await _unitOfWork.JobTypes.Delete(id);
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
