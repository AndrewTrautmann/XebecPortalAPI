using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using XebecAPI.DTOs;
using XebecAPI.IRepositories;
using XebecAPI.Shared;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace XebecAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [AllowAnonymous]
    public class CollaboratorsAssignedController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper mapper;

        public CollaboratorsAssignedController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            this.mapper = mapper;
        }
        // GET: api/<CollaboratorsAssignedController>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetCollaboratorsAssigned()
        {
            try
            {
                var collaboratorsAssigned = await _unitOfWork.CollaboratorsAssigned.GetAll();

                return Ok(collaboratorsAssigned);

            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }
        }

        // GET api/<CollaboratorsAssignedController>/5
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetCollaboratorsAssigned(int id)
        {
            try
            {
                var collaboratorsAssigned = await _unitOfWork.CollaboratorsAssigned.GetT(q => q.Id == id);
                return Ok(collaboratorsAssigned);
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }
        }

        //Get api<CollaboratorsAssignedController/{jobId}>
        [HttpGet("collaborators/{jobId}")]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetCollaboratorsAssignedByJob(int jobId)
        {
            try
            {
                var collaboratorsAssignedByJob = await _unitOfWork.CollaboratorsAssigned.GetAll(q => q.JobId == jobId);

                return Ok(collaboratorsAssignedByJob);

            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }
        }

        // POST api/<CollaboratorsAssignedController>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> CreateCollaboratorsAssigned([FromBody] CollaboratorAssigned collaboratorsAssigned)
        {

            if (!ModelState.IsValid)
            {

                return BadRequest(ModelState);
            }


            try
            {

                await _unitOfWork.CollaboratorsAssigned.Insert(collaboratorsAssigned);
                await _unitOfWork.Save();

                return CreatedAtAction("GetCollaboratorsAssigned", new { id = collaboratorsAssigned.Id }, collaboratorsAssigned);

            }
            catch (Exception e)
            {

                return StatusCode(StatusCodes.Status500InternalServerError,
                    e.InnerException);
            }


        }

        // PUT api/<CollaboratorsAssignedController>/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCollaboratorsAssigned(int id, [FromBody] CollaboratorsAssignedDTO collaboratorsAssigned)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var originalAdditionalInformation = await _unitOfWork.CollaboratorsAssigned.GetT(q => q.Id == id);

                if (originalAdditionalInformation == null)
                {
                    return BadRequest("Submitted data is invalid");
                }
                mapper.Map(collaboratorsAssigned, originalAdditionalInformation);
                _unitOfWork.CollaboratorsAssigned.Update(originalAdditionalInformation);
                await _unitOfWork.Save();

                return NoContent();

            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }

        }



        // DELETE api/<CollaboratorsAssignedController>/5
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DeleteCollaboratorsAssigned(int id)
        {
            if (id < 1)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var AdditionalInformation = await _unitOfWork.CollaboratorsAssigned.GetT(q => q.Id == id);

                if (AdditionalInformation == null)
                {
                    return BadRequest("Submitted data is invalid");
                }

                await _unitOfWork.AdditionalInformation.Delete(id);
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
