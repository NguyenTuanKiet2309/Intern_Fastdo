using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Driver;

namespace ProjectOKRs.Data;

public class MongoDB
{
    public static IMongoDatabase ConnectMongoDB(string name){
        var mongoClient = new MongoClient();
        return mongoClient.GetDatabase(name);
    }

    private static readonly string Characters = "0123456789abcdefghijklmnopqrstuvwxyz";
        public static string RandomId()
        {
            DateTime date = DateTime.Now;

            var result = new StringBuilder();
            result.Append(DateTime.Now.ToString("yy"));
            result.Append(Characters[date.Month]);
            result.Append(Characters[date.Day]);
            result.Append(Guid.NewGuid().ToString().Replace("-", "").Substring(0, 6));
            return result.ToString().ToUpper();
        }
}
