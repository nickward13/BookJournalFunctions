using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace Hectagon.BookJournal
{
    public class GetReviews
    {
        private readonly ILogger<GetReviews> _logger;

        public GetReviews(ILogger<GetReviews> logger)
        {
            _logger = logger;
        }

        [Function("GetReviews")]
        public IActionResult Run([HttpTrigger(AuthorizationLevel.Anonymous, "get")] HttpRequest req)
        {
            _logger.LogInformation("C# HTTP trigger function processed a request.");
            // get the review id from the query string
            
            // get the user id from the request
            string userid = Authenticator.GetUserId(req) ?? string.Empty;

            // get the reviews from the database
            List<JournalEntry> reviews = CosmosDbService.GetJournalEntriesAsync(userid).Result;

            // return review to the caller
            return new OkObjectResult(reviews);
        }
    }
}
