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

namespace XebecAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [AllowAnonymous]
    public class PolicyController:ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper mapper;

        public PolicyController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            this.mapper = mapper;
        }

        // GET: api/<DepartmentController>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetPolicy()
        {
            try
            {
                var policies = await _unitOfWork.Policies.GetAll();

                return Ok(policies);

            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }
        }

        // GET api/<DepartmentController>/5
        [HttpGet("single/{id}")]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetSinglePolicyById(int id)
        {
            try
            {
                var Policy = await _unitOfWork.Policies.GetT(q => q.Id == id);
                return Ok(Policy);
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }
        }

        // POST api/<DepartmentController>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> CreatePolicy([FromBody] Policy Policy)
        {

            if (!ModelState.IsValid)
            {

                return BadRequest(ModelState);
            }


            try
            {
                await _unitOfWork.Policies.Insert(Policy);
                await _unitOfWork.Save();
                return CreatedAtAction("GetSinglePolicyById", new { id = Policy.Id }, Policy);

            }
            catch (Exception e)
            {

                return StatusCode(StatusCodes.Status500InternalServerError, e.InnerException);
            }
        }

        // PUT api/<DocumentsController>/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdatePolicy(int id, [FromBody] PolicyDTO Policy)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var originalPolicy = await _unitOfWork.Policies.GetT(q => q.Id == id);

                if (originalPolicy == null)
                {
                    return BadRequest("Submitted data is invalid");
                }
                mapper.Map(Policy, originalPolicy);
                _unitOfWork.Policies.Update(originalPolicy);
                await _unitOfWork.Save();

                return NoContent();
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }
        }

        // DELETE api/<DocumentsController>/5
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DeletePolicy(int id)
        {
            if (id < 1)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var Policy = await _unitOfWork.Policies.GetT(q => q.Id == id);

                if (Policy == null)
                {
                    return BadRequest("Submitted data is invalid");
                }

                await _unitOfWork.Policies.Delete(id);
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
