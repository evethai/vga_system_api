using Api.Constants;
using Application.Interface.Service;
using Domain.Model.News;
using Domain.Model.Response;
using Infrastructure.Persistence.Service;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [Route("api/news")]
    [ApiController]
    public class NewsController : ControllerBase
    {
        private readonly INewsService _newsService;

        public NewsController(INewsService newsService)
        {
            _newsService = newsService;
        }
        [HttpGet(ApiEndPointConstant.News.NewsEndpoint)]
        public async Task<IActionResult> GetListNewsAsync([FromQuery] NewsSearchModel searchModel)
        {
            var news = await _newsService.GetListNewsAsync(searchModel);
            return Ok(news);
        }
        [HttpGet(ApiEndPointConstant.News.NewEndpoint)]
        public async Task<IActionResult> GetNewsByIdAsync(Guid id)
        {
            var news = await _newsService.GetNewsByIdAsync(id);
            return Ok(news);
        }
        [HttpPost(ApiEndPointConstant.News.NewsEndpoint)]
        public async Task<IActionResult> CreateNewsAsync(NewsPostModel postModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                var result = await _newsService.CreateNewsAsync(postModel);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPut(ApiEndPointConstant.News.NewEndpoint)]
        public async Task<IActionResult> UpdateNewsAsync(NewsPutModel putModel, Guid id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                var result = await _newsService.UpdateNewsAsync(putModel, id);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpDelete(ApiEndPointConstant.News.NewEndpoint)]
        public async Task<IActionResult> DeleteNewsAsync(Guid id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                var result = await _newsService.DeleteNewsAsync(id);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPost(ApiEndPointConstant.ImageNews.ImageNewsEndpoint)]
        public async Task<IActionResult> CreateImageNewsAsync(Guid NewsId, List<ImageNewsPostModel> imageNews)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                var result = await _newsService.CreateImageNewsAsync(NewsId, imageNews);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpDelete(ApiEndPointConstant.ImageNews.ImageNewsDeleteEndpoint)]
        public async Task<IActionResult> DeleteNewsImageAsync(int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                var result = await _newsService.DeleteImageNewsAsync(id);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPut(ApiEndPointConstant.ImageNews.ImageNewsUpdateEndpoint)]
        public async Task<IActionResult> UpdateNewsImageAsync(ImageNewsPutModel imageNewsModel, int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                var result = await _newsService.UpdateImageNewsAsync(imageNewsModel, id);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        
    }
}
