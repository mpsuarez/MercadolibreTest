using Microsoft.EntityFrameworkCore;
using Proxy.Domain;
using Proxy.Persistence.Database.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Proxy.Persistence.Database
{
    public class ProxyDbContext : DbContext
    {

        public virtual DbSet<GeneralSettings> GeneralSettings { get; set; }

        public virtual DbSet<Request> Request { get; set; }

        public virtual DbSet<Response> Response { get; set; }

        public virtual DbSet<SettingsByEndpoint> SettingsByEndpoint { get; set; }

        public virtual DbSet<SettingsByIP> SettingsByIP { get; set; }

        public ProxyDbContext(DbContextOptions<ProxyDbContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            
            GeneralSettingsConfiguration.SetEntityBuilder(modelBuilder.Entity<GeneralSettings>());
            RequestConfiguration.SetEntityBuilder(modelBuilder.Entity<Request>());
            ResponseConfiguration.SetEntityBuilder(modelBuilder.Entity<Response>());
            SettingsByEndpointConfiguration.SetEntityBuilder(modelBuilder.Entity<SettingsByEndpoint>());
            SettingsByIPConfiguration.SetEntityBuilder(modelBuilder.Entity<SettingsByIP>());

            base.OnModelCreating(modelBuilder);

        }

    }
}
