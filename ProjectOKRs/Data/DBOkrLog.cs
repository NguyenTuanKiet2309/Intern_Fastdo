using MongoDB.Driver;
using ProjectOKRs.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectOKRs.Data;

public class DBOkrLog
{
   private static string nameDB = "OKRsApp";
   private static string collectionDB = "okrs_history";

   public static async Task<OkrLog> Create( OkrLog model)
   {
      if (string.IsNullOrEmpty(model.id))
         model.id = MongoDB.RandomId();
      model.created = DateTime.Now.Ticks;

      var connect = MongoDB.ConnectMongoDB(nameDB);
      var collection = connect.GetCollection<OkrLog>(collectionDB);
      await collection.InsertOneAsync(model);

      return model;
   }


   public static async Task<OkrLog> Update( OkrLog model)
   {
      var connect = MongoDB.ConnectMongoDB(nameDB);
      var collection = connect.GetCollection<OkrLog>(collectionDB);
      var option = new ReplaceOptions { IsUpsert = false };
      var result = await collection.ReplaceOneAsync(x => x.id.Equals(model.id), model, option);

      return model;
   }

   public static async Task<List<OkrLog>> GetList(string userId)
   {
      var connect = MongoDB.ConnectMongoDB(nameDB);
      var collection = connect.GetCollection<OkrLog>(collectionDB);
      var results = await collection.FindAsync(x => x.user_id == userId).Result.ToListAsync();

      return results.OrderByDescending(x => x.created).ToList();
   }
}