using Microsoft.AspNetCore.Authentication.Cookies;
using MongoDB.Driver;
using ProjectOKRs.Models;
using MongoDB.Driver.Linq;

namespace ProjectOKRs.Data
{
    public class DBConfigSuggest
    {
        private static string nameDB = "OKRsApp";
        private static string collectionDB = "config_suggest";

        public static async Task CreatedConfigSuggest(ConfigSuggest configSuggest)
        {

            var connect = MongoDB.ConnectMongoDB(nameDB);
            var collection = connect.GetCollection<ConfigSuggest>(collectionDB);
            await collection.InsertOneAsync(configSuggest);
        }

        public static async Task<List<ConfigSuggest>> GetAllConfigSuggest()
        {

            var connect = MongoDB.ConnectMongoDB(nameDB);
            var collection = connect.GetCollection<ConfigSuggest>(collectionDB);
            return await collection.FindAsync(x => true).Result.ToListAsync();
        }

        public static async Task DeleteConfigSuggest(string id)
        {
            var connect = MongoDB.ConnectMongoDB(nameDB);
            var collection = connect.GetCollection<ConfigSuggest>(collectionDB);
            await collection.DeleteOneAsync(x => x.id == id);
        }

        public static async Task UpdateConfigSuggest(ConfigSuggest configSuggest)
        {
            var connect = MongoDB.ConnectMongoDB(nameDB);
            var collection = connect.GetCollection<ConfigSuggest>(collectionDB);
            var option = new ReplaceOptions { IsUpsert = false };
            var result = await collection
            .ReplaceOneAsync(x => x.id == configSuggest.id, configSuggest, option);
        }
    }
}