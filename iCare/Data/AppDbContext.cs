﻿using Microsoft.EntityFrameworkCore;
using iCare.Models;

namespace iCare.Data
{
    public class AppDbContext : DbContext
    {
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Patient> Patients { get; set; }
        public DbSet<PatientRecord> PatientRecords { get; set; }
        public DbSet<Document> Documents { get; set; } // Removed redundant full qualification
    }
}