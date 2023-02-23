using System;
using System.Collections.Generic;
using System.Text;
using Tsi.Core.Entities.Auditing;
using TsiErp.Entities.Enums;

namespace TsiErp.Entities.Entities.Employee.Dtos
{
    public class UpdateEmployeesDto : FullAuditedEntityDto
    {
        public string Code { get; set; }
        /// <summary>
        /// İsim
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Soyisim
        /// </summary>
        public string Surname { get; set; }
        /// <summary>
        /// Departman ID
        /// </summary>        
        public Guid? DepartmentID { get; set; }
        /// <summary>
        /// Departman Adı
        /// </summary>
        public string Department { get; set; }
        /// <summary>
        /// TC Kimlik No
        /// </summary>
        public string IDnumber { get; set; }
        /// <summary>
        /// Doğum Günü
        /// </summary>
        public DateTime? Birthday { get; set; }
        /// <summary>
        /// Kan Grubu
        /// </summary>
        public BloodTypeEnum? BloodType { get; set; }
        /// <summary>
        /// Adres
        /// </summary>
        public string Address { get; set; }
        /// <summary>
        /// İlçe
        /// </summary>
        public string District { get; set; }
        /// <summary>
        /// Şehir
        /// </summary>
        public string City { get; set; }
        /// <summary>
        /// Ev Telefonu
        /// </summary>
        public string HomePhone { get; set; }
        /// <summary>
        /// Cep Telefonu
        /// </summary>
        public string CellPhone { get; set; }
        /// <summary>
        /// EPosta
        /// </summary>
        public string Email { get; set; }
        /// <summary>
        /// Resim
        /// </summary>
        public byte[] Image { get; set; }
        /// <summary>
        /// Aktiflik
        /// </summary>
        public bool IsActive { get; set; }
    }
}
