using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tsi.Core.Entities;
using Tsi.Core.Utilities.SqlDataTypeMappingUtilities;

namespace TsiErp.Entities.Entities.GeneralSystemIdentifications.NotificationTemplate
{
    public class NotificationTemplates : IEntity
    {

        [SqlColumnType(Nullable = false, SqlDbType = SqlDataType.UniqueIdentifier)]

        /// <summary>
        /// ID
        /// </summary>
        public Guid Id { get; set; }

        [SqlColumnType(MaxLength = 50, Nullable = false, SqlDbType = SqlDataType.NVarChar)]

        /// <summary>
        /// Modül Adı
        /// </summary>
        public string ModuleName_ { get; set; }

        [SqlColumnType(MaxLength = 50, Nullable = false, SqlDbType = SqlDataType.NVarChar)]

        /// <summary>
        /// İşlem Adı
        /// </summary>
        public string ProcessName_ { get; set; }

        [SqlColumnType(MaxLength = 50, Nullable = false, SqlDbType = SqlDataType.NVarChar)]

        /// <summary>
        /// Context Menü Adı
        /// </summary>
        public string ContextMenuName_ { get; set; }

        [SqlColumnType(Nullable = false, SqlDbType = SqlDataType.UniqueIdentifier)]

        /// <summary>
        /// Kaynak Departman ID
        /// </summary>
        public Guid SourceDepartmentId { get; set; }

        [SqlColumnType(Nullable = false, SqlDbType = SqlDataType.UniqueIdentifier)]
        /// <summary>
        /// Hedef Departman ID
        /// </summary>
        public Guid TargetDepartmentId { get; set; }

        [SqlColumnType(Nullable = false, SqlDbType = SqlDataType.Bit)]
        /// <summary>
        /// Aktiflik
        /// </summary>
        public bool IsActive { get; set; }

        [SqlColumnType(Nullable = false, SqlDbType = SqlDataType.VarCharMax)]

        /// <summary>
        /// str
        /// </summary>
        public string QueryStr { get; set; }

    }
}
