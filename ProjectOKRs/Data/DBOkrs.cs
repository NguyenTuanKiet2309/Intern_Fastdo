using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Driver;
using ProjectOKRs.Models;

namespace ProjectOKRs.Data;

public class DBOkrs
{
    private static string nameDB = "OKRsApp";
    private static string collectionDB = "okrs";
    public static async Task<List<OKRs>> GetOKRsByCycleAndUser(string cycleId, string userCreated)
    {
        var connect = MongoDB.ConnectMongoDB(nameDB);
        var collection = connect.GetCollection<OKRs>(collectionDB);
        return await collection.Find(x => x.cycleId == cycleId && x.user_created == userCreated).ToListAsync();
    }
    public static async Task CreatedOKRs(OKRs okrs)
    {

        var connect = MongoDB.ConnectMongoDB(nameDB);
        var collection = connect.GetCollection<OKRs>(collectionDB);
        await collection.InsertOneAsync(okrs);
    }
    public static async Task<List<OKRs>> GetAll(string cycle)
    {
        var connect = MongoDB.ConnectMongoDB(nameDB);
        var collection = connect.GetCollection<OKRs>(collectionDB);

        var results = await collection.FindAsync(x => x.cycleId == cycle && !x.delete).Result.ToListAsync();

        return results.OrderByDescending(x => x.user_created).ToList();
    }
    public static async Task<OKRs> Get(string id)
    {
        if (string.IsNullOrEmpty(id))
            return null;

        var connect = MongoDB.ConnectMongoDB(nameDB);
        var collection = connect.GetCollection<OKRs>(collectionDB);

        return await collection.FindAsync(x => x.Id == id).Result.FirstOrDefaultAsync();
    }

    public static async Task<OKRs> Get(string id, string cycle)
    {

        var connect = MongoDB.ConnectMongoDB(nameDB);
        var collection = connect.GetCollection<OKRs>(collectionDB);

        return await collection.FindAsync(x => x.Id == id && x.cycleId == cycle).Result.FirstOrDefaultAsync();
    }

    public static async Task<OKRs> Update(OKRs model)
    {
        var connect = MongoDB.ConnectMongoDB(nameDB);
        var collection = connect.GetCollection<OKRs>(collectionDB);
        var option = new ReplaceOptions { IsUpsert = false };

        var result = await collection.ReplaceOneAsync(x => x.Id.Equals(model.Id), model, option);

        return model;
    }
    public static async Task<List<OKRs>> GetListByReview(string cycle, string userId)
    {

        var connect = MongoDB.ConnectMongoDB(nameDB);
        var collection = connect.GetCollection<OKRs>(collectionDB);
        var results = await collection.FindAsync(x => x.cycleId == cycle && !x.delete
            && (x.review_manager_id == userId || x.review_viewers.Contains(userId))).Result.ToListAsync();

        return results.OrderByDescending(x => x.user_created).ToList();
    }


    public static async Task<List<OKRs>> GetList(string cycle, string user)
    {
        var connect = MongoDB.ConnectMongoDB(nameDB);
        var collection = connect.GetCollection<OKRs>(collectionDB);

        var builder = Builders<OKRs>.Filter;

        var filtered = builder.Eq(x => x.cycleId, cycle) & builder.Eq("delete", false); ;

        if (!string.IsNullOrEmpty(user))
        {
            filtered = filtered & builder.Eq(x => x.user_created, user);
        }

        var result = await collection.Find(filtered).ToListAsync();

        return result.OrderBy(okr => okr.date_created).ToList();
    }

    public static async Task<List<OKRs>> GetListReview(string cycle, List<string> listUser)
    {
        var connect = MongoDB.ConnectMongoDB(nameDB);
        var collection = connect.GetCollection<OKRs>(collectionDB);

        var results = await collection.FindAsync(x => x.cycleId == cycle && !x.delete
            && listUser.Contains(x.user_created)).Result.ToListAsync();

        return results.OrderByDescending(x => x.user_created).ToList();
    }

    public static double GetProgress(List<OKRs.KeyResult> keyResults)
    {
        if (keyResults != null && keyResults.Count > 0)
        {
            double total = 0;
            foreach (var kr in keyResults)
            {
                total += Progress(kr.result, kr.goal);
            }

            return total / keyResults.Count;
        }
        else
            return 0;
    }

    public static double Progress(double result, double target)
    {
        if (result > 0 && target > 0)
        {
            double progress = result * 100 / target;
            return progress > 100 ? 100 : progress;
        }
        else
            return 0;
    }

    public static List<StaticModel> Confident()
    {
        var list = new List<StaticModel>();

        list.Add(new StaticModel
        {
            id = 1,
            name = "Rất tốt",
            color = "has-text-success",
        });

        list.Add(new StaticModel
        {
            id = 2,
            name = "Ổn",
            color = "has-text-info",
        });

        list.Add(new StaticModel
        {
            id = 3,
            name = "Không ổn",
            color = "has-text-danger",
        });

        return list;
    }


    public static List<StaticModel> Type()
    {
        var list = new List<StaticModel>();

        list.Add(new StaticModel
        {
            id = 2,
            name = "OKRs cam kết",
            color = "is-danger",
        });

        list.Add(new StaticModel
        {
            id = 1,
            name = "OKRs mở rộng",
            color = "is-primary",
        });

        return list;
    }
    // Mức độ tự tin: chi tiết
    public static StaticModel Confident(int id)
    {
        var query = from s in Confident()
                    where s.id == id
                    select s;
        if (query.Count() > 0)
            return query.FirstOrDefault();
        return new StaticModel();
    }

}
