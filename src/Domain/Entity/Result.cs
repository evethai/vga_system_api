using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Enum;

namespace Domain.Entity
{
    public class Result
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public Guid StudentId { get; set; } 
        public Student Student { get; set; } = null!;

        public int TestId { get; set; }
        public Test Test { get; set; } = null!;

        public ResultPersonalityTypeMBIT Personality_Type { get; set; } 
        public string Personality_Description { get; set; } = null!;
        public DateTime CreateAt { get; set; }
    }
}
