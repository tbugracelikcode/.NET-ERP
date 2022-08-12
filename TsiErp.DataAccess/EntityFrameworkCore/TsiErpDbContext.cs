using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tsi.Core.Entities.Auditing;
using TsiErp.DataAccess.EntityFrameworkCore.Configurations;
using TsiErp.DataAccess.EntityFrameworkCore.Extensions;
using TsiErp.Entities.Entities.Branch;
using TsiErp.Entities.Entities.Period;

namespace TsiErp.DataAccess.EntityFrameworkCore
{
    public class TsiErpDbContext : DbContext
    {
        private IConfigurationRoot _configuration;

        public bool _IsSoftDelete { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            IConfigurationRoot configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();

            if (configuration != null)
            {
                _configuration = configuration;

                _IsSoftDelete = _configuration.GetSection("AppParams")["IsSoftDelete"].ToString() == "true" ? true : false;
            }


            //optionsBuilder.UseSqlServer(@"Server=DBSRV;Database=TsiErpYeni;UID=sa;PWD=Logo1234567890;");
            optionsBuilder.UseSqlServer(_configuration.GetConnectionString("AppConnectionString"));
        }

        public TsiErpDbContext()
        {
        }

        public TsiErpDbContext(DbContextOptions<TsiErpDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            if (_IsSoftDelete)
            {
                foreach (var entityType in builder.Model.GetEntityTypes())
                {
                    if (typeof(IFullEntityObject).IsAssignableFrom(entityType.ClrType))
                    {
                        entityType.AddSoftDeleteQueryFilter();
                    }
                }
            }

            /* Configure your own tables/entities inside here */


            builder.ConfigureBranches();
            builder.ConfigurePeriods();
        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            if (_IsSoftDelete)
            {
                ChangeTracker.SetAuditProperties();
            }
            return await base.SaveChangesAsync(cancellationToken);
        }

        public override int SaveChanges()
        {
            if (_IsSoftDelete)
            {
                ChangeTracker.SetAuditProperties();
            }
            return base.SaveChanges();
        }

        public override int SaveChanges(bool acceptAllChangesOnSuccess)
        {
            if (_IsSoftDelete)
            {
                ChangeTracker.SetAuditProperties();
            }
            return base.SaveChanges(acceptAllChangesOnSuccess);
        }

        public override async Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default)
        {
            if (_IsSoftDelete)
            {
                ChangeTracker.SetAuditProperties();
            }
            return await base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
        }

        #region DbSets
        public DbSet<Branches> Branches { get; set; }
        public DbSet<Periods> Periods { get; set; }
        #endregion
    }
}
