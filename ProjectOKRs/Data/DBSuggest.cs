using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Driver;
using ProjectOKRs.Models;

namespace ProjectOKRs.Data;

public class DBSuggest
{
    private static string nameDB = "OKRsApp";
    private static string collectionDB = "suggest";

    public static async Task<List<Suggest>> GetAll(string cycle)
    {
        var connect = MongoDB.ConnectMongoDB(nameDB);
        var collection = connect.GetCollection<Suggest>(collectionDB);
        var results = await collection.FindAsync(x => x.cycle == cycle && !x.draft).Result.ToListAsync();

        return results.OrderByDescending(x => x.date).ToList();
    }


    public static async Task<List<Suggest>> GetList(string cycle, string user)
    {
        var connect = MongoDB.ConnectMongoDB(nameDB);
        var collection = connect.GetCollection<Suggest>(collectionDB);

        var builder = Builders<Suggest>.Filter;

        var filtered = builder.Eq("cycle", cycle)
           & builder.Eq("draft", false);

        if (!string.IsNullOrEmpty(user))
            filtered = filtered & builder.Eq("user", user);

        var result = await collection.FindAsync(filtered).Result.ToListAsync();

        return (from x in result orderby x.date descending select x).ToList();
    }

    public static async Task<bool> Delete(string id)
    {
        var connect = MongoDB.ConnectMongoDB(nameDB);
        var collection = connect.GetCollection<Suggest>(collectionDB);

        var result = await collection.DeleteOneAsync(x => x.id == id);

        if (result.DeletedCount > 0)
            return true;
        else
            return false;
    }
    public static async Task<Suggest> Create(Suggest model)
    {
        if (string.IsNullOrEmpty(model.id))
            model.id = MongoDB.RandomId();
        model.date = DateTime.Now.Ticks;
        var connect = MongoDB.ConnectMongoDB(nameDB);
        var collection = connect.GetCollection<Suggest>(collectionDB);

        await collection.InsertOneAsync(model);

        return model;
    }

    public static async Task<List<Suggest>> GetDrafts(string cycle, string user)
    {
        var connect = MongoDB.ConnectMongoDB(nameDB);
        var collection = connect.GetCollection<Suggest>(collectionDB);

        var builder = Builders<Suggest>.Filter;

        var filtered = builder.Eq("cycle", cycle)
           & builder.Eq("draft", true)
           & builder.Eq("user", user);

        var sorted = Builders<Suggest>.Sort.Ascending("date");

        var result = await collection.FindAsync(filtered).Result.ToListAsync();

        return (from x in result orderby x.date select x).ToList();
    }
    public static async Task<Suggest> Update(Suggest model)
    {
        var connect = MongoDB.ConnectMongoDB(nameDB);
        var collection = connect.GetCollection<Suggest>(collectionDB);


        var option = new ReplaceOptions { IsUpsert = false };

        var result = await collection.ReplaceOneAsync(x => x.id.Equals(model.id), model, option);

        return model;
    }
}
