using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tsi.EntityFrameworkCore.Modeling;
using TsiErp.Entities.Entities.Branch;
using TsiErp.Entities.Entities.Period;

namespace TsiErp.DataAccess.EntityFrameworkCore.Configurations
{
    public static class TsiErpDbContextModelBuilderExtensions
    {
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
    }
}
