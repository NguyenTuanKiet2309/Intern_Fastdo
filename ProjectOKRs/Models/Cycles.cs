using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Bson.Serialization.Attributes;

namespace ProjectOKRs.Models;

public class Cycles
{   
    [BsonId]
    public string id { get; set; }
    public string name { get; set; }
    public long start_date { get; set; }
    public long end_date { get; set; }
    public bool is_done { get; set; }
}
