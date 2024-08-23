using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tsi.Core.Entities;
using Tsi.Core.Entities.Auditing;
using Tsi.Core.Utilities.SqlDataTypeMappingUtilities;

namespace TsiErp.Entities.Entities.GeneralSystemIdentifications.NotificationTemplate
{
    public class NotificationTemplates : FullAuditedEntity
    {

        [SqlColumnType(MaxLength = 200, Nullable = true, SqlDbType = SqlDataType.NVarChar)]

        /// <summary>
        /// Ad
        /// </summary>
        public string Name { get; set; }

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

        [SqlColumnType(Nullable = false, SqlDbType = SqlDataType.VarCharMax)]
        /// <summary>
        /// Hedef Kullanıcı Grubu Departman ID
        /// </summary>
        public string TargetDepartmentId { get; set; }

        [SqlColumnType(Nullable = false, SqlDbType = SqlDataType.Bit)]
        /// <summary>
        /// Aktiflik
        /// </summary>
        public bool IsActive { get; set; }

        [SqlColumnType(Nullable = false, SqlDbType = SqlDataType.VarCharMax)]

        /// <summary>
        /// Hedef Kullanıcı Grubu ID
        /// </summary>
        public string TargetUsersId { get; set; }

        [SqlColumnType(Nullable = false, SqlDbType = SqlDataType.VarCharMax)]
        /// <summary>
        /// Mesaj
        /// </summary>
        public string Message_ { get; set; }

    }
}
