using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tsi.Core.Entities;
using Tsi.Core.Utilities.SqlDataTypeMappingUtilities;

namespace TsiErp.Entities.Entities.GeneralSystemIdentifications.UserPermission
{
    public class UserPermissions : IEntity
    {
        [SqlColumnType(Nullable = false, SqlDbType = SqlDataType.UniqueIdentifier)]
        /// <summary>
        /// Id
        /// </summary>
        public Guid Id { get; set; }

        [SqlColumnType(Nullable = false, SqlDbType = SqlDataType.UniqueIdentifier)]
        /// <summary>
        /// Kullanıcı Id
        /// </summary>
        public Guid UserId { get; set; }

        [SqlColumnType(Nullable = false, SqlDbType = SqlDataType.UniqueIdentifier)]
        /// <summary>
        /// Menu Id
        /// </summary>
        public Guid MenuId { get; set; }

        [SqlColumnType(Nullable = false, SqlDbType = SqlDataType.Bit)]
        /// <summary>
        /// Kullanıcı Yetkisi
        /// </summary>
        public bool IsUserPermitted { get; set; }


    }
}
