using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tsi.Authentication.Entities.Menus;
using Tsi.Authentication.Entities.RolePermissions;
using Tsi.Authentication.Entities.Roles;
using Tsi.Authentication.Entities.UserRoles;
using Tsi.Authentication.Entities.Users;
using Tsi.EntityFrameworkCore.Modeling;
using TsiErp.Entities.Entities.Branch;
using TsiErp.Entities.Entities.Logging;
using TsiErp.Entities.Entities.Period;

namespace TsiErp.DataAccess.EntityFrameworkCore.Configurations
{
    public static class TsiErpDbContextModelBuilderExtensions
    {
        public static void ConfigureUsers(this ModelBuilder builder)
        {
            builder.Entity<TsiUser>(b =>
            {
                b.ToTable("TsiUser");
                b.ConfigureByConvention();

                b.Property(t => t.UserName).IsRequired().HasColumnType(SqlDbType.NVarChar.ToString()).HasMaxLength(250);
                b.Property(t => t.Name).IsRequired().HasColumnType(SqlDbType.NVarChar.ToString()).HasMaxLength(250);
                b.Property(t => t.Surname).IsRequired().HasColumnType(SqlDbType.NVarChar.ToString()).HasMaxLength(250);
                b.Property(t => t.Email).IsRequired().HasColumnType(SqlDbType.NVarChar.ToString()).HasMaxLength(250);
                b.Property(t => t.EmailConfirmed).HasColumnType(SqlDbType.Bit.ToString());
                b.Property(t => t.PasswordHash).IsRequired().HasColumnType("nvarchar(max)");
                b.Property(t => t.IsActive).HasColumnType(SqlDbType.Bit.ToString());
                b.Property(t => t.PhoneNumber).IsRequired().HasColumnType(SqlDbType.NVarChar.ToString()).HasMaxLength(95);

            });
        }

        public static void ConfigureUserRoles(this ModelBuilder builder)
        {
            builder.Entity<TsiUserRoles>(b =>
            {
                b.ToTable("TsiUserRoles");
                b.ConfigureByConvention();

                b.Property(t => t.UserId).IsRequired().HasColumnType(SqlDbType.UniqueIdentifier.ToString());
                b.Property(t => t.RoleId).IsRequired().HasColumnType(SqlDbType.UniqueIdentifier.ToString());
            });
        }

        public static void ConfigureRolePermissions(this ModelBuilder builder)
        {
            builder.Entity<TsiRolePermissions>(b =>
            {
                b.ToTable("TsiRolePermissions");
                b.ConfigureByConvention();

                b.Property(t => t.RoleId).IsRequired().HasColumnType(SqlDbType.UniqueIdentifier.ToString());
                b.Property(t => t.MenuId).IsRequired().HasColumnType(SqlDbType.UniqueIdentifier.ToString());

                

                b.HasOne(x=>x.TsiMenus).WithMany(x=>x.TsiRolePermissions).HasForeignKey(x=>x.MenuId);
                b.HasOne(x=>x.TsiRoles).WithMany(x=>x.TsiRolePermissions).HasForeignKey(x=>x.RoleId);
            });
        }

        public static void ConfigureRoles(this ModelBuilder builder)
        {
            builder.Entity<TsiRoles>(b =>
            {
                b.ToTable("TsiRoles");
                b.ConfigureByConvention();

                b.Property(t => t.RoleName).IsRequired().HasColumnType(SqlDbType.NVarChar.ToString()).HasMaxLength(250);

            });
        }

        public static void ConfigureMenus(this ModelBuilder builder)
        {
            builder.Entity<TsiMenus>(b =>
            {
                b.ToTable("TsiMenus");

                b.Property(t => t.Id).IsRequired().HasColumnType(SqlDbType.UniqueIdentifier.ToString());
                b.Property(t => t.ParentMenutId).HasColumnType(SqlDbType.UniqueIdentifier.ToString());
                b.Property(t => t.MenuName).IsRequired().HasColumnType(SqlDbType.NVarChar.ToString()).HasMaxLength(250);
            });
        }

        public static void ConfigureBranches(this ModelBuilder builder)
        {
            builder.Entity<Branches>(b =>
            {
                b.ToTable("Branches");
                b.ConfigureByConvention();

                //b.HasQueryFilter(x => !x.IsDeleted);

                b.Property(t => t.Code).IsRequired().HasColumnType(SqlDbType.NVarChar.ToString()).HasMaxLength(17);
                b.Property(t => t.Name).IsRequired().HasColumnType(SqlDbType.NVarChar.ToString()).HasMaxLength(200);
                b.Property(t => t.Description_).HasColumnType("nvarchar(MAX)");
                b.Property(t => t.IsActive).HasColumnType(SqlDbType.Bit.ToString());

                b.HasIndex(x => x.Code);

            });
        }

        public static void ConfigurePeriods(this ModelBuilder builder)
        {
            builder.Entity<Periods>(b =>
            {
                b.ToTable("Periods");
                b.ConfigureByConvention();

                b.Property(t => t.Code).IsRequired().HasColumnType(SqlDbType.NVarChar.ToString()).HasMaxLength(17);
                b.Property(t => t.Name).IsRequired().HasColumnType(SqlDbType.NVarChar.ToString()).HasMaxLength(200);
                b.Property(t => t.Description_).HasColumnType("nvarchar(MAX)");
                b.Property(t => t.IsActive).HasColumnType(SqlDbType.Bit.ToString());
                b.Property(t => t.BranchID).HasColumnType(SqlDbType.UniqueIdentifier.ToString());

                b.HasIndex(x => x.Code);

                b.HasOne(x => x.Branches).WithMany(x => x.Periods).HasForeignKey(x => x.BranchID).OnDelete(DeleteBehavior.NoAction);

            });
        }

        public static void ConfigureLogs(this ModelBuilder builder)
        {
            builder.Entity<Logs>(b =>
            {
                b.ToTable("Logs");


                b.Property(t => t.Id).IsRequired().HasColumnType(SqlDbType.UniqueIdentifier.ToString());
                b.Property(t => t.Date_).IsRequired().HasColumnType(SqlDbType.DateTime.ToString()).HasMaxLength(200);
                b.Property(t => t.MethodName_).IsRequired().HasColumnType("nvarchar(MAX)");
                b.Property(t => t.BeforeValues).IsRequired().HasColumnType("sql_variant").HasMaxLength(5000);
                b.Property(t => t.AfterValues).IsRequired().HasColumnType("sql_variant").HasMaxLength(5000);
                b.Property(t => t.LogLevel_).IsRequired().HasColumnType(SqlDbType.NVarChar.ToString()).HasMaxLength(50);
                b.Property(t => t.UserId).IsRequired().HasColumnType(SqlDbType.UniqueIdentifier.ToString());

            });
        }

    }
}
