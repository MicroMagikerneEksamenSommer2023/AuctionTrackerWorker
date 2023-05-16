using Newtonsoft.Json;
using MongoDB.Driver;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace AuctionTrackerWorker.Models;

    public class Bid
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id {get;set;}
        [Newtonsoft.Json.JsonProperty("catalogId")]
        public string CatalogId {get; set;}
        [Newtonsoft.Json.JsonProperty("buyerEmail")]
        public string BuyerEmail {get;set;}
        [Newtonsoft.Json.JsonProperty("BidValue")]
        public double BidValue {get;set;}
        
        [JsonConstructor]
        public Bid(string catalogid, string buyeremail, double bidvalue)
        {
            
            this.CatalogId = catalogid;
            this.BuyerEmail = buyeremail;
            this.BidValue = bidvalue;
        }


        

    }