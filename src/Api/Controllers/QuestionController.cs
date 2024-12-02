using Api.Constants;
using Api.Validators;
using Application.Interface.Service;
using Domain.Enum;
using Domain.Model.Question;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [ApiController]
    //[CustomAuthorize(RoleEnum.Admin)]
    public class QuestionController : ControllerBase
    {
        private readonly IQuestionService _questionService;
        public QuestionController(IQuestionService questionService)
        {
            _questionService = questionService;
        }

        [HttpGet(ApiEndPointConstant.Question.QuestionsEndpointByTestId)]
        public async Task<IActionResult> GetAllQuestionsByType([FromForm] QuestionSearchModel model)
        {
            var result = await _questionService.GetAllQuestionsByTestId(model);
            return Ok(result);
        }
        [HttpGet(ApiEndPointConstant.Question.QuestionEndpoint)]
        public async Task<IActionResult> GetQuestionById(int id)
        {
            var result = await _questionService.GetQuestionById(id);
            return Ok(result);
        }

        //[HttpPost(ApiEndPointConstant.Question.QuestionsEndpoint)]
        //public async Task<IActionResult> CreateQuestion(QuestionPostModel questionModel)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest(ModelState);
        //    }
        //    try
        //    {
        //        var result = await _questionService.CreateQuestion(questionModel);
        //        return Ok(result);
        //    }
        //    catch (Exception ex)
        //    {
        //        return BadRequest(ex.Message);
        //    }
        //}

        [HttpPut(ApiEndPointConstant.Question.QuestionEndpoint)]
        public async Task<IActionResult> UpdateQuestion(QuestionPutModel questionModel, int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                var result = await _questionService.UpdateQuestion(questionModel,id);
                if (!result.IsSuccess)
                {
                    return BadRequest(result.Message);
                }
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost(ApiEndPointConstant.Question.QuestionsEndpointForPersonalTest)]
        public async Task<IActionResult> CreateQuestionForPersonalTest(QuestionPostModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                var result = await _questionService.CreateQuestionForPersonalTest(model);
                if (!result.IsSuccess) 
                {
                    return BadRequest(result.Message);
                }
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete(ApiEndPointConstant.Question.QuestionEndpoint)]
        public async Task<IActionResult> DeleteQuestion(int id)
        {
            try
            {
                var result = await _questionService.DeleteQuestion(id);
                if (!result.IsSuccess)
                {
                    return BadRequest(result.Message);
                }
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
