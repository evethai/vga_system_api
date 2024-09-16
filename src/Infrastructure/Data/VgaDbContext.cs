using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entity;
using Domain.Enum;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data
{
    public class VgaDbContext : DbContext
    {
        public VgaDbContext(DbContextOptions<VgaDbContext> options) : base(options)
        {
        }

        public DbSet<Answer> answer { get; set; }
        public DbSet<HighSchool> highSchool { get; set; }
        public DbSet<Major> major { get; set; }
        public DbSet<MajorType> major_type { get; set; }
        public DbSet<PersonalGroup> personal_group { get; set; }
        public DbSet<Region> Region { get; set; }
        public DbSet<Student> student { get; set; }
        public DbSet<StudentTest> student_test { get; set; }
        public DbSet<TestType> test_type { get; set; }
        public DbSet<PersonalTest> personal_test { get; set; }
        public DbSet<Question> question { get; set; }
        public DbSet<TestQuestion> test_question { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<TestQuestion>()
                .HasKey(tq => new { tq.QuestionId, tq.PersonalTestId });

            modelBuilder.Entity<TestQuestion>()
                .HasOne(tq => tq.PersonalTest)
                .WithMany(pt => pt.TestQuestions)
                .HasForeignKey(tq => tq.PersonalTestId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<TestQuestion>()
                .HasOne(tq => tq.Question)
                .WithMany(q => q.TestQuestions)
                .HasForeignKey(tq => tq.QuestionId)
                .OnDelete(DeleteBehavior.Restrict);  

            modelBuilder.Entity<StudentTest>()
                .HasKey(st => new { st.StudentId, st.PersonalTestId, st.PersonalGroupId });

            modelBuilder.Entity<StudentTest>()
                .HasOne(st => st.Student)
                .WithMany(s => s.StudentTests)
                .HasForeignKey(st => st.StudentId)
                .OnDelete(DeleteBehavior.Restrict);  

            modelBuilder.Entity<StudentTest>()
                .HasOne(st => st.PersonalTest)
                .WithMany(pt => pt.StudentTests)
                .HasForeignKey(st => st.PersonalTestId)
                .OnDelete(DeleteBehavior.Restrict);  

            modelBuilder.Entity<StudentTest>()
                .HasOne(st => st.PersonalGroup)
                .WithMany(pg => pg.StudentTests)
                .HasForeignKey(st => st.PersonalGroupId)
                .OnDelete(DeleteBehavior.Restrict);  

            modelBuilder.Entity<MajorType>().HasKey(mt => new { mt.MajorId, mt.PersonalGroupId });
            modelBuilder.Entity<MajorType>()
                .HasOne(mt => mt.Major)
                .WithMany(m => m.MajorTypes)
                .HasForeignKey(mt => mt.MajorId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<MajorType>()
                .HasOne(mt => mt.PersonalGroup)
                .WithMany(pg => pg.MajorTypes)
                .HasForeignKey(mt => mt.PersonalGroupId)
                .OnDelete(DeleteBehavior.Cascade);

            base.OnModelCreating(modelBuilder);
        }

    }
}
