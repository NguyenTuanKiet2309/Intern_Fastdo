using MongoDB.Driver;
using ProjectOKRs.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectOKRs.Data
{
   public class DBTimeCheckin
   {
      private static string nameDB = "OKRsApp";
      private static string collectionDB = "time_checkin";

      public static async Task<TimeCheckin> Create(TimeCheckin model)
      {
         model.id = Guid.NewGuid().ToString();

         var connect = MongoDB.ConnectMongoDB(nameDB);
         var collection = connect.GetCollection<TimeCheckin>(collectionDB);

         await collection.InsertOneAsync(model);

         return model;
      }


      public static async Task<TimeCheckin> Update(TimeCheckin model)
      {
         var connect = MongoDB.ConnectMongoDB(nameDB);
         var collection = connect.GetCollection<TimeCheckin>(collectionDB);
         var option = new ReplaceOptions { IsUpsert = false };

         var result = await collection.ReplaceOneAsync(x => x.id.Equals(model.id), model, option);

         return model;
      }


      public static async Task<bool> Delete(string id)
      {
         var connect = MongoDB.ConnectMongoDB(nameDB);
         var collection = connect.GetCollection<TimeCheckin>(collectionDB);

         var result = await collection.DeleteOneAsync(x => x.id == id);

         if (result.DeletedCount > 0)
            return true;
         else
            return false;
      }

      public static async Task<TimeCheckin> Get(string id)
      {
         var connect = MongoDB.ConnectMongoDB(nameDB);
         var collection = connect.GetCollection<TimeCheckin>(collectionDB);

         return await collection.Find(x => x.id == id).FirstOrDefaultAsync();
      }


      public static async Task<TimeCheckin> GetbyOkr(string okr)
      {
         var connect = MongoDB.ConnectMongoDB(nameDB);
         var collection = connect.GetCollection<TimeCheckin>(collectionDB);

         var result = await collection.FindAsync(x => x.okr == okr && x.status == 0).Result.FirstOrDefaultAsync();

         return result;
      }


      public static async Task<List<TimeCheckin>> GetList(string cycle,
        DateTime? start, DateTime? end)
      {
         var connect = MongoDB.ConnectMongoDB(nameDB);
         var collection = connect.GetCollection<TimeCheckin>(collectionDB);

         var builder = Builders<TimeCheckin>.Filter;

         var filtered = builder.Eq("cycle", cycle);

         if (start != null)
            filtered = filtered & builder.Gte("date_checkin", start.Value.Ticks);
         if (end != null)
            filtered = filtered & builder.Lt("date_checkin", end.Value.AddDays(1).Ticks);

         var result = await collection.FindAsync(filtered).Result.ToListAsync();

         return (from x in result orderby x.date_checkin descending select x).ToList();
      }
   }
}
