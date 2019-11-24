using System;
using System.IO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Logging;

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
        public string CatchAll()
        {
            var request = HttpContext.Request;

            _logger.LogInformation($@"{{
    ""method"": ""{request.Method}"",
    ""protocol"": ""{request.Protocol}"",
    ""url"": ""{request.Host}{request.Path}{request.QueryString}"",
    ""content-length"": ""{request.ContentLength}"",
    ""content-type"": ""{request.ContentType}"",
    ""body"": ""{GetBody()}"",
    ""headers"": {{{GetHeaders()}}},
}}
");
            return "k";
        }

        private string GetHeaders()
        {
            var headers = string.Empty;
            foreach (var key in Request.Headers.Keys)
            {
                headers += "\t\t\"" + key + "\":\"" + Request.Headers[key] + "\"," + Environment.NewLine;
            }

            return headers;
        }

        private string GetBody()
        {
            string body;
            using (var reader = new StreamReader(Request.Body))
            {
                body = reader.ReadToEnd();
            }

            return body;
        }
    }
}