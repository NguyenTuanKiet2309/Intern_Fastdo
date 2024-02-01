using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Bson.Serialization.Attributes;

namespace ProjectOKRs.Models
{
    [BsonIgnoreExtraElements]

    public class OKRs
    {
        [BsonId]
        public string Id { get; set; }
        public string name { get; set; }
        public double progress_prev { get; set; }
        public string cycleId { get; set; }
        public bool delete { get; set; }
        public int confident { get; set; }
        public int status_checkin { get; set; }
        public long next_checkin { get; set; }
        public long date_checkin { get; set; }
        public string user_checkin { get; set; }
        public int status_checkin_old { get; set; }
        public long date_checkin_confirm { get; set; }
        public string review_staff_comment { get; set; }
        public long date_created { get; set; }
        public string okr_manager { get; set; }
        public int review_send_status { get; set; }
        public int type { get; set; }
        public bool done { get; set; }
        public string user_created { get; set; }
        public string review_manager_comment { get; set; }
        public string review_manager_id { get; set; }
        public long review_send_date { get; set; }
        public long review_manager_date { get; set; }
        public Review review_data { get; set; } = new();
        public List<string> review_viewers { get; set; } = new();
        public List<KeyResult> key_results { get; set; }

        public class KeyResult
        {
            public string id { get; set; }
            // tên KR
            public string name { get; set; }
            // mục tiêu
            public double goal { get; set; }
            public double change { get; set; }
            public Review review_data { get; set; } = new();
            // đơn vị tính
            public string unit { get; set; }
            public string link { get; set; }

            public int confident { get; set; }
            // kết quả đạt được
            public double result { get; set; }

            public List<CrossLink> cross_linking { get; set; }
        }

        public class CrossLink
        {
            public string id { get; set; }
            public string okr { get; set; }
            public string user { get; set; }
        }

        public class Review
        {
            /// Nhân sự tự đánh giá
            public double staff_point { get; set; }

            /// Quản lý đánh giá
            public double manager_point { get; set; }

            /// Nhân sự tự nhận xét
            public string staff_comment { get; set; }

            /// Quản lý nhận xét
            public string manager_comment { get; set; }
        }
    }
}