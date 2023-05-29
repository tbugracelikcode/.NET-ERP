using System;
using System.Collections.Generic;
using System.Text;
using Tsi.Core.Entities.CreationEntites;
using Tsi.Core.Entities.DeleterEntities;
using Tsi.Core.Entities.ModifierEntities;
using Tsi.Core.Utilities.SqlDataTypeMappingUtilities;

namespace Tsi.Core.Entities.Auditing
{
    public abstract class FullEntity : IFullEntityObject
    {
        [SqlColumnType(IsPrimaryKey = true, Nullable = false, SqlDbType = SqlDataType.UniqueIdentifier)]
        public Guid Id { get; set; }


        [SqlColumnType(Nullable = true, SqlDbType = SqlDataType.UniqueIdentifier)]
        public virtual Guid? CreatorId { get; set; }


        [SqlColumnType(Nullable = false, SqlDbType = SqlDataType.DateTime)]
        public virtual DateTime? CreationTime { get; set; }


        [SqlColumnType(Nullable = true, SqlDbType = SqlDataType.UniqueIdentifier)]
        public virtual Guid? LastModifierId { get; set; }


        [SqlColumnType(Nullable = true, SqlDbType = SqlDataType.DateTime)]
        public virtual DateTime? LastModificationTime { get; set; }


        [SqlColumnType(Nullable = true, SqlDbType = SqlDataType.UniqueIdentifier)]
        public virtual Guid? DeleterId { get; set; }


        [SqlColumnType(Nullable = true, SqlDbType = SqlDataType.DateTime)]
        public virtual DateTime? DeletionTime { get; set; }


        [SqlColumnType(Nullable = false, SqlDbType = SqlDataType.Bit)]
        public bool IsDeleted { get; set; }


        [SqlColumnType(Nullable = true, SqlDbType = SqlDataType.Bit)]
        public bool? DataOpenStatus { get; set; }


        [SqlColumnType(Nullable = true, SqlDbType = SqlDataType.UniqueIdentifier)]
        public Guid? DataOpenStatusUserId { get; set; }
    }
}
