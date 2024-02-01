using System;
using System.Collections.Generic;
using MongoDB.Bson.Serialization.Attributes;

namespace ProjectOKRs.Models;

[BsonIgnoreExtraElements]
public class OkrLog
{
  [BsonId]
  public string id { get; set; }

  public long created { get; set; }

  public string user_id { get; set; }

  public string okr_id { get; set; }

  public string action { get; set; }

  public Log old { get; set; } = new();

  public Log edit { get; set; } = new();

  public class Log
  {
    public string okr { get; set; }

    public List<string> kr { get; set; } = new();

    public List<string> goal { get; set; } = new();
  }
}
