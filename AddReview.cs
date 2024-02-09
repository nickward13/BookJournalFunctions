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
        public IActionResult Run([HttpTrigger(AuthorizationLevel.Anonymous, "get", "post")] HttpRequest req
        )
        {
            _logger.LogInformation("C# HTTP trigger function processed a request.");

            var newEntry = new JournalEntry(userid: "1", title: "The Great Gatsby", author: "F. Scott Fitzgerald", review: "A great book!", rating: 5);

            try{
                CosmosDbService.AddJournalEntryAsync(newEntry).Wait();
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error adding journal entry");
                return new BadRequestObjectResult("Error adding journal entry");
            }

            return new CreatedResult();
        }
    }
}
