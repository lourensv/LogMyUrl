using System.IO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace LogMyUrl.Controllers
{
    [ApiController]
    public class Controller : ControllerBase
    {
        private readonly ILogger<Controller> _logger;

        public Controller(ILogger<Controller> logger)
        {
            _logger = logger;
        }

        [Route("{*.}")]
        public IActionResult CatchAll()
        {
            var request = HttpContext.Request;

            var details = new
            {
                method = request.Method,
                protocol = request.Protocol,
                url = $"{request.Host}{request.Path}{request.QueryString}",
                contentLength = request.ContentLength,
                request.ContentType,
                body = GetBody(),
                headers = Request.Headers
            };

            _logger.LogInformation(JsonConvert.SerializeObject(details));

            return Ok();
        }

        private string GetBody()
        {
            using (var reader = new StreamReader(Request.Body))
            {
                return reader.ReadToEnd();
            }
        }
    }
}