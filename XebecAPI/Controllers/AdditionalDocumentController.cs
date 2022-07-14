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
    [Authorize]
    public class AdditionalDocumentController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper mapper;

        
        public AdditionalDocumentController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            this.mapper = mapper;
        }

        // GET: api/<DocumentsController>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetAdditionalDocuments()
        {
            try
            {
                var AdditionalDocument = await _unitOfWork.AdditionalDocuments.GetAll();

                return Ok(AdditionalDocument);

            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }
        }

        // GET api/<DocumentsController>/5
        [HttpGet("single/{id}")]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetSingleAdditionalDocumentById(int id)
        {
            try
            {
                var AdditionalDocument = await _unitOfWork.AdditionalDocuments.GetT(q => q.Id == id);
                return Ok(AdditionalDocument);
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }
        }

        // GET api/<DocumentController>/userId=1
        [HttpGet("all/{userId}")]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAdditionalDocumentsByUserId(int userId)
        {
            try
            {
                var AdditionalDocument = await _unitOfWork.AdditionalDocuments.GetAll(q => q.AppUserId == userId);
                return Ok(AdditionalDocument);
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }
        }

        //get by appuserid
        // GET api/<DocumentController>/5
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetSingleAdditionalDocumentByUserID(int id)
        {
            try
            {
                var AdditionalDocument = await _unitOfWork.AdditionalDocuments.GetT(q => q.AppUserId == id);
                return Ok(AdditionalDocument);
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }
        }

        // POST api/<DocumentsController>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> CreateDocument([FromBody] AdditionalDocument additionalDocument)
        {

            if (!ModelState.IsValid)
            {

                return BadRequest(ModelState);
            }


            try
            {

                await _unitOfWork.AdditionalDocuments.Insert(additionalDocument);
                await _unitOfWork.Save();
                return CreatedAtAction("GetSingleAdditionalDocumentById", new { id = additionalDocument.Id }, additionalDocument);

            }
            catch (Exception e)
            {

                return StatusCode(StatusCodes.Status500InternalServerError,
                    e.InnerException);
            }


        }


        // PUT api/<DocumentsController>/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateDocument(int id, [FromBody] AdditionalDocumentDTO additionalDocument)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var originalDocument = await _unitOfWork.AdditionalDocuments.GetT(q => q.Id == id);

                if (originalDocument == null)
                {
                    return BadRequest("Submitted data is invalid");
                }
                mapper.Map(additionalDocument, originalDocument);
                _unitOfWork.AdditionalDocuments.Update(originalDocument);
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
        public async Task<IActionResult> DeleteDocument(int id)
        {
            if (id < 1)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var AdditionalDocument = await _unitOfWork.AdditionalDocuments.GetT(q => q.Id == id);

                if (AdditionalDocument == null)
                {
                    return BadRequest("Submitted data is invalid");
                }

                await _unitOfWork.AdditionalDocuments.Delete(id);
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
