using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using StudentActivities.src.Models;

namespace StudentActivities.src.Data
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<AcademicClasses> AcademicClasses { get; set; }
        public DbSet<Checkin> Checkins { get; set; }
        public DbSet<Clubs> Clubs { get; set; }
        public DbSet<Events> Events { get; set; }
        public DbSet<Faculties> Faculties { get; set; }
        public DbSet<Notifications> Notifications { get; set; }
        public DbSet<Report> Reports { get; set; }
        public DbSet<Resgistrations> Resgistrations { get; set; }
        public DbSet<Semester> Semesters { get; set; }
        public DbSet<TrainingScores> TrainingScores { get; set; }
        public DbSet<Users> Users { get; set; }
        public DbSet<Students> Students { get; set; }
        public DbSet<Admins> Admins { get; set; }
        public DbSet<Organizers> Organizers { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> dbContextOptions) : base(dbContextOptions) { }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AcademicClasses>(entity =>
            {
                entity.HasKey(a => a.Id);
                entity.Property(a => a.Id)
                    .ValueGeneratedOnAdd()
                    .IsRequired();
                entity.Property(a => a.Name)
                    .IsUnicode()
                    .HasMaxLength(20)
                    .IsRequired();
                entity.Property(a => a.FacultyId)
                    .IsRequired();

                entity.HasOne(a => a.Faculties)
                    .WithMany(f => f.AcademicClasses)
                    .HasForeignKey(a => a.FacultyId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<Checkin>(entity =>
            {
                entity.HasKey(c => c.Id);
                entity.Property(c => c.Id)
                    .ValueGeneratedOnAdd()
                    .IsRequired();
                entity.Property(c => c.StudentId)
                    .IsRequired();
                entity.Property(c => c.EventId)
                    .IsRequired();
                entity.Property(c => c.Status)
                    .HasMaxLength(20)
                    .IsRequired();
                entity.Property(c => c.CheckInTime);
                entity.Property(c => c.CheckedInBy);

                entity.HasOne(c => c.Students)
                    .WithOne(s => s.Checkin)
                    .HasForeignKey<Checkin>(c => c.StudentId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(c => c.Events)
                    .WithOne(e => e.Checkin)
                    .HasForeignKey<Checkin>(c => c.EventId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(c => c.CheckedInByUser)
                    .WithMany()
                    .HasForeignKey(c => c.CheckedInBy)
                    .OnDelete(DeleteBehavior.SetNull);
            });

            modelBuilder.Entity<Clubs>(entity =>
            {
                entity.HasKey(c => c.Id);
                entity.Property(c => c.Id)
                    .ValueGeneratedOnAdd()
                    .IsRequired();
                entity.Property(c => c.Name)
                    .IsUnicode()
                    .HasMaxLength(20)
                    .IsRequired();
                entity.Property(c => c.Thumbnail)
                    .IsRequired()
                    .HasMaxLength(500);
                entity.Property(c => c.Description)
                    .IsRequired()
                    .HasMaxLength(1000)
                    .IsUnicode();
                entity.Property(c => c.OrganizerId)
                    .IsRequired();

                entity.HasOne(c => c.Organizers)
                    .WithMany(o => o.Clubs)
                    .HasForeignKey(c => c.OrganizerId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<Events>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id)
                    .ValueGeneratedOnAdd()
                    .IsRequired();
                entity.Property(e => e.Name)
                    .IsUnicode()
                    .HasMaxLength(100)
                    .IsRequired();
                entity.Property(e => e.Thumbnail)
                    .IsRequired()
                    .HasMaxLength(500);
                entity.Property(e => e.Paticipants)
                    .IsRequired()
                    .HasMaxLength(500)
                    .IsUnicode();
                entity.Property(e => e.Description)
                    .IsRequired()
                    .HasMaxLength(1000)
                    .IsUnicode();
                entity.Property(e => e.DetailDescription)
                    .IsRequired()
                    .HasMaxLength(2000)
                    .IsUnicode();
                entity.Property(e => e.StartDate)
                    .IsRequired();
                entity.Property(e => e.EndDate)
                    .IsRequired();
                entity.Property(e => e.Location)
                    .IsRequired()
                    .HasMaxLength(300);
                entity.Property(e => e.MaxCapacity)
                    .IsRequired();
                entity.Property(e => e.CurrentRegistrations)
                    .IsRequired();
                entity.Property(e => e.OrganizerId)
                    .IsRequired();

                entity.HasOne(e => e.Organizers)
                    .WithMany(o => o.Events)
                    .HasForeignKey(e => e.OrganizerId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<Faculties>(entity =>
            {
                entity.HasKey(f => f.Id);
                entity.Property(f => f.Id)
                    .ValueGeneratedOnAdd()
                    .IsRequired();
                entity.Property(f => f.Name)
                    .IsRequired()
                    .IsUnicode()
                    .HasMaxLength(100);
            });

            modelBuilder.Entity<Notifications>(entity =>
            {
                entity.HasKey(n => n.Id);
                entity.Property(n => n.Id)
                    .ValueGeneratedOnAdd()
                    .IsRequired();
                entity.Property(n => n.Context)
                    .IsRequired()
                    .HasMaxLength(500);
                entity.Property(n => n.SendDate)
                    .IsRequired();
                entity.Property(n => n.ClubId)
                    .IsRequired();
                entity.Property(n => n.EventId)
                    .IsRequired();
                entity.Property(n => n.StudentId)
                    .IsRequired();
                entity.Property(n => n.Status)
                    .IsRequired()
                    .HasMaxLength(10);

                entity.HasOne(n => n.Events)
                    .WithMany(e => e.Notifications)
                    .HasForeignKey(n => n.EventId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(n => n.Clubs)
                    .WithMany(e => e.Notifications)
                    .HasForeignKey(n => n.ClubId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(n => n.Students)
                    .WithMany(e => e.Notifications)
                    .HasForeignKey(n => n.StudentId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<Report>(entity =>
            {
                entity.HasKey(r => r.Id);
                entity.Property(r => r.Id)
                    .ValueGeneratedOnAdd()
                    .IsRequired();
                entity.Property(r => r.Type)
                    .IsRequired()
                    .HasMaxLength(20);
                entity.Property(r => r.AdminId)
                    .IsRequired();
                entity.Property(r => r.OrganizerId)
                    .IsRequired();

                entity.HasOne(r => r.Admins)
                    .WithMany(a => a.Reports)
                    .HasForeignKey(r => r.AdminId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(r => r.Organizers)
                    .WithMany(o => o.Reports)
                    .HasForeignKey(r => r.OrganizerId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<Resgistrations>(entity =>
            {
                entity.HasKey(r => r.Id);
                entity.Property(r => r.Id)
                    .ValueGeneratedOnAdd()
                    .IsRequired();
                entity.Property(r => r.Status)
                    .IsRequired()
                    .HasMaxLength(20);
                entity.Property(r => r.Type)
                    .IsRequired()
                    .HasMaxLength(10);
                entity.Property(r => r.RegistrationDate)
                    .IsRequired();
                entity.Property(r => r.StudentId)
                    .IsRequired();

                entity.HasOne(r => r.Students)
                    .WithMany(s => s.Resgistrations)
                    .HasForeignKey(r => r.StudentId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(r => r.Clubs)
                    .WithMany(c => c.Resgistrations)
                    .HasForeignKey(r => r.ClubId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(r => r.Events)
                    .WithMany(e => e.Resgistrations)
                    .HasForeignKey(r => r.EventId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<Semester>(entity =>
            {
                entity.HasKey(s => s.Id);
                entity.Property(s => s.Id)
                    .ValueGeneratedOnAdd()
                    .IsRequired();
                entity.Property(s => s.Name)
                    .IsRequired()
                    .IsUnicode()
                    .HasMaxLength(50);
                entity.Property(s => s.StartDate)
                    .IsRequired();
                entity.Property(s => s.EndDate)
                    .IsRequired();
            });

            modelBuilder.Entity<TrainingScores>(entity =>
            {
                entity.HasKey(t => t.Id);
                entity.Property(t => t.Id)
                    .ValueGeneratedOnAdd()
                    .IsRequired();
                entity.Property(t => t.Score)
                    .IsRequired();
                entity.Property(t => t.DateAssigned)
                    .IsRequired();
                entity.Property(t => t.EventId)
                    .IsRequired();
                entity.Property(t => t.StudentId)
                    .IsRequired();
                entity.Property(t => t.SemesterId)
                    .IsRequired();

                entity.HasOne(t => t.Events)
                    .WithOne(s => s.TrainingScores)
                    .HasForeignKey<TrainingScores>(t => t.EventId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(t => t.Students)
                    .WithMany(s => s.TrainingScores)
                    .HasForeignKey(t => t.StudentId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(t => t.Semester)
                    .WithMany(s => s.TrainingScores)
                    .HasForeignKey(t => t.SemesterId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<Users>(entity =>
            {
                entity.HasKey(u => u.Id);
                entity.Property(u => u.Id)
                    .ValueGeneratedOnAdd()
                    .IsRequired();
                entity.Property(u => u.UserName)
                    .IsRequired()
                    .HasMaxLength(20);
                entity.Property(u => u.Password)
                    .IsRequired()
                    .HasMaxLength(10);
                entity.Property(u => u.Role)
                    .IsRequired();

                // entity.Property(u => u.FirstName)
                //     .IsRequired()
                //     .IsUnicode()
                //     .HasMaxLength(20);
                // entity.Property(u => u.LastName)
                //     .IsRequired()
                //     .IsUnicode()
                //     .HasMaxLength(20);
                // entity.Property(u => u.PhoneNumber)
                //     .IsRequired()
                //     .HasMaxLength(10);
                // entity.Property(u => u.Email)
                //     .IsRequired()
                //     .HasMaxLength(50);
                // entity.Property(u => u.Role)
                //     .IsRequired()
                //     .HasMaxLength(10);
                // entity.Property(u => u.Faculty)
                //     .IsRequired()
                //     .HasMaxLength(10);
                // entity.Property(u => u.Class)
                //     .IsRequired()
                //     .HasMaxLength(10);
                // entity.Property(u => u.Semester)
                //     .IsRequired()
                //     .HasMaxLength(10);
            });

            // modelBuilder.Entity<Users>()
            //     .HasDiscriminator<string>("UserType")
            //     .HasValue<Students>("Student")
            //     .HasValue<Admins>("Admin")
            //     .HasValue<Organizers>("Organizer");

            modelBuilder.Entity<Students>(entity =>
            {
                entity.HasKey(s => s.Id);
                entity.Property(s => s.Id)
                    .ValueGeneratedOnAdd()
                    .IsRequired();
                // entity.Property(s => s.Role)
                //     .IsRequired()
                //     .HasMaxLength(20);
                entity.Property(s => s.AcademicClassId)
                    .IsRequired();
                entity.Property(s => s.FacultyId)
                    .IsRequired();
                entity.Property(u => u.FirstName)
                    .IsRequired()
                    .IsUnicode()
                    .HasMaxLength(20);
                entity.Property(u => u.LastName)
                    .IsRequired()
                    .IsUnicode()
                    .HasMaxLength(20);
                entity.Property(u => u.PhoneNumber)
                    .IsRequired()
                    .HasMaxLength(10);
                entity.Property(u => u.Email)
                    .IsRequired()
                    .HasMaxLength(50);
                entity.Property(s => s.TrainingScore)
                    .IsRequired();

                entity.HasOne(s => s.Users)
                    .WithOne(u => u.Students)
                    .HasForeignKey<Students>(s => s.UserId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(s => s.AcademicClasses)
                    .WithMany(a => a.Students)
                    .HasForeignKey(s => s.AcademicClassId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<Admins>(entity =>
            {
                entity.HasKey(a => a.Id);
                entity.Property(a => a.Id)
                    .ValueGeneratedOnAdd()
                    .IsRequired();
                // entity.Property(a => a.Role)
                //     .IsRequired()
                //     .HasMaxLength(20);
                entity.Property(u => u.FirstName)
                    .IsRequired()
                    .IsUnicode()
                    .HasMaxLength(20);
                entity.Property(u => u.LastName)
                    .IsRequired()
                    .IsUnicode()
                    .HasMaxLength(20);
                entity.Property(u => u.PhoneNumber)
                    .IsRequired()
                    .HasMaxLength(10);
                entity.Property(u => u.Email)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.HasOne(a => a.Users)
                    .WithOne(u => u.Admins)
                    .HasForeignKey<Admins>(a => a.UserId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<Organizers>(entity =>
            {
                entity.HasKey(o => o.Id);
                entity.Property(o => o.Id)
                    .ValueGeneratedOnAdd()
                    .IsRequired();
                // entity.Property(o => o.Role)
                //     .IsRequired()
                //     .HasMaxLength(20);
                entity.Property(u => u.FirstName)
                    .IsRequired()
                    .IsUnicode()
                    .HasMaxLength(20);
                entity.Property(u => u.LastName)
                    .IsRequired()
                    .IsUnicode()
                    .HasMaxLength(20);
                entity.Property(u => u.PhoneNumber)
                    .IsRequired()
                    .HasMaxLength(10);
                entity.Property(u => u.Email)
                    .IsRequired()
                    .HasMaxLength(50);
                entity.Property(u => u.UserId)
                    .IsRequired();

                entity.HasOne(o => o.Users)
                    .WithOne(u => u.Organizers)
                    .HasForeignKey<Organizers>(o => o.UserId)
                    .OnDelete(DeleteBehavior.Cascade);
            });
        }
    }
}