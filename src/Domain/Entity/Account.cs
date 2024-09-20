using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Enum;
using Microsoft.AspNetCore.Authorization.Infrastructure;

namespace Domain.Entity
{
    public class Account
    {
        [Key]
        public Guid Id { get; set; }
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty!;
        public string Phone { get; set; } = string.Empty;
        public string? Image_Url { get; set; }
        public Role RoleId { get; set; }
        public bool Status { get; set; }
        public string? VerifyToken { get; set; } 
        public string? ResetPasswordToken { get; set; } 
        public DateTime CreateAt { get; set; }
        public DateTime VerifyAt { get; set; }
        public DateTime ResetPasswordAt { get; set; }
        public virtual ICollection<Notification> Notifications { get; set; } = new List<Notification>();
        public virtual ICollection<Like> Likes { get; set; } = new List<Like>();
        public virtual RefreshToken RefreshTokens { get; set; } = null!;
        public virtual Wallet Wallet { get; set; } = null!;
        public virtual Student Student { get; set; } = null!;
        public virtual CareerExpert CareerExpert { get; set; } = null!;
        public virtual HighSchool HighSchool { get; set; } = null!;
        public virtual University University { get; set; } = null!;
    }
}
