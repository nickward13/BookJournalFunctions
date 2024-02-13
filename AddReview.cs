using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

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
        public async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Anonymous, "post")] HttpRequest req
        )
        {
            _logger.LogInformation("C# HTTP trigger function processed a request.");
            _logger.LogInformation($"CosmosDB connection string: {System.Environment.GetEnvironmentVariable("BookJournalCosmosDBConnectionString").Substring(0,50)}");
            
            string userid = Authenticator.GetUserId(req);
            
            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            if (string.IsNullOrEmpty(requestBody))
            {
                return new BadRequestObjectResult("Please pass a journal entry in the request body");
            }

            _logger.LogInformation($"Adding journal entry for user {userid}");

            try
            {
                JournalEntry newEntry = JsonConvert.DeserializeObject<JournalEntry>(requestBody);
                newEntry.Userid = userid;
                _logger.LogInformation($"Adding journal entry with title {newEntry.Title} for user {newEntry.Userid}");
                CosmosDbService.AddJournalEntryAsync(newEntry).Wait();
                _logger.LogInformation($"Successfully added journal entry with title {newEntry.Title} for user {newEntry.Userid}");
            }
            catch (Exception e)
            {
                _logger.LogError(e, $"Error adding journal entry: {e.Message}");
                return new BadRequestObjectResult("Error adding journal entry");
            }

            return new CreatedResult();
        }
    }
}
