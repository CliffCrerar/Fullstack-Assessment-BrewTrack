using BrewTrack.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;

namespace BrewTrack.Data
{
    public class BrewTrackDbContext: DbContext
    {
        private IConfiguration _config;
        public BrewTrackDbContext(DbContextOptions<BrewTrackDbContext> options) :base(options)
        {
            _config = new ConfigurationBuilder().Build();
        }

        public BrewTrackDbContext()
        {
            _config = new ConfigurationBuilder().Build();
        }

        //protected override void OnConfiguring(DbContextOptionsBuilder options)
        //    => options.UseMySQL(_config.GetConnectionString("MySql"));

        public DbSet<User> Users => Set<User>();
        public DbSet<UserHistory> UserHistory => Set<UserHistory>();
        public DbSet<BrewPubs> Brewpubs => Set<BrewPubs>();
    }
}
