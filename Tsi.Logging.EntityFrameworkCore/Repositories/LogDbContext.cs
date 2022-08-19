using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tsi.EntityFrameworkCore.EntityframeworkCore;
using Tsi.Logging.EntityFrameworkCore.Entities;

namespace Tsi.Logging.EntityFrameworkCore.Repositories
{
    public class LogDbContext : AppDbContext
    {


        public LogDbContext()
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

        public DbSet<Logs> Logs { get; set; }
    }
}
