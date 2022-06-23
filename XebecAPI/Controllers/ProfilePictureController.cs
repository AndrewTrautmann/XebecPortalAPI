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
    [Authorize]
    [ApiController]
    public class ProfilePictureController:ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper mapper;

        public ProfilePictureController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            this.mapper = mapper;
        }

        // GET: api/<ProfilePictureController>
        [HttpGet]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetProfilePicture()
        {
            try
            {
                var ProfilePicture = await _unitOfWork.ProfilePictures.GetAll();

                return Ok(ProfilePicture);

            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }
        }

        // GET api/<ProfilePictureController>/5
        [HttpGet("single/{id}")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetProfilePictureById(int id)
        {
            try
            {
                var ProfilePicture = await _unitOfWork.ProfilePictures.GetT(q => q.Id == id);
                return Ok(ProfilePicture);
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }
        }

        //get by appuserId
        // GET api/<ProfilePictureController>/5
        [HttpGet("appUser/{id}")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetFirstProfilePictureByUserID(int id)
        {
            try
            {
                var ProfilePicture = await _unitOfWork.ProfilePictures.GetT(q => q.AppUserId == id);
                return Ok(ProfilePicture);
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }
        }

        // POST api/<ProfilePictureController>
        [HttpPost]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> CreateProfilePicture([FromBody] ProfilePicture profilePicture)

        {

            if (!ModelState.IsValid)
            {

                return BadRequest(ModelState);
            }


            try
            {

                await _unitOfWork.ProfilePictures.Insert(profilePicture);
                await _unitOfWork.Save();

                return CreatedAtAction("GetProfilePictureById", new { id = profilePicture.Id }, profilePicture);

            }
            catch (Exception e)
            {

                return StatusCode(StatusCodes.Status500InternalServerError,
                    e.InnerException);
            }
        }

        // PUT api/<ProfilePictureController>/5
        [HttpPut("{id}")]
        [Authorize]
        public async Task<IActionResult> UpdateProfilePicture(int id, [FromBody] ProfilePictureDTO profilePicture)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var originalProfilePic = await _unitOfWork.ProfilePictures.GetT(q => q.Id == id);

                if (originalProfilePic == null)
                {
                    return BadRequest("Submitted data is invalid");
                }
                mapper.Map(profilePicture, originalProfilePic);
                _unitOfWork.ProfilePictures.Update(originalProfilePic);
                await _unitOfWork.Save();

                return NoContent();

            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }

        }

        // DELETE api/<ProfilePictureController>/5
        [HttpDelete("{id}")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DeleteProfilePicture(int id)
        {
            if (id < 1)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var profilePicture = await _unitOfWork.ProfilePictures.GetT(q => q.Id == id);

                if (profilePicture == null)
                {
                    return BadRequest("Submitted data is invalid");
                }

                await _unitOfWork.ProfilePictures.Delete(id);
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
