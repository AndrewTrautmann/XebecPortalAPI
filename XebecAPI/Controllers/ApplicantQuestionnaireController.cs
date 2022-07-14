using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using XebecAPI.IRepositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using XebecAPI.Shared;
using Microsoft.AspNetCore.Authorization;
using XebecAPI.IRepositories.CustomIRepositories;

namespace XebecAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ApplicantQuestionnaireController : ControllerBase
    {

        private readonly IUsersCustomRepo usersCustomRepo;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper mapper;

        public ApplicantQuestionnaireController(IUnitOfWork unitOfWork, IMapper mapper, IUsersCustomRepo usersCustomRepo)
        {
            _unitOfWork = unitOfWork;
            this.mapper = mapper;
            this.usersCustomRepo = usersCustomRepo;
        }

        // GET: api/<UsersController>
        [HttpGet()]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetApplicantQuestionnaire()
        {
            try
            {
                var users = await _unitOfWork.QuestionnaireApplicantForms.GetAll();

                return Ok(users);

            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetApplicantQuestionnaire(int id)
        {

            try
            {

                var Question = await _unitOfWork.QuestionnaireApplicantForms.GetT(p => p.Id == id);

                return Ok(Question);
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }
        }

        //[HttpGet("job/{JobId}")]
        //[ProducesResponseType(StatusCodes.Status200OK)]
        //[ProducesResponseType(StatusCodes.Status500InternalServerError)]
        //public async Task<IActionResult> GetQuestionsForApplicantByJob(int JobId)
        //{
            
        //    //this is completely wrong

        //    try
        //    {
                
        //        var Question = await _unitOfWork.QuestionnaireHRForms.GetAll(p => p.JobId == JobId);

        //        return Ok(Question);
        //    }
        //    catch (Exception e)
        //    {
        //        return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
        //    }
        //}

        // POST api/<QuestionnaireController>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> CreateApplicantQuestion([FromBody] QuestionnaireApplicantForm customQuestion)
        {

            if (!ModelState.IsValid)
            {

                return BadRequest(ModelState);
            }


            try
            {

                await _unitOfWork.QuestionnaireApplicantForms.Insert(customQuestion);
                await _unitOfWork.Save();

                return CreatedAtAction("GetApplicantQuestionnaire", new { id = customQuestion.Id }, customQuestion);

            }
            catch (Exception e)
            {

                return StatusCode(StatusCodes.Status500InternalServerError,
                    e.InnerException);
            }


        }

        [HttpPost("list")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> CreateQuestionsForForm([FromBody] List<QuestionnaireApplicantForm> lstquestions)
        {

            if (!ModelState.IsValid)
            {

                return BadRequest(ModelState);
            }


            try
            {
                await _unitOfWork.QuestionnaireApplicantForms.InsertRange(lstquestions);

                await _unitOfWork.Save();

                return CreatedAtAction("GetApplicantQuestionnaire", lstquestions);
            }
            catch (Exception e)
            {

                return StatusCode(StatusCodes.Status500InternalServerError,
                    e.InnerException);
            }


        }


        // PUT api/<QuestionnaireController>/5 start here------------
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateQuestionnaire(int id, [FromBody] QuestionnaireApplicantForm question)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var originalquestion = await _unitOfWork.QuestionnaireApplicantForms.GetT(q => q.Id == id);

                if (originalquestion == null)
                {
                    return BadRequest("Submitted data is invalid");
                }
                mapper.Map(question, originalquestion);
                _unitOfWork.QuestionnaireApplicantForms.Update(originalquestion);
                await _unitOfWork.Save();

                 return CreatedAtAction("GetApplicantQuestionnaire", new { id = originalquestion.Id }, originalquestion);

            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }

        }


        // DELETE api/<QuestionnaireController>/5
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DeleteQuestion(int id)
        {
            if (id < 1)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var question = await _unitOfWork.QuestionnaireApplicantForms.GetT(q => q.Id == id);

                if (question == null)
                {
                    return BadRequest("Submitted data is invalid");
                }

                await _unitOfWork.QuestionnaireApplicantForms.Delete(id);
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
