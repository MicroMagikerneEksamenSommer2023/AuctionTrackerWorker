using System.Collections.Generic;
using System.Threading.Tasks;
using AuctionTrackerWorker.Models;
public interface IMongoService
{
    Task<List<Bid>> GetAllBids();
    Task<Bid> GetById(string id);
    Task<List<Bid>> GetByEmail(string email);
    Task<Bid> UpdateBid(Bid newData);
    Task<Bid> CreateNewBid(Bid data);
    Task<BidLogs> LogBid(Bid data);
    Task<List<BidLogs>> GetAllLogs();
    Task<List<BidLogs>> GetLogsById(string id);
    Task<List<BidLogs>> GetLogsByEmail(string email);
}