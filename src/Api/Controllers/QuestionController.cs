using Api.Constants;
using Application.Interface.Service;
using Domain.Model.Question;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [ApiController]
    public class QuestionController : ControllerBase
    {
        private readonly IQuestionService _questionService;
        public QuestionController(IQuestionService questionService)
        {
            _questionService = questionService;
        }
        [HttpGet(ApiEndPointConstant.Question.QuestionsEndpoint)]
        public async Task<IActionResult> GetAllQuestionsByType(Guid id)
        {
            var result = await _questionService.GetAllQuestionsByType(id);
            return Ok(result);
        }
        [HttpGet(ApiEndPointConstant.Question.QuestionEndpoint)]
        public async Task<IActionResult> GetQuestionById(int id)
        {
            var result = await _questionService.GetQuestionById(id);
            return Ok(result);
        }
        [HttpPost(ApiEndPointConstant.Question.QuestionsEndpoint)]
        public async Task<IActionResult> CreateQuestion([FromForm] QuestionPostModel questionModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                var result = await _questionService.CreateQuestion(questionModel);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut(ApiEndPointConstant.Question.QuestionEndpoint)]
        public async Task<IActionResult> UpdateQuestion([FromForm] QuestionPutModel questionModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                var result = await _questionService.UpdateQuestion(questionModel);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
