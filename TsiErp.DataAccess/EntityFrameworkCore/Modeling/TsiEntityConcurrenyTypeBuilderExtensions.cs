using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tsi.Core.Entities.DataConcurrencyEntities;

namespace TsiErp.DataAccess.EntityFrameworkCore.Modeling
{
    public static class TsiEntityConcurrenyTypeBuilderExtensions
    {
        public static void ConfigureByConcurrencyConvention(this EntityTypeBuilder b)
        {
            b.TryConfigureDataConcurrenyDataOpenStatus();
            b.TryConfigureDataConcurrenyDataOpenStatusUserId();
        }

        public static void TryConfigureDataConcurrenyDataOpenStatus(this EntityTypeBuilder b)
        {
            b.Property(nameof(IDataConcurrencyStamp.DataOpenStatus))
                .IsRequired(false)
                .HasDefaultValue(false)
                .HasColumnName(nameof(IDataConcurrencyStamp.DataOpenStatus));
        }

        public static void TryConfigureDataConcurrenyDataOpenStatusUserId(this EntityTypeBuilder b)
        {
            b.Property(nameof(IDataConcurrencyStamp.DataOpenStatusUserId))
                .IsRequired(false)
                .HasDefaultValue(Guid.Empty)
                .HasColumnName(nameof(IDataConcurrencyStamp.DataOpenStatusUserId));
        }
    }
}
