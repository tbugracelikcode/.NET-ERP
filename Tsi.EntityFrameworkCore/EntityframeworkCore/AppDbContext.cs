using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tsi.Core.Entities.Auditing;
using Tsi.EntityFrameworkCore.Extensions;
using Tsi.EntityFrameworkCore.Modeling;

namespace Tsi.EntityFrameworkCore.EntityframeworkCore
{
    public abstract class AppDbContext<TDbContext> : DbContext
        where TDbContext : DbContext
    {
        public IConfigurationRoot _configuration;

        public virtual bool _IsSoftDelete { get; set; }

        public virtual string BasePath { get; set; }

        public virtual string JsonFile { get; set; }
        
        public virtual string SoftDeleteSectionName { get; set; }

        public virtual string SoftDeleteKey { get; set; }

        public virtual string ConnectionStringKey { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            IConfigurationRoot configuration = new ConfigurationBuilder()
                .SetBasePath(BasePath)
                .AddJsonFile(JsonFile)
                .Build();

            if (configuration != null)
            {
                _configuration = configuration;

                _IsSoftDelete = _configuration.GetSection(SoftDeleteSectionName)[SoftDeleteKey].ToString() == "true" ? true : false;
            }


            optionsBuilder.UseSqlServer(_configuration.GetConnectionString(ConnectionStringKey));
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
        }

        public AppDbContext()
        {
        }

        public AppDbContext(DbContextOptions<TDbContext> options) : base(options)
        {
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
    }
}
