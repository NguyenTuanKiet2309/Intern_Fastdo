using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Driver;
using ProjectOKRs.Models;

namespace ProjectOKRs.Data;

public class DBOkrsCheckin
{

    private static string nameDB = "OKRsApp";
    private static string collectionDB = "okrs_checkin";
    public static async Task<OkrsCheckin> Get(string id)
    {
        var connect = MongoDB.ConnectMongoDB(nameDB);
        var collection = connect.GetCollection<OkrsCheckin>(collectionDB);

        return await collection.FindAsync(x => x.id == id).Result.FirstOrDefaultAsync();
    }

    public static async Task<OkrsCheckin> Create(OkrsCheckin model)
    {
        if (string.IsNullOrEmpty(model.id))
            model.id = MongoDB.RandomId();
        if (model.key_results == null)
            model.key_results = new();
        model.date_created = DateTime.Now.Ticks;

        var connect = MongoDB.ConnectMongoDB(nameDB);
        var collection = connect.GetCollection<OkrsCheckin>(collectionDB);

        await collection.InsertOneAsync(model);

        return model;
    }
    public static async Task<List<OkrsCheckin>> GetList(string cycle, string okr, bool? checkin)
    {
        var connect = MongoDB.ConnectMongoDB(nameDB);
        var collection = connect.GetCollection<OkrsCheckin>(collectionDB);

        var builder = Builders<OkrsCheckin>.Filter;

        var filtered = builder.Eq("cycle", cycle)
          & builder.Eq("okr", okr);

        if (checkin != null)
            filtered = filtered & builder.Eq("is_done", checkin.Value);

        var sorted = Builders<OkrsCheckin>.Sort.Descending("date_created");

        var result = await collection.FindAsync(filtered).Result.ToListAsync();

        result = result.OrderByDescending(x => x.date_created).ToList();

        return result;
    }

    public static async Task<bool> Delete(string id)
    {
        var connect = MongoDB.ConnectMongoDB(nameDB);
        var collection = connect.GetCollection<OkrsCheckin>(collectionDB);

        var result = await collection.DeleteOneAsync(x => x.id == id);

        if (result.DeletedCount > 0)
            return true;
        else
            return false;
    }
    public static double GetProgress(List<OkrsCheckin.KeyResult> keyCheckin, List<OKRs.KeyResult> keyResults)
    {
        if (keyCheckin != null && keyCheckin.Count > 0)
        {
            double total = 0;
            foreach (var kr in keyResults)
            {
                var checkin = keyCheckin.SingleOrDefault(x => x.id == kr.id);
                if (checkin != null)
                    total += Shared.Components.Shared.Progress(checkin.result, kr.goal);
            }
            return total / keyResults.Count;
        }
        else
            return 0;
    }


    public static async Task<OkrsCheckin> Update(OkrsCheckin model)
    {
        var connect = MongoDB.ConnectMongoDB(nameDB);
        var collection = connect.GetCollection<OkrsCheckin>(collectionDB);
        var option = new ReplaceOptions { IsUpsert = false };
        var result = await collection.ReplaceOneAsync(x => x.id.Equals(model.id), model, option);

        return model;
    }

    public static async Task<List<OkrsCheckin>> GetListChart(string cycle, string okr, bool? checkin)
    {
        var connect = MongoDB.ConnectMongoDB(nameDB);
        var collection = connect.GetCollection<OkrsCheckin>(collectionDB);

        var builder = Builders<OkrsCheckin>.Filter;

        var filtered = builder.Eq("cycle", cycle)
            & builder.Eq("okr", okr);

        if (checkin != null)
            filtered = filtered & builder.Eq("is_done", checkin.Value);

        var sorted = Builders<OkrsCheckin>.Sort.Descending("date_create");
        var result = await collection.FindAsync(filtered).Result.ToListAsync();
        result = result.OrderByDescending(x => x.date_created).ToList();

        return result;
    }


    public static List<StaticModel> Question()
    {
        var list = new List<StaticModel>();

        list.Add(new StaticModel
        {
            id = 0,
            name = "Tiến độ, kết quả công việc?"
        });

        list.Add(new StaticModel
        {
            id = 1,
            name = "Công việc nào đang & sẽ chậm tiến độ?"
        });

        list.Add(new StaticModel
        {
            id = 2,
            name = "Trở ngại, khó khăn là gì?"
        });

        list.Add(new StaticModel
        {
            id = 3,
            name = "Cần làm gì để vượt qua trở ngại?"
        });

        return list;
    }

    // Câu hỏi: chi tiết
    public static StaticModel Question(int id)
    {
        var query = from s in Question()
                    where s.id == id
                    select s;
        if (query.Count() > 0)
            return query.FirstOrDefault();
        return new StaticModel();

    }
}
