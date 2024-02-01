using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Bson.Serialization.Attributes;

namespace ProjectOKRs.Models;

public class OkrsCheckin
{
  [BsonId]
  public string id { get; set; }
  public string cycle { get; set; }
  public long date_created { get; set; }
  public string user_created { get; set; }
  public int status_checkin { get; set; }
  public long next_date_checkin { get; set; }
  public long date_checkin { get; set; }
  public string user_checkin { get; set; }
  public bool draft { get; set; }
  public int confident { get; set; }
  public double progress_prev { get; set; }
  public string okr { get; set; }
  public bool is_done { get; set; }
  public List<KeyResult> key_results { get; set; } = new();
  public List<Feedback> feedbacks { get; set; } = new();

  public class KeyResult
  {
    public string id { get; set; }
    public double result { get; set; }
    public double change { get; set; }
    public int confident { get; set; }
    public List<string> questions { get; set; }
    public string feedback { get; set; }
  }
  public class Feedback
  {
    public string id { get; set; }
    public string user { get; set; }
    public string content { get; set; }
    public string kr { get; set; }
    public long date { get; set; }
  }
}
