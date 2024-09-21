using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entity
{
    public class RefreshToken : BasicEntity
    {
        public Guid AccountId { get; set; }
        public virtual Account Account { get; set; } = null!;
        public string Token { get; set; } = null!;
        public string JwtId { get; set; } = null!;
        public bool IsUsed { get; set; }
        public bool IsRevoked { get; set; }
        public DateTime ExpireAt { get; set; }
        public DateTime IssuedAt { get; set; }
    }
}
