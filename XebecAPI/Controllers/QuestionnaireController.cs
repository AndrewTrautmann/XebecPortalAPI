using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using XebecAPI.IRepositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using XebecAPI.Shared;
using XebecAPI.DTOs;
using Microsoft.AspNetCore.Authorization;

namespace XebecAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [AllowAnonymous]
    public class QuestionnaireController : ControllerBase
    {


        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper mapper;

        public QuestionnaireController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            this.mapper = mapper;
        }

        // GET: api/<QuestionnaireController>
        [HttpGet]
        [AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetQuestionnaire()
        {
            try
            {
                var Questionnaire = await _unitOfWork.QuestionnaireHRForms.GetAll();

                return Ok(Questionnaire);

            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }
        }

        // GET api/<QuestionnaireController>/5
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetQuestionnaire(int id)
        {
            try
            {
                var Questionnaire = await _unitOfWork.QuestionnaireHRForms.GetT(q => q.Id == id);
                return Ok(Questionnaire);
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }
        }

        [HttpGet("job/{jobId}")]
        [AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetQuestionnairebyJob(int jobId)
        {
            try
            {

                var HRQuestionnaire = await _unitOfWork.QuestionnaireHRForms.GetAll(q => q.JobId == jobId);

                return Ok(HRQuestionnaire);

            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }
        }

        // POST api/<QuestionnaireController>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> CreateQuestionnaire([FromBody] QuestionnaireHRForm QuestionnaireHRForm)
        {

            if (!ModelState.IsValid)
            {

                return BadRequest(ModelState);
            }


            try
            {

                await _unitOfWork.QuestionnaireHRForms.Insert(QuestionnaireHRForm);
                await _unitOfWork.Save();

                return CreatedAtAction("GetQuestionnaire", new { id = QuestionnaireHRForm.Id }, QuestionnaireHRForm);

            }
            catch (Exception e)
            {

                return StatusCode(StatusCodes.Status500InternalServerError,
                    e.InnerException);
            }


        }


        // PUT api/<QuestionnaireController>/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateQuestionnaire(int id, [FromBody] QuestionnaireHRFormDTO questionnaire)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var originalquestionnaire = await _unitOfWork.QuestionnaireHRForms.GetT(q => q.Id == id);

                if (originalquestionnaire == null)
                {
                    return BadRequest("Submitted data is invalid");
                }
                mapper.Map(questionnaire, originalquestionnaire);
                _unitOfWork.QuestionnaireHRForms.Update(originalquestionnaire);
                await _unitOfWork.Save();

                return NoContent();

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
        public async Task<IActionResult> DeleteQuestionnaire(int id)
        {
            if (id < 1)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var question = await _unitOfWork.QuestionnaireHRForms.GetT(q => q.Id == id);

                if (question == null)
                {
                    return BadRequest("Submitted data is invalid");
                }

                await _unitOfWork.QuestionnaireHRForms.Delete(id);
                await _unitOfWork.Save();

                return NoContent();


            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }

        }

        // POST api/<QuestionnaireController>
        [HttpPost("array")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> CreateCustomQuestion(int[] ArrayId)
        {
            List<QuestionnaireHRForm> QuestionnaireHRForm = new List<QuestionnaireHRForm>();

            if (!ModelState.IsValid)
            {

                return BadRequest(ModelState);
            }


            try
            {

                foreach (var items in ArrayId)
                {
                    if (items != 0)
                    {
                        var Question = await _unitOfWork.Questions.GetT(q => q.Id == items);

                        QuestionnaireHRForm.Add(new QuestionnaireHRForm
                        {
                            Question = Question.QuestionDescription
                        });
                    }

                }

                await _unitOfWork.QuestionnaireHRForms.InsertRange(QuestionnaireHRForm);

                await _unitOfWork.Save();

                return NoContent();
            }
            catch (Exception e)
            {

                return StatusCode(StatusCodes.Status500InternalServerError,
                    e.InnerException);
            }


        }


        // POST api/<QuestionnaireController>
        [HttpPost("lst")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> CreateQuestionsForForm(List<QuestionnaireHRForm> lstquestions)
        {

            if (!ModelState.IsValid)
            {

                return BadRequest(ModelState);
            }


            try
            {
                await _unitOfWork.QuestionnaireHRForms.InsertRange(lstquestions);

                await _unitOfWork.Save();

                return AcceptedAtAction("GetQuestionnaire");
            }
            catch (Exception e)
            {

                return StatusCode(StatusCodes.Status500InternalServerError,
                    e.InnerException);
            }


        }
    }
}
