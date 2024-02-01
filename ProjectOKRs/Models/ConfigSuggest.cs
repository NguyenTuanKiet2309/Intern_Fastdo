using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Bson.Serialization.Attributes;

namespace ProjectOKRs.Models;

public class ConfigSuggest
{
    [BsonId]
     public string id { get; set; }
    public string name { get; set; }
}
