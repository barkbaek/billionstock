using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace billionStock
{
    class Stock
    {
        public String Code { get; set; }
        public String Name { get; set; }
        public List<String> Market { get; set; }

        public Stock(string code, string name, List<String> market)
        {
            Code = code;
            Name = name;
            Market = market;
        }

        public void AppendMarket (string market)
        {
            Market.Add(market);
        }

        public override string ToString()
        {
            return String.Format("종목코드: {0}, 종목명: {1}, 마켓: {2}", Code, Name, string.Join(", ", Market.ToArray()) );
        }
    }
}
