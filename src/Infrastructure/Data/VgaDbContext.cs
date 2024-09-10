using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entity;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data
{
    public class VgaDbContext : DbContext
    {
        public VgaDbContext(DbContextOptions<VgaDbContext> options) : base(options)
        {
        }

        public DbSet<Student> students { get; set; }
        public DbSet<Result> result { get; set; }
        public DbSet<Test> test { get; set; }
        public DbSet<Question> question { get; set; }
        public DbSet<Answer> answer { get; set; }
        public DbSet<StudentSelected> student_selected { get; set; }
        public DbSet<MBTIPersonality> mbti_personality { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            base.OnModelCreating(modelBuilder);
        }
    }
}
