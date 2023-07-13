﻿using Microsoft.EntityFrameworkCore;
using PlanIt.Identity.Application.Interfaces;
using PlanIt.Identity.Domain.Entities;
using PlanIt.Identity.Persistence.EntityTypeConfigurations;

namespace PlanIt.Identity.Persistence
{
    public class ApplicationDbContext : DbContext, IApplicationDbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<UserData> UserDatas { get; set; }
        public DbSet<RefreshSession> RefreshSessions { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new RefreshSessionConfiguration());
            modelBuilder.ApplyConfiguration(new UserConfiguration());
            modelBuilder.ApplyConfiguration(new UserDataConfiguration());
            base.OnModelCreating(modelBuilder);
        }
    }
}
