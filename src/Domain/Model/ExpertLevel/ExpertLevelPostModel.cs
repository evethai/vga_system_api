using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Domain.Model.ExpertLevel
{
    public class ExpertLevelPostModel
    {
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public double PriceOnSlot { get; set; }
        [JsonIgnore]
        public bool Status { get; set; } = true;
    }
}
