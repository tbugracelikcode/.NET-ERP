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
using TsiErp.DataAccess.EntityFrameworkCore.Configurations;
using TsiErp.Entities.Entities.Branch;
using TsiErp.Entities.Entities.Period;

namespace TsiErp.DataAccess.EntityFrameworkCore
{
    public class TsiErpDbContext : AppDbContext<TsiErpDbContext>
    {
        public TsiErpDbContext()
        {
            base.BasePath = Directory.GetCurrentDirectory();
            base.JsonFile = "appsettings.json";
            base.SoftDeleteSectionName = "AppParams";
            base.SoftDeleteKey = "IsSoftDelete";
            base.ConnectionStringKey = "AppConnectionString";
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.ConfigureBranches();
            builder.ConfigurePeriods();
            builder.ConfigureUsers();
            builder.ConfigureUserRoles();
            builder.ConfigureRolePermissions();
            builder.ConfigureRoles();
            builder.ConfigureMenus();
        }

        public override int SaveChanges()
        {
            return base.SaveChanges();
        }

        public override int SaveChanges(bool acceptAllChangesOnSuccess)
        {
            return base.SaveChanges(acceptAllChangesOnSuccess);
        }

        public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default)
        {
            return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            return base.SaveChangesAsync(cancellationToken);
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
        #endregion
    }
}
