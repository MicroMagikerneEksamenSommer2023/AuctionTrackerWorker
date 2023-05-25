using System.Collections.Generic;
using System.Threading.Tasks;
using AuctionTrackerWorker.Models;
public interface IMongoService
{

    Task<BidLogs> LogBid(Bid data);
    Task<List<BidLogs>> GetAllLogs();
    Task<List<BidLogs>> GetLogsById(string id);
    Task<List<BidLogs>> GetLogsByEmail(string email);
}