﻿using Base.Infrastructure.Persistence;
using Configuration.Domain;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using NewScotApp.Setup.CurrentUser;
using System.Reflection;

namespace Configuration.Infrastructure
{
    public class ConfigurationsDbContext : ApplicationDbContext
    {
        public ConfigurationsDbContext(IHttpContextAccessor httpContextAccessor) : base(httpContextAccessor)
        {
        }

        public ConfigurationsDbContext(DbContextOptions<ConfigurationsDbContext> options, IHttpContextAccessor httpContextAccessor)
            : base(options, httpContextAccessor, accessor => CurrentUser.Id)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {

            base.OnModelCreating(builder);
            builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }


        public DbSet<Country> Countries => Set<Country>();
        public DbSet<City> Cities => Set<City>();
        public DbSet<Region> Regions => Set<Region>();
    }
}