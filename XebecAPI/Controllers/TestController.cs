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
using XebecAPI.IRepositories.CustomIRepositories;
using XebecAPI.Shared;

namespace XebecAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class TestController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper mapper;
        private readonly IUsersCustomRepo usersCustom;
        private readonly IApplicationPhaseHelperRepository applicationPhaseHelper;

        public TestController(IUnitOfWork unitOfWork, IMapper mapper, IUsersCustomRepo usersCustom, IApplicationPhaseHelperRepository applicationPhaseHelper )
        {
            _unitOfWork = unitOfWork;
            this.mapper = mapper;
            this.usersCustom = usersCustom;
            this.applicationPhaseHelper = applicationPhaseHelper;
        }

        // GET: api/<AdditionalInformationController>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetAnswerTypes()
        {
            try
            {
                var Type = await usersCustom.GetCandidateDetails(6);

                return Ok(Type);

            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }
        }

        [HttpGet("ApplicantPortal")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetApplicants([FromQuery]int JobId)
        {
            try
            {
                var Type = await applicationPhaseHelper.GetApplicantsForJob(JobId);
                if (Type.Count > 0)
                {
                    Type = Type.GroupBy(a => a.User.AppUserId).Select(g => g.Last()).ToList();
                }
                return Ok(Type);

            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }
        }

    }
}
