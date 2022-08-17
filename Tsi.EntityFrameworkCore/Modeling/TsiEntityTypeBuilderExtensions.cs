using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Tsi.Core.Entities;
using Tsi.Core.Entities.CreationEntites;
using Tsi.Core.Entities.DeleterEntities;
using Tsi.Core.Entities.ModifierEntities;

namespace Tsi.EntityFrameworkCore.Modeling
{
    public static class TsiEntityTypeBuilderExtensions
    {

        public static void ConfigureByConvention(this EntityTypeBuilder b)
        {
            b.TryConfigureEntityId();
            b.TryConfigureCreationEntity();
            b.TryConfigureCreationTime();
            b.TryConfigureModificationAudited();
            b.TryConfigureLastModificationTime();
            b.TryConfigureDeletionEntity();
            b.TryConfigureDeletionTime();
            //b.TryConfigureSoftDelete();
        }

        //public static void ConfigureLogs(this ModelBuilder builder)
        //{
        //    builder.Entity<Logs>(b =>
        //    {
        //        b.ToTable("Logs");


        //        b.Property(t => t.Id).IsRequired().HasColumnType(SqlDbType.UniqueIdentifier.ToString());
        //        b.Property(t => t.Date_).IsRequired().HasColumnType(SqlDbType.DateTime.ToString()).HasMaxLength(200);
        //        b.Property(t => t.MethodName).IsRequired().HasColumnType("nvarchar(MAX)");
        //        b.Property(t => t.BeforeValues).IsRequired().HasColumnType("nvarchar(MAX)");
        //        b.Property(t => t.AfterValues).IsRequired().HasColumnType("nvarchar(MAX)");
        //        b.Property(t => t.LogLevel).IsRequired().HasColumnType(SqlDbType.NVarChar.ToString()).HasMaxLength(50);
        //        b.Property(t => t.UserId).IsRequired().HasColumnType(SqlDbType.UniqueIdentifier.ToString());

        //        b.HasIndex(x => x.MethodName);
        //        b.HasIndex(x => x.LogLevel);
        //        b.HasIndex(x => x.UserId);

        //    });
        //}

        public static void TryConfigureEntityId(this EntityTypeBuilder b)
        {

            b.Property(nameof(IEntity.Id))
                .IsRequired()
                .HasColumnName(nameof(IEntity.Id));

        }

        public static void TryConfigureCreationEntity(this EntityTypeBuilder b)
        {

            b.Property(nameof(ICreationEntity.CreatorId))
                .IsRequired()
                .HasColumnName(nameof(ICreationEntity.CreatorId));

        }

        public static void TryConfigureCreationTime(this EntityTypeBuilder b)
        {

            b.Property(nameof(ICreationTime.CreationTime))
                .IsRequired()
                .HasColumnName(nameof(ICreationTime.CreationTime));

        }

        public static void TryConfigureModificationAudited(this EntityTypeBuilder b)
        {

            b.Property(nameof(IModificationEntity.LastModifierId))
                .IsRequired(false)
                .HasColumnName(nameof(IModificationEntity.LastModifierId));

        }

        public static void TryConfigureLastModificationTime(this EntityTypeBuilder b)
        {

            b.Property(nameof(IModificationTime.LastModificationTime))
                .IsRequired(false)
                .HasColumnName(nameof(IModificationTime.LastModificationTime));

        }

        public static void TryConfigureDeletionEntity(this EntityTypeBuilder b)
        {

            b.Property(nameof(IDeletionEntity.DeleterId))
                .IsRequired(false)
                .HasColumnName(nameof(IDeletionEntity.DeleterId));

        }

        public static void TryConfigureSoftDelete(this EntityTypeBuilder b)
        {

            b.Property(nameof(ISoftDelete.IsDeleted))
                .IsRequired()
                .HasDefaultValue(false)
                .HasColumnName(nameof(ISoftDelete.IsDeleted));

        }

        public static void TryConfigureDeletionTime(this EntityTypeBuilder b)
        {

            b.Property(nameof(IDeletionTime.DeletionTime))
                .IsRequired(false)
                .HasColumnName(nameof(IDeletionTime.DeletionTime));

        }
    }
}
