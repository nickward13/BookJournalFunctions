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

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();

            try{
                JournalEntry newEntry = JsonConvert.DeserializeObject<JournalEntry>(requestBody);
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
