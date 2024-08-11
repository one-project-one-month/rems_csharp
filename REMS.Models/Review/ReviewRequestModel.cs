using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace REMS.Models.Review
{
    public class ReviewRequestModel
    {
        public int? UserId { get; set; }

        public int? PropertyId { get; set; }

        public int Rating { get; set; }

        public string? Comments { get; set; }

        [JsonIgnore]
        public DateTime? DateCreated { get; set; } = DateTime.Now;
    }
}
