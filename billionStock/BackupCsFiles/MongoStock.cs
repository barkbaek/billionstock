using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

/*
 List 업데이트할 때 참고!
 List<string> Images = someList;
 var update = Update.Set("Images", new BsonArray(Images));
 collection.Update(query, update, UpdateFlags.Upsert);     
*/

namespace billionStock
{
    class MongoStock
    {
        [BsonId]
        public ObjectId Id { get; set; }
        [BsonElement("code")]
        public String Code { get; set; }
        [BsonElement("name")]
        public String Name { get; set; }
        [BsonElement("market")]
        public List<String> Market { get; set; }

        public MongoStock(string code, string name, List<String> market)
        {
            Code = code;
            Name = name;
            Market = market;
        }
    }
}
