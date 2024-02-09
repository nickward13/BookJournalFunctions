using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace Hectagon.BookJournal
{
    public class AddReview
    {
        private readonly ILogger<AddReview> _logger;

        public AddReview(ILogger<AddReview> logger)
        {
            _logger = logger;
        }

        [Function("AddReview")]
        public IActionResult Run([HttpTrigger(AuthorizationLevel.Anonymous, "get", "post")] HttpRequest req)
        {
            _logger.LogInformation("C# HTTP trigger function processed a request.");
            return new OkObjectResult("Welcome to Azure Functions!");
        }
    }
}
