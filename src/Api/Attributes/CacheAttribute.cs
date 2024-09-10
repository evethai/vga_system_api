using Api.Configurations;
using Microsoft.AspNetCore.Mvc;
using System.Text;
using Microsoft.AspNetCore.Mvc.Filters;
using Api.Services;

namespace Api.Attributes
{
    public class CacheAttribute : Attribute, IAsyncActionFilter
    {
        private readonly int _timeToLiveSeconds;
        public CacheAttribute(int timeToLiveSeconds = 1000)
        {
            _timeToLiveSeconds = timeToLiveSeconds;
        }
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            //check cache exsited
            var cacheConfig = context.HttpContext.RequestServices.GetRequiredService<RedisConfiguration>();
            if (!cacheConfig.Enabled)
            {
                await next();
                return;
            }
            var cacheService = context.HttpContext.RequestServices.GetRequiredService<ICacheService>();

            var cacheKey = GenerateCacheKeyFromRequest(context.HttpContext.Request);

            var cacheResponse = await cacheService.GetCacheResponseAsync<string>(cacheKey);

            if (cacheResponse != null)
            {
                var contentResult = new ContentResult
                {
                    Content = cacheResponse,
                    ContentType = "application/json",
                    StatusCode = 200
                };
                context.Result = contentResult;
                return;
            }

            var executedContext = await next();
            if (executedContext.Result is OkObjectResult objectResult)
            {
                await cacheService.SetCacheResponseAsync(cacheKey, objectResult.Value, TimeSpan.FromSeconds(_timeToLiveSeconds));
            }
        }

        private static string GenerateCacheKeyFromRequest(HttpRequest request)
        {
            var keyBuilder = new StringBuilder();
            keyBuilder.Append($"{request.Path}");
            foreach (var (key, value) in request.Query.OrderBy(x => x.Key))
            {
                keyBuilder.Append($"|{key}-{value}");
            }
            return keyBuilder.ToString();
        }
    }
}
