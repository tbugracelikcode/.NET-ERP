using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tsi.Core.Entities;
using Tsi.Core.Utilities.SqlDataTypeMappingUtilities;

namespace TsiErp.Entities.Entities.Other.Notification
{
    public class Notifications : IEntity
    {

        [SqlColumnType(Nullable = false, SqlDbType = SqlDataType.UniqueIdentifier)]

        /// <summary>
        /// ID
        /// </summary>
        public Guid Id { get; set; }

        [SqlColumnType(Nullable = false, SqlDbType = SqlDataType.UniqueIdentifier)]

        /// <summary>
        /// Kullanıcı ID
        /// </summary>
        public Guid UserId { get; set; }

        [SqlColumnType(Nullable = false, SqlDbType = SqlDataType.VarCharMax)]
        /// <summary>
        /// Mesaj
        /// </summary>
        public string Message_ { get; set; }

        [SqlColumnType(Nullable = true, SqlDbType = SqlDataType.DateTime)]
        /// <summary>
        /// Bildirim Tarihi
        /// </summary>
        public DateTime? NotificationDate { get; set; }

        [SqlColumnType(Nullable = false, SqlDbType = SqlDataType.Bit)]
        /// <summary>
        /// Görünürlük
        /// </summary>
        public bool IsViewed { get; set; }

        [SqlColumnType(Nullable = true, SqlDbType = SqlDataType.DateTime)]
        /// <summary>
        /// Görülme Tarihi
        /// </summary>
        public DateTime? ViewDate { get; set; }

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

        [SqlColumnType(MaxLength = 50, Nullable = false, SqlDbType = SqlDataType.NVarChar)]

        /// <summary>
        /// Kayıt No
        /// </summary>
        public string RecordNumber { get; set; }


    }
}
