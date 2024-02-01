using Microsoft.AspNetCore.Authentication.Cookies;
using MongoDB.Driver;
using ProjectOKRs.Models;

namespace ProjectOKRs.Data
{
    public class DBCycles
    {
        private static string nameDB = "OKRsApp";
        private static string collectionDB = "cycles";

        public static async Task CreatedCycles(Cycles cycles)
        {

            var connect = MongoDB.ConnectMongoDB(nameDB);
            var collection = connect.GetCollection<Cycles>(collectionDB);
            await collection.InsertOneAsync(cycles);
        }

        public static async Task<List<Cycles>> GetAllCycles()
        {

            var connect = MongoDB.ConnectMongoDB(nameDB);
            var collection = connect.GetCollection<Cycles>(collectionDB);
            var result = await collection.FindAsync(x => true).Result.ToListAsync();
             return result = result.OrderByDescending(x => x.start_date).ToList();
        }

        public static async Task DeleteCycles(string id)
        {
            var connect = MongoDB.ConnectMongoDB(nameDB);
            var collection = connect.GetCollection<Cycles>(collectionDB);
            await collection.DeleteOneAsync(x => x.id == id);
        }

        public static async Task UpdateCycles(Cycles cycles)
        {
            var connect = MongoDB.ConnectMongoDB(nameDB);
            var collection = connect.GetCollection<Cycles>(collectionDB);
            var option = new ReplaceOptions { IsUpsert = false };
            var result = await collection
            .ReplaceOneAsync(x => x.id == cycles.id, cycles, option);
        }

       public static async Task<Cycles> Get(string Cycleid)
        {

            var connect = MongoDB.ConnectMongoDB(nameDB);
            var collection = connect.GetCollection<Cycles>(collectionDB);
            var cycle = await collection.FindAsync(x => x.id == Cycleid).Result.FirstOrDefaultAsync();
            return cycle;
        }
    }
}