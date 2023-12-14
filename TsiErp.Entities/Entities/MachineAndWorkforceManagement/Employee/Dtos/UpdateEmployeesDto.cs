using System;
using System.Collections.Generic;
using System.Text;
using Tsi.Core.Entities.Auditing;
using TsiErp.Entities.Enums;

namespace TsiErp.Entities.Entities.MachineAndWorkforceManagement.Employee.Dtos
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
        public int BloodType { get; set; }
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
        /// Aktiflik
        /// </summary>
        public bool IsActive { get; set; }

        /// <summary>
        /// Üretim Ekran Kullanıcısı
        /// </summary>
        public bool IsProductionScreenUser { get; set; }
        /// <summary>
        /// Üretim Ekran Kullanıcı Şifresi
        /// </summary>
        public string ProductionScreenPassword { get; set; }
        /// <summary>
        /// Üretim Ekran Ayar Kullanıcısı
        /// </summary>
        public bool IsProductionScreenSettingUser { get; set; }
        /// <summary>
        /// Kıdem ID
        /// </summary>
        public Guid? SeniorityID { get; set; }
        /// <summary>
        /// Eğitim Seviyesi ID
        /// </summary>
        public Guid? EducationLevelID { get; set; }
        /// <summary>
        /// Mevcut Maaş
        /// </summary>
        public decimal CurrentSalary { get; set; }
        /// <summary>
        /// Görev Tanımı
        /// </summary>
        public string TaskDefinition { get; set; }
        /// <summary>
        /// İşe Alınma Tarihi
        /// </summary>
        public DateTime HiringDate { get; set; }
    }
}
