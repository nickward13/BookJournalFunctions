using Microsoft.Azure.Cosmos;

namespace Hectagon.BookJournal
{
    public class JournalEntry
    {
        public string? Id {get; set;}
        public string? Userid {get; set;}
        public string? Title {get; set;}
        public string? Author {get; set;}
        public string? Review {get; set;}
        public int? Rating {get; set;}

        public JournalEntry(string userid, string title, string author, string review, int rating)
        {
            Id = Guid.NewGuid().ToString();
            Userid = userid;
            Title = title;
            Author = author;
            Review = review;
            Rating = rating;
        }

        public JournalEntry()
        {
            Id = Guid.NewGuid().ToString();
        }
    }

    public static class CosmosDbService
    {
        private static readonly string? CosmosDbConnectionString = System.Environment.GetEnvironmentVariable("BookJournalCosmosDBConnectionString");
        private static readonly string DatabaseId = "BookJournal";
        private static readonly string ContainerId = "JournalEntries";

        private static CosmosClientOptions options = new CosmosClientOptions
        {
            SerializerOptions = new CosmosSerializationOptions
            {
                PropertyNamingPolicy = CosmosPropertyNamingPolicy.CamelCase
            }
        };

        private static readonly CosmosClient cosmosClient = new CosmosClient(CosmosDbConnectionString, options);
        private static readonly Container container = cosmosClient.GetContainer(DatabaseId, ContainerId);

        public static async Task AddJournalEntryAsync(JournalEntry entry)
        {
            await container.CreateItemAsync(entry, new PartitionKey(entry.Userid));
        }

        public static async Task<JournalEntry> GetJournalEntryAsync(string id, string userid)
        {
            JournalEntry entry = await container.ReadItemAsync<JournalEntry>(id, new PartitionKey(userid));
            return entry;
        }

        public static async Task<List<JournalEntry>> GetJournalEntriesAsync(string userid)
        {
            var query = new QueryDefinition("SELECT * FROM c WHERE c.userid = @userid")
                .WithParameter("@userid", userid);
            List<JournalEntry> entries = new List<JournalEntry>();
            using (FeedIterator<JournalEntry> resultSet = container.GetItemQueryIterator<JournalEntry>(query))
            {
                while (resultSet.HasMoreResults)
                {
                    FeedResponse<JournalEntry> response = await resultSet.ReadNextAsync();
                    entries.AddRange(response);
                }
            }
            return entries;
        }
    }
}