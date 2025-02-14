﻿using Microsoft.EntityFrameworkCore;
using Training.Core.Models;

namespace Training.DAL
{
    public class TrainingDbContext : DbContext
    {
        private readonly string _connectionString;
        public DbSet<User> Users { get; set; }
        public DbSet<Book> Books { get; set; }
        public DbSet<Reservation> Reservations { get; set; }
        public DbSet<Album> Albums { get; set; }

        public TrainingDbContext(string connectionString)
        {
            _connectionString = connectionString;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(_connectionString);
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>(x =>
            {
                x.HasKey(x => x.Id);
                x.HasIndex(x => x.NationalIdentifier).IsUnique();
                x.Property(x => x.NationalIdentifier).HasMaxLength(15);
            });
            modelBuilder.Entity<Book>(x =>
            {
                x.HasKey(x => x.Id);
                x.HasIndex(x => x.ISBN).IsUnique();
                x.Property(x => x.ISBN).HasMaxLength(15);
            });
            modelBuilder.Entity<Album>(x =>
            {
                x.HasKey(x => x.Id);
            });
            modelBuilder.Entity<Reservation>(x =>
            {
                x.HasKey(x => x.Id);
                x.HasOne(x => x.User).WithMany(x => x.Reservations);
                x.HasOne(x => x.Book).WithMany(x => x.Reservations);
            });
        }
    }
}
