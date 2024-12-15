using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Domain.Model.ConsultantRelation
{
    public class ConsultantRelationPostPutModel
    {
        public Guid? Id { get; set; }
        //[Required(ErrorMessage = "UniversityId is required")]
        public Guid UniversityId { get; set; }
        [JsonIgnore]
        public bool Status { get; set; } = true;
    }
}
