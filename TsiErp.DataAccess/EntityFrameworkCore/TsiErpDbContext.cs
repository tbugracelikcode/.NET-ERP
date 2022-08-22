using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
        }



        #region DbSets
        public DbSet<Branches> Branches { get; set; }
        public DbSet<Periods> Periods { get; set; }
        #endregion
    }
}
