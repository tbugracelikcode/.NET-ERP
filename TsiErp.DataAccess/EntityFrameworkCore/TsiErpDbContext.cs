using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tsi.Authentication.Entities.Menus;
using Tsi.Authentication.Entities.RolePermissions;
using Tsi.Authentication.Entities.Roles;
using Tsi.Authentication.Entities.UserRoles;
using Tsi.Authentication.Entities.Users;
using Tsi.Core.Entities.Auditing;
using Tsi.EntityFrameworkCore.EntityframeworkCore;
using Tsi.EntityFrameworkCore.Extensions;
using TsiErp.DataAccess.EntityFrameworkCore.Configurations;
using TsiErp.Entities.Entities.Branch;
using TsiErp.Entities.Entities.Logging;
using TsiErp.Entities.Entities.Period;
using TsiErp.Entities.Entities.UnitSet;

namespace TsiErp.DataAccess.EntityFrameworkCore
{
    public class TsiErpDbContext : DbContext
    {
        public IConfigurationRoot _configuration;

        public virtual bool _IsSoftDelete { get; set; }

        public virtual string BasePath { get; set; }

        public virtual string JsonFile { get; set; }

        public virtual string SoftDeleteSectionName { get; set; }

        public virtual string SoftDeleteKey { get; set; }

        public virtual string ConnectionStringKey { get; set; }

        public TsiErpDbContext()
        {
            this.BasePath = Directory.GetCurrentDirectory();
            this.JsonFile = "appsettings.json";
            this.SoftDeleteSectionName = "AppParams";
            this.SoftDeleteKey = "IsSoftDelete";
            this.ConnectionStringKey = "AppConnectionString";
        }

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

            builder.ConfigureBranches();
            builder.ConfigurePeriods();
            builder.ConfigureUsers();
            builder.ConfigureUserRoles();
            builder.ConfigureRolePermissions();
            builder.ConfigureRoles();
            builder.ConfigureMenus();
            builder.ConfigureLogs();
            builder.ConfigureUnitSets();
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

        #region Authentication DbSets

        public DbSet<TsiUser> Users { get; set; }
        public DbSet<TsiUserRoles> UserRoles { get; set; }
        public DbSet<TsiRoles> Roles { get; set; }
        public DbSet<TsiMenus> Menus { get; set; }
        public DbSet<TsiRolePermissions> RolePermissions { get; set; }

        #endregion


        #region DbSets
        public DbSet<Branches> Branches { get; set; }
        public DbSet<Periods> Periods { get; set; }
        public DbSet<Logs> Logs { get; set; }
        public DbSet<UnitSets> UnitSets { get; set; }
        #endregion
    }
}
