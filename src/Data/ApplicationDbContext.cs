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
        public DbSet<Clubs> Clubs { get; set; }
        public DbSet<Events> Events { get; set; }
        public DbSet<Faculties> Faculties { get; set; }
        public DbSet<Notifications> Notifications { get; set; }
        public DbSet<Report> Reports { get; set; }
        public DbSet<Resgistrations> Resgistrations { get; set; }
        public DbSet<Semester> Semesters { get; set; }
        public DbSet<TrainingScores> TrainingScores { get; set; }
        public DbSet<Users> Users { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> dbContextOptions) : base(dbContextOptions) {}

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) {}

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
                entity.Property(c => c.Description)
                    .IsRequired()
                    .HasMaxLength(200)
                    .IsUnicode();
                entity.Property(c => c.UserId)
                    .IsRequired();

                entity.HasOne(c => c.Users)
                    .WithMany(u => u.Clubs)
                    .HasForeignKey(c => c.UserId)
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
                entity.Property(e => e.UserId)
                    .IsRequired();

                entity.HasOne(e => e.Users)
                    .WithMany(u => u.Events)
                    .HasForeignKey(e => e.UserId)
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

                entity.HasOne(n => n.Events)
                    .WithMany(e => e.Notifications)
                    .HasForeignKey(n => n.EventId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(n => n.Clubs)
                    .WithMany(e => e.Notifications)
                    .HasForeignKey(n => n.ClubId)
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
            });

            modelBuilder.Entity<Resgistrations>(entity =>
            {
                entity.HasKey(r => r.Id);
                entity.Property(r => r.Id)
                    .ValueGeneratedOnAdd()
                    .IsRequired();
                entity.Property(r => r.Status)
                    .IsRequired()
                    .HasMaxLength(100);
                entity.Property(r => r.DateResgistered)
                    .IsRequired();
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
                entity.Property(t => t.UserId)
                    .IsRequired();

                entity.HasOne(t => t.Users)
                    .WithOne(t => t.TrainingScores)
                    .HasForeignKey<TrainingScores>(t => t.UserId)
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
                entity.Property(u => u.Role)
                    .IsRequired()
                    .HasMaxLength(10);
                entity.Property(u => u.Faculty)
                    .IsRequired()
                    .HasMaxLength(10);
                entity.Property(u => u.Class)
                    .IsRequired()
                    .HasMaxLength(10);
                entity.Property(u => u.Semester)
                    .IsRequired()
                    .HasMaxLength(10);
            });
        }
    }
}