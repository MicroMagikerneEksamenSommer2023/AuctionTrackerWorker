using System.Collections.Generic;
using System.Threading.Tasks;
using AuctionTrackerWorker.Models;
public interface IMongoService
{
    Task<List<Bid>> GetAllBids();
    Task<Bid> GetById(string id);
    Task<Bid> UpdateBid(Bid newData);
    Task<Bid> CreateNewBid(Bid data);
    Task<BidLogs> LogBid(Bid data);
    Task<List<BidLogs>> GetAllLogs();
}