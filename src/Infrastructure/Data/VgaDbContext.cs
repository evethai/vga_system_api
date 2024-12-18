﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Common.Hubs;
using Domain.Entity;
using Domain.Enum;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data
{
    public class VgaDbContext : DbContext
    {
        private readonly IHubContext<NotificationHub> _hubContext;
        private readonly UserConnectionManager _userConnectionManager;
        public VgaDbContext(DbContextOptions<VgaDbContext> options,IHubContext<NotificationHub> hubContext, UserConnectionManager userConnectionManager) : base(options)
        {
            _hubContext = hubContext;
            _userConnectionManager = userConnectionManager;
        }

        public DbSet<Account> Account { get; set; }
        public DbSet<AdmissionInformation> AdmissionInformation { get; set; }
        public DbSet<AdmissionMethod> AdmissionMethod { get; set; }
        public DbSet<Answer> Answer { get; set; }
        public DbSet<Booking> Booking { get; set; }
        public DbSet<Consultant> Consultant { get; set; }
        public DbSet<Certification> Certification { get; set; }
        public DbSet<ConsultationDay> ConsultationDay { get; set; }
        public DbSet<ConsultationTime> ConsultationTime { get; set; }
        public DbSet<ConsultantLevel> ConsultantLevel { get; set; }
        public DbSet<HighSchool> HighSchool { get; set; }
        public DbSet<ImageNews> ImageNews { get; set; }
        public DbSet<Major> Major { get; set; }
        public DbSet<MajorPersonalityMatrix> MajorPersonalMatrix { get; set; }
        public DbSet<News> News { get; set; }
        public DbSet<Notification> Notification { get; set; }
        public DbSet<PersonalGroup> PersonalGroup { get; set; }
        public DbSet<PersonalTest> PersonalTest { get; set; }
        public DbSet<Question> Question { get; set; }
        public DbSet<RefreshToken> RefreshToken { get; set; }
        public DbSet<Region> Region { get; set; }
        public DbSet<Student> Student { get; set; }
        public DbSet<StudentTest> StudentTest { get; set; }
        public DbSet<TestQuestion> TestQuestion { get; set; }
        public DbSet<TestType> TestType { get; set; }
        public DbSet<TimeSlot> TimeSlot { get; set; }
        public DbSet<Transaction> Transaction { get; set; }
        public DbSet<University> University { get; set; }
        public DbSet<Wallet> Wallet { get; set; }
        public DbSet<Occupation> Occupation { get; set; }
        public DbSet<OccupationalGroup> OccupationalGroup { get; set; }
        public DbSet<EntryLevelEducation> EntryLevelEducation { get; set; }
        public DbSet<WorkSkills> WorkSkills { get; set; }
        public DbSet<OccupationalSKills> OccupationalSKills { get; set; }
        public DbSet<MajorCategory> MajorCategory { get; set; }
        public DbSet<MajorOccupationMatrix> MajorOccupationMatrix { get; set; }
        public DbSet<StudentChoice> StudentChoice { get; set; }
        public DbSet<UniversityLocation> UniversityLocation { get; set; }
        public DbSet<ConsultantRelation> ConstantRelation { get; set; }


        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {

            var entries = ChangeTracker.Entries<Notification>();

            foreach (var entry in entries)
            {
                if (entry.State == EntityState.Added || entry.State == EntityState.Modified )
                {
                    var accountId = entry.Entity.AccountId.ToString();

                    var connections = _userConnectionManager.GetConnections(accountId);

                    var tasks = connections.Select(connectionId =>
                        _hubContext.Clients.Client(connectionId).SendAsync("ReceiveNotification", new
                        {
                            Title = entry.Entity.Title,
                            Message = entry.Entity.Message,
                            CreatedAt = entry.Entity.CreatedAt,
                        })
                    );

                    await Task.WhenAll(tasks);
                }
            }

            return await base.SaveChangesAsync(cancellationToken);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Account>(entity =>
            {
                entity.HasKey(a => a.Id);
                entity.HasMany(a => a.Notifications).WithOne(n => n.Account).HasForeignKey(n => n.AccountId).OnDelete(DeleteBehavior.Restrict);
                entity.HasIndex(a => a.Email).IsUnique();
                entity.HasIndex(a => a.Phone).IsUnique();
            });

            // expert
            modelBuilder.Entity<Consultant>(entity =>
            {
                entity.HasKey(c => c.Id);
                entity.HasOne(c => c.Account).WithOne(a => a.Consultant).HasForeignKey<Consultant>(c => c.AccountId);
                entity.HasOne(c => c.ConsultantLevel).WithMany(cl => cl.Consultants).HasForeignKey(c => c.ConsultantLevelId).OnDelete(DeleteBehavior.Restrict);
                entity.HasMany(c => c.Certifications).WithOne(c => c.Consultant).HasForeignKey(c => c.ConsultantId).OnDelete(DeleteBehavior.Restrict);
                entity.HasMany(c => c.ConsultationDays).WithOne(c => c.Consultant).HasForeignKey(c => c.ConsultantId).OnDelete(DeleteBehavior.Restrict);
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
                //entity.HasMany(u => u.Consultants).WithOne(u => u.University).HasForeignKey(u => u.UniversityId).OnDelete(DeleteBehavior.Restrict);
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

            //consultantRelation
            modelBuilder.Entity<ConsultantRelation>(entity =>
            {
                entity.HasKey(c => c.Id);
                entity.HasOne(st => st.Consultant).WithMany(pt => pt.ConsultantRelations).HasForeignKey(st => st.ConsultantId).OnDelete(DeleteBehavior.Restrict);
                entity.HasOne(st => st.University).WithMany(pt => pt.ConsultantRelations).HasForeignKey(st => st.UniversityId).OnDelete(DeleteBehavior.Restrict);

            });

            modelBuilder.Entity<Certification>(entity =>
            {
                entity.HasKey(st => st.Id);
                entity.HasOne(st => st.Consultant).WithMany(pt => pt.Certifications).HasForeignKey(st => st.ConsultantId).OnDelete(DeleteBehavior.Restrict);
                entity.HasOne(st => st.Major).WithMany(pg => pg.Certifications).HasForeignKey(st => st.MajorId).OnDelete(DeleteBehavior.Restrict);
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
            modelBuilder.Entity<MajorPersonalityMatrix>(entity =>
            {
                entity.HasKey(mt => mt.Id);
                entity.HasOne(mt => mt.MajorCategory).WithMany(m => m.MajorPersonalMatrixs).HasForeignKey(m => m.MajorCategoryId).OnDelete(DeleteBehavior.Restrict);
                entity.HasOne(mt => mt.PersonalGroup).WithMany(m => m.MajorPersonalMatrixs).HasForeignKey(m => m.PersonalGroupId).OnDelete(DeleteBehavior.Restrict);
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
            modelBuilder.Entity<Occupation>(entity => entity.HasKey(o => o.Id));
            modelBuilder.Entity<OccupationalGroup>(entity => entity.HasKey(og => og.Id));
            modelBuilder.Entity<EntryLevelEducation>(entity => entity.HasKey(el => el.Id));
            modelBuilder.Entity<WorkSkills>(entity => entity.HasKey(ws => ws.Id));
            modelBuilder.Entity<OccupationalSKills>(entity => entity.HasKey(os => os.Id));
            modelBuilder.Entity<MajorCategory>(entity => entity.HasKey(mc => mc.Id));
            modelBuilder.Entity<MajorOccupationMatrix>(entity => entity.HasKey(moc => moc.Id));
            modelBuilder.Entity<StudentChoice>(entity => entity.HasKey(sc => sc.Id));


            base.OnModelCreating(modelBuilder);
        }
    }
}
