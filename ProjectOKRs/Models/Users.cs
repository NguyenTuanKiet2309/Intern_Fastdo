using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace ProjectOKRs.Models
{
    public class    Users
    {
        [BsonId]
        public string Id { get; set; }
        public string fullname { get; set; }
        public bool delete { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Role { get; set; }
        public bool is_admin { get; set; }
        public bool okr_checkin { get; set; }
        public string okr_cycle { get; set; }
    }
}