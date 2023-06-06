using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tsi.Core.Entities.Auditing;
using TsiErp.Entities.Entities.UserGroup;
using Tsi.Core.Utilities.SqlDataTypeMappingUtilities;
using SqlDataType = Tsi.Core.Utilities.SqlDataTypeMappingUtilities.SqlDataType;

namespace TsiErp.Entities.Entities.User
{
    /// <summary>
    /// Kullanıcılar
    /// </summary>
    public class Users : FullAuditedEntity
    {
        /// <summary>
        /// Kullanıcı Kodu
        /// </summary>
        public string Code { get; set; }
        /// <summary>
        /// Kullanıcı Adı
        /// </summary>
        public string UserName { get; set; }
        /// <summary>
        ///  Adı Soyadı
        /// </summary>
        public string NameSurname { get; set; }
        /// <summary>
        ///  E-Posta
        /// </summary>
        public string Email { get; set; }
        /// <summary>
        /// E-posta Onay
        /// </summary>
        public bool IsEmailApproved { get; set; }
        /// <summary>
        /// Aktiflik
        /// </summary>
        public bool IsActive { get; set; }
        /// <summary>
        ///  Parola
        /// </summary>
        public string Password { get; set; }
        /// <summary>
        /// Grup ID
        /// </summary>
        public Guid GroupID { get; set; }
    }
}
