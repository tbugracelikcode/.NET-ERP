using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tsi.Core.Entities.Auditing;

namespace TsiErp.Entities.Entities.GeneralSystemIdentifications.User.Dtos
{
    public class CreateUsersDto : FullAuditedEntityDto
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
        public Guid? GroupID { get; set; }
    }
}
