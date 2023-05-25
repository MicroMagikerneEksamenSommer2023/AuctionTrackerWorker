using MongoDB.Driver;
using AuctionTrackerWorker.Models;

namespace AuctionTrackerWorker.Services;

public class MongoService : IMongoService
{
        private readonly ILogger<MongoService> _logger;
        private readonly IMongoCollection<BidLogs> _logs;
        private readonly IConfiguration _config;

        public MongoService(ILogger<MongoService> logger, IConfiguration config )
        {
            _logger = logger;
            _config = config;
            var mongoClient = new MongoClient(_config["connectionsstring"]);
            var database = mongoClient.GetDatabase(_config["database"]);
            _logs = database.GetCollection<BidLogs>(_config["collection"]);
    
        }

        public async Task<BidLogs> LogBid(Bid data)
        {
               try
            {
                BidLogs NewBid = new BidLogs(data.CatalogId,data.BuyerEmail,data.BidValue,DateTime.UtcNow);
                await _logs.InsertOneAsync(NewBid);
                return NewBid;
            }
            catch(Exception ex)
            {
                throw new Exception("Failed to insert document: " + ex.Message);
            }
        }
        
         public async Task<List<BidLogs>> GetAllLogs()
        {
            var filter = Builders<BidLogs>.Filter.Empty;
            var dbData = (await _logs.FindAsync(filter)).ToList();
            if (dbData.Count == 0)
            {
                throw new ItemsNotFoundException("No bids were found in the LOG database.");
            }
            return dbData;
        }

        public async Task<List<BidLogs>> GetLogsById(string id)
        {
            var filter = Builders<BidLogs>.Filter.Eq(b => b.CatalogId, id);
            var dbData = (await _logs.FindAsync(filter)).ToList();
              if (dbData == null)
            {
                throw new ItemsNotFoundException($"No bid logs with catalogID {id} was found in the database.");
            }
            return dbData;
        }

        public async Task<List<BidLogs>> GetLogsByEmail(string email)
        {
            var filter = Builders<BidLogs>.Filter.Eq(b => b.BuyerEmail, email);
            var dbData = (await _logs.FindAsync(filter)).ToList();
              if (dbData == null)
            {
                throw new ItemsNotFoundException($"No bid logs with buyer-email: {email} was found in the database.");
            }
            return dbData;
        }
        


}