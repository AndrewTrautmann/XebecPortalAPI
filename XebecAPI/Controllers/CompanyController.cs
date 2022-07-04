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
    public class CompanyController:ControllerBase
    {      
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper mapper;

        public CompanyController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            this.mapper = mapper;
        }

        // GET: api/<DepartmentController>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetCompany()
        {
            try
            {
                var Company = await _unitOfWork.Companies.GetAll();

                return Ok(Company);

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
        public async Task<IActionResult> GetSingleCompanyById(int id)
        {
            try
            {
                var Company = await _unitOfWork.Companies.GetT(q => q.Id == id);
                return Ok(Company);
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
        public async Task<IActionResult> CreateCompany([FromBody] Company Company)
        {

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {

                await _unitOfWork.Companies.Insert(Company);
                await _unitOfWork.Save();
                return CreatedAtAction("GetSingleCompanyById", new { id = Company.Id }, Company);
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    e.InnerException);
            }
        }

        // PUT api/<DocumentsController>/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCompany(int id, [FromBody] CompanyDTO Company)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var existingCompany = await _unitOfWork.Companies.GetT(q => q.Id == id);

                if (existingCompany == null)
                {
                    return BadRequest("Submitted data is invalid");
                }
                mapper.Map(Company, existingCompany);
                _unitOfWork.Companies.Update(existingCompany);
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
        public async Task<IActionResult> DeleteCompany(int id)
        {
            if (id < 1)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var Company = await _unitOfWork.Companies.GetT(q => q.Id == id);

                if (Company == null)
                {
                    return BadRequest("Submitted data is invalid");
                }

                await _unitOfWork.Companies.Delete(id);
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
