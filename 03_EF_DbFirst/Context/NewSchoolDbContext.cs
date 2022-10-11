using System;
using System.Collections.Generic;
using EF_DbFirst.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace EF_DbFirst.Context
{
    public partial class NewSchoolDbContext : DbContext
    {
        public NewSchoolDbContext()
        {
        }

        public NewSchoolDbContext(DbContextOptions<NewSchoolDbContext> options)
            : base(options)
        {
            Database.EnsureCreated();
        }

        public virtual DbSet<Course> Courses { get; set; } = null!;
        public virtual DbSet<Student> Students { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("Server=DevCenter;Database=NewSchoolDb;Uid=sa;Pwd=B0ba1964;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Course>(entity =>
            {
                entity.ToTable("Course");

                entity.Property(e => e.Title).HasMaxLength(60);

                entity.HasMany(d => d.StudentsStudents)
                    .WithMany(p => p.CoursesCourses)
                    .UsingEntity<Dictionary<string, object>>(
                        "CourseStudent",
                        l => l.HasOne<Student>().WithMany().HasForeignKey("StudentsStudentId"),
                        r => r.HasOne<Course>().WithMany().HasForeignKey("CoursesCourseId"),
                        j =>
                        {
                            j.HasKey("CoursesCourseId", "StudentsStudentId");

                            j.ToTable("CourseStudent");

                            j.HasIndex(new[] { "StudentsStudentId" }, "IX_CourseStudent_StudentsStudentId");
                        });
            });

            modelBuilder.Entity<Student>(entity =>
            {
                entity.ToTable("Student");

                entity.Property(e => e.LastName).HasMaxLength(30);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
