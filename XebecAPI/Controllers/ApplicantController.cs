using AutoMapper;
using XebecAPI.Data;
using XebecAPI.Shared;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using XebecAPI.DTOs.ViewModels;
using Microsoft.AspNetCore.Authorization;
using XebecAPI.IRepositories.CustomIRepositories;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace XebecAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ApplicantController : ControllerBase
    {
        private readonly IApplicationPhaseHelperRepository _unitOfWork;

        public ApplicantController(IApplicationPhaseHelperRepository unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        // GET: api/<ApplicationsController>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status412PreconditionFailed)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetAllApplicants()
        {
            try
            {
                var Applications = await _unitOfWork.GetallApplicants();
                if (Applications.Count < 1)
                {
                    return StatusCode(StatusCodes.Status412PreconditionFailed);
                }

                return Ok(Applications);
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }
        }

        // GET: api/<ApplicationsController>
        [HttpGet("{jobId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status412PreconditionFailed)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetApplications(int jobId)
        {
            try
            {
                var Applications = await _unitOfWork.GetApplicants(jobId);
                if (Applications.Count < 1)
                {
                    return StatusCode(StatusCodes.Status412PreconditionFailed);
                }
                Applications = Applications.GroupBy(x => x.Id).Select(g => g.Last()).ToList();

                return Ok(Applications);
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }
        }
    }
}