using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Store.Service.CasheService;
using System.Text;

namespace Store.API.Helper
{
    public class CasheAttribute : Attribute, IAsyncActionFilter
    {
        private readonly int _timeToLiveInSeconds;

        public CasheAttribute(int timeToLiveInSeconds) 
        {
            _timeToLiveInSeconds = timeToLiveInSeconds;
        }
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var _casheService=context.HttpContext.RequestServices.GetRequiredService<ICasheService>();
            var CasheKey=GenerateCasheKeyFormRequest(context.HttpContext.Request);
            var cashedResponse = await _casheService.GetCasheResponeAsync(CasheKey);
            if (!string.IsNullOrEmpty(cashedResponse))
            {
                var contentResult = new ContentResult()
                {
                    Content = cashedResponse,
                    ContentType = "application/json",
                    StatusCode=200
                };
                context.Result= contentResult;
                return;
            }
            var excutedContext = await next();
            if (excutedContext.Result is OkObjectResult response) 
            {
                await _casheService.SetCasheResponseAsync(CasheKey, response.Value, TimeSpan.FromSeconds(_timeToLiveInSeconds));
            }
        }
        private string GenerateCasheKeyFormRequest(HttpRequest request)
        {
            StringBuilder casheKey=new StringBuilder();
            casheKey.Append($"{request.Path}");
            foreach(var (key,value) in request.Query.OrderBy(x => x.Key))
            {
                casheKey.Append($"|{key}|{value}");
            }
            return casheKey.ToString();
        }
    }
}
