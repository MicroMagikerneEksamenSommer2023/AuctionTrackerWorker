using Newtonsoft.Json;
using MongoDB.Driver;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace AuctionTrackerWorker.Models;

    public class AuctionDTO
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        [Newtonsoft.Json.JsonProperty("id")]
        public string? Id { get; set; }

        [Newtonsoft.Json.JsonProperty("sellerId")]
        public string SellerId { get; set; }

        [Newtonsoft.Json.JsonProperty("startTime")]
        public DateTime StartTime { get; set; }

        [Newtonsoft.Json.JsonProperty("endTime")]
        public DateTime EndTime { get; set; }

        [Newtonsoft.Json.JsonProperty("startingBid")]
        public double StartingBid { get; set; }

        [Newtonsoft.Json.JsonProperty("buyoutPrice")]
        public double BuyoutPrice { get; set; }

        [Newtonsoft.Json.JsonProperty("currentBid")]
        public double CurrentBid { get; set; }

    
    }

