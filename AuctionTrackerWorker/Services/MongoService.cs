using MongoDB.Driver;
using AuctionTrackerWorker.Models;

namespace AuctionTrackerWorker.Services;

public class MongoService
{
        private readonly ILogger<MongoService> _logger;
        private readonly IMongoCollection<Bid> _bids;
        private readonly IMongoCollection<BidLogs> _logs;
        private readonly IConfiguration _config;

        public MongoService(ILogger<MongoService> logger, IConfiguration config )
        {
            _logger = logger;
            _config = config;
            var mongoClient = new MongoClient(_config["connectionsstring"]);
            var database = mongoClient.GetDatabase(_config["database"]);
            _bids = database.GetCollection<Bid>(_config["collection"]);
            _logs = database.GetCollection<BidLogs>(_config["collection2"]);
    
        }

        public async Task<List<Bid>> GetAllBids()
        {
            var filter = Builders<Bid>.Filter.Empty;
            var dbData = (await _bids.FindAsync(filter)).ToList();
            if (dbData.Count == 0)
            {
                throw new ItemsNotFoundException("No bids were found in the database.");
            }
            return dbData;
        }

        public async Task<Bid> GetById(string id)
        {
            var filter = Builders<Bid>.Filter.Eq(b => b.Id, id);
            var dbData = (await _bids.FindAsync(filter)).FirstOrDefault();
              if (dbData == null)
            {
                throw new ItemsNotFoundException($"No bid with ID {id} was found in the database.");
            }
            return dbData;
        }

        public async Task<Bid> UpdateBid(Bid newData)
        {
            var filter = Builders<Bid>.Filter.Eq(b=> b.Id, newData.Id);
            var update = Builders<Bid>.Update.Set(b=>b.BuyerEmail,newData.BuyerEmail).Set(b=>b.BidValue,newData.BidValue);
            var dbData = await _bids.FindOneAndUpdateAsync(filter,update, new FindOneAndUpdateOptions<Bid>{ReturnDocument = ReturnDocument.After} );
            if(dbData == null)
            {throw new ItemsNotFoundException($"No bid with the {newData.Id} was found in the database");}
            await LogBid(dbData);
            return dbData;
        }

        public async Task<Bid> CreateNewBid(Bid data)
        {
              try
            {
                Bid NewBid = data;
                await _bids.InsertOneAsync(NewBid);
                await LogBid(NewBid);
                return NewBid;
            }
            catch(Exception ex)
            {
                throw new Exception("Failed to insert document: " + ex.Message);
            }
        }

        public async Task<BidLogs> LogBid(Bid data)
        {
               try
            {
                BidLogs NewBid = new BidLogs(data.Id,data.CatalogId,data.BuyerEmail,data.BidValue,DateTime.UtcNow);
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
        
//magler logs

}