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

        public DbSet<Account> account { get; set; }
        public DbSet<Role> role { get; set; }
        public DbSet<AdmissionInformation> admission_information { get; set; }
        public DbSet<AdmissionMethod> admission_method { get; set; }
        public DbSet<Answer> answer { get; set; }
        public DbSet<Booking> booking { get; set; }
        public DbSet<Consultant> consultant { get; set; }
        public DbSet<Certification> certification { get; set; }
        public DbSet<ConsultationDay> consultation_day { get; set; }
        public DbSet<ConsultationTime> consultation_time { get; set; }
        public DbSet<ConsultantLevel> consultant_level { get; set; }
        public DbSet<HighSchool> high_school { get; set; }
        public DbSet<ImageNews> image_news { get; set; }
        public DbSet<Like> like { get; set; }
        public DbSet<Major> major { get; set; }
        public DbSet<MajorType> major_type { get; set; }
        public DbSet<News> news { get; set; }
        public DbSet<Notification> notification { get; set; }
        public DbSet<PersonalGroup> personal_group { get; set; }
        public DbSet<PersonalTest> personal_test { get; set; }
        public DbSet<Question> question { get; set; }
        public DbSet<RefreshToken> refresh_token { get; set; }
        public DbSet<Region> region { get; set; }
        public DbSet<Student> student { get; set; }
        public DbSet<StudentTest> student_test { get; set; }
        public DbSet<TestQuestion> test_question { get; set; }
        public DbSet<TestType> test_type { get; set; }
        public DbSet<TimeSlot> time_slot { get; set; }
        public DbSet<Transaction> transaction { get; set; }
        public DbSet<University> university { get; set; }
        public DbSet<Wallet> wallet { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Account>(entity =>
            {
                entity.HasKey(a => a.Id);
                entity.HasMany(a => a.Notifications).WithOne(n => n.Account).HasForeignKey(n => n.AccountId).OnDelete(DeleteBehavior.Restrict);
                entity.HasMany(a => a.Likes).WithOne(l => l.Account).HasForeignKey(l => l.AccountId).OnDelete(DeleteBehavior.Restrict);
            });

            // expert
            modelBuilder.Entity<Consultant>(entity =>
            {
                entity.HasKey(c => c.Id);
                entity.HasOne(c => c.Account).WithOne(a => a.CareerExpert).HasForeignKey<Consultant>(c => c.AccountId);
                entity.HasMany(c => c.Certifications).WithOne(c => c.Expert).HasForeignKey(c => c.ExpertId).OnDelete(DeleteBehavior.Restrict);
                entity.HasMany(c => c.ConsultationDays).WithOne(c => c.Expert).HasForeignKey(c => c.ExpertId).OnDelete(DeleteBehavior.Restrict);
            });

            // student 
            modelBuilder.Entity<Student>(entity =>
            {
                entity.HasKey(s => s.Id);
                entity.HasOne(s => s.Account).WithOne(s => s.Student).HasForeignKey<Student>(s => s.AccountId);
                entity.HasMany(s => s.StudentTests).WithOne(s => s.Student).HasForeignKey(s => s.StudentId).OnDelete(DeleteBehavior.Restrict);
                entity.HasMany(s => s.Bookings).WithOne(s => s.Student).HasForeignKey(s => s.StudentId).OnDelete(DeleteBehavior.Restrict);
            });

            // high school
            modelBuilder.Entity<HighSchool>(entity =>
            {
                entity.HasKey(h => h.Id);
                entity.HasOne(h => h.Account).WithOne(a => a.HighSchool).HasForeignKey<HighSchool>(h => h.AccountId);
                entity.HasMany(h => h.Students).WithOne(s => s.HighSchool).HasForeignKey(s => s.HighSchoolId).OnDelete(DeleteBehavior.Restrict);
            });

            // university
            modelBuilder.Entity<University>(entity =>
            {
                entity.HasKey(u => u.Id);
                entity.HasOne(u => u.Account).WithOne(a => a.University).HasForeignKey<University>(u => u.AccountId);
                entity.HasMany(u => u.AdmissionInformation).WithOne(u => u.University).HasForeignKey(u => u.UniversityId).OnDelete(DeleteBehavior.Restrict);
                entity.HasMany(u => u.News).WithOne(u => u.University).HasForeignKey(u => u.UniversityId).OnDelete(DeleteBehavior.Restrict);
            });

            // personal test
            modelBuilder.Entity<PersonalTest>(entity =>
            {
                entity.HasKey(p => p.Id);
                entity.HasMany(p => p.TestQuestions).WithOne(q => q.PersonalTest).HasForeignKey(q => q.PersonalTestId).OnDelete(DeleteBehavior.Restrict);
            });

            // student test
            modelBuilder.Entity<StudentTest>(entity =>
            {
                entity.HasKey(st => st.Id);
                entity.HasOne(st => st.Student).WithMany(s => s.StudentTests).HasForeignKey(st => st.StudentId).OnDelete(DeleteBehavior.Restrict);
                entity.HasOne(st => st.PersonalTest).WithMany(pt => pt.StudentTests).HasForeignKey(st => st.PersonalTestId).OnDelete(DeleteBehavior.Restrict);
                entity.HasOne(st => st.PersonalGroup).WithMany(pg => pg.StudentTests).HasForeignKey(st => st.PersonalGroupId).OnDelete(DeleteBehavior.Restrict);
            });

            // question
            modelBuilder.Entity<Question>(entity =>
            {
                entity.HasKey(q => q.Id);
                entity.HasMany(q => q.Answers).WithOne(a => a.Question).HasForeignKey(a => a.QuestionId).OnDelete(DeleteBehavior.Restrict);
            });

            // answer
            modelBuilder.Entity<Answer>(entity =>
            {
                entity.HasKey(a => a.Id);
            });

            //major type
            modelBuilder.Entity<MajorType>(entity =>
            {
                entity.HasKey(mt => mt.Id);
                entity.HasOne(mt => mt.Major).WithMany(m => m.MajorTypes).HasForeignKey(m => m.MajorId).OnDelete(DeleteBehavior.Restrict);
                entity.HasOne(mt => mt.PersonalGroup).WithMany(m => m.MajorTypes).HasForeignKey(m => m.PersonalGroupId).OnDelete(DeleteBehavior.Restrict);
            });

            // other entities
            modelBuilder.Entity<AdmissionInformation>(entity => entity.HasKey(ai => ai.Id));
            modelBuilder.Entity<AdmissionMethod>(entity => entity.HasKey(am => am.Id));
            modelBuilder.Entity<Booking>(entity => entity.HasKey(b => b.Id));
            modelBuilder.Entity<Certification>(entity => entity.HasKey(c => c.Id));
            modelBuilder.Entity<ConsultationDay>(entity => entity.HasKey(cd => cd.Id));
            modelBuilder.Entity<ConsultationTime>(entity => entity.HasKey(ct => ct.Id));
            modelBuilder.Entity<ConsultantLevel>(entity => entity.HasKey(el => el.Id));
            modelBuilder.Entity<ImageNews>(entity => entity.HasKey(im => im.Id));
            modelBuilder.Entity<Like>(entity => entity.HasKey(l => l.Id));
            modelBuilder.Entity<Major>(entity => entity.HasKey(m => m.Id));
            modelBuilder.Entity<News>(entity => entity.HasKey(n => n.Id));
            modelBuilder.Entity<Notification>(entity => entity.HasKey(n => n.Id));
            modelBuilder.Entity<PersonalGroup>(entity => entity.HasKey(pg => pg.Id));
            modelBuilder.Entity<RefreshToken>(entity => entity.HasKey(rt => rt.Id));
            modelBuilder.Entity<Region>(entity => entity.HasKey(r => r.Id));
            modelBuilder.Entity<TestQuestion>(entity => entity.HasKey(tq => tq.Id));
            modelBuilder.Entity<TestType>(entity => entity.HasKey(tt => tt.Id));
            modelBuilder.Entity<TimeSlot>(entity => entity.HasKey(ts => ts.Id));
            modelBuilder.Entity<Transaction>(entity => entity.HasKey(t => t.Id));
            modelBuilder.Entity<Wallet>(entity => entity.HasKey(w => w.Id));
            modelBuilder.Entity<Role>(entity => entity.HasKey(r => r.Id));

            base.OnModelCreating(modelBuilder);
        }
    }
}
