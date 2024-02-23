using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace Hectagon.BookJournal
{
    public class GetReview
    {
        private readonly ILogger<GetReview> _logger;

        public GetReview(ILogger<GetReview> logger)
        {
            _logger = logger;
        }

        [Function("GetReview")]
        public IActionResult Run([HttpTrigger(AuthorizationLevel.Anonymous, "get")] HttpRequest req)
        {
            _logger.LogInformation("C# HTTP trigger function processed a request.");

            // get the review id from the query string
            string reviewId = req.Query["id"].FirstOrDefault() ?? string.Empty;

            if(string.IsNullOrEmpty(reviewId))
            {
                return new BadRequestObjectResult("Please pass a review id in the query string");
            }

            // get the user id from the request
            string userid = Authenticator.GetUserId(req) ?? string.Empty;

            // get the review from the database
            JournalEntry review = CosmosDbService.GetJournalEntryAsync(reviewId, userid).Result;

            // return review to the caller
            return new OkObjectResult(review);
        }
    }
}
