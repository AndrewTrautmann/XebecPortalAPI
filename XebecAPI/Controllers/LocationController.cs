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
    public class LocationController: ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper mapper;

        public LocationController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            this.mapper = mapper;
        }

        // GET: api/<LocationController>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetLocation()
        {
            try
            {
                var location = await _unitOfWork.Locations.GetAll();

                return Ok(location);

            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }
        }

        // GET api/<LocationController>/5
        [HttpGet("single/{id}")]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetSingleLocationById(int id)
        {
            try
            {
                var location = await _unitOfWork.Locations.GetT(q => q.Id == id);
                return Ok(location);
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
        public async Task<IActionResult> CreateLocation([FromBody] Location Location)
        {

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {

                await _unitOfWork.Locations.Insert(Location);
                await _unitOfWork.Save();
                return CreatedAtAction("GetSingleLocationById", new { id = Location.Id }, Location);
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,e.InnerException);
            }
        }

        // PUT api/<DocumentsController>/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateLocation(int id, [FromBody] LocationDTO Location)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var originalLocation = await _unitOfWork.Locations.GetT(q => q.Id == id);

                if (originalLocation == null)
                {
                    return BadRequest("Submitted data is invalid");
                }
                mapper.Map(Location, originalLocation);
                _unitOfWork.Locations.Update(originalLocation);
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
        public async Task<IActionResult> DeleteLocation(int id)
        {
            if (id < 1)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var Location = await _unitOfWork.Locations.GetT(q => q.Id == id);

                if (Location == null)
                {
                    return BadRequest("Submitted data is invalid");
                }

                await _unitOfWork.Locations.Delete(id);
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
