using Tsi.Core.Entities.Auditing;
using TSI.QueryBuilder.MappingAttributes;
using TsiErp.Entities.Entities.MachineAndWorkforceManagement.EmployeeOperation.Dtos;

namespace TsiErp.Entities.Entities.MachineAndWorkforceManagement.EmployeeScoringLine.Dtos
{
    public class SelectEmployeeScoringLinesDto : FullAuditedEntityDto
    {
        /// <summary>
        /// Puantaj ID
        /// </summary>
        public Guid EmployeeScoringID { get; set; }
        /// <summary>
        /// Satır No
        /// </summary>
        public int LineNr { get; set; }
        /// <summary>
        /// Personel ID
        /// </summary>
        public Guid EmployeeID { get; set; }
        /// <summary>
        /// Personel Adı
        /// </summary>
        public string EmployeeName { get; set; }
        /// <summary>
        /// Personel Soyadı
        /// </summary>
        public string EmployeeSurname { get; set; }
        /// <summary>
        /// Personel İşe Giriş Tarihi
        /// </summary>
        public DateTime EmployeeHiringDate { get; set; }
        /// <summary>
        /// Personel Mevcut Maaş
        /// </summary>
        public decimal EmployeeCurrentSalary { get; set; }
        /// <summary>
        /// Personel Görev Tanımı
        /// </summary>
        public string EmployeeTaskDefinition { get; set; }
        /// <summary>
        /// Departman ID
        /// </summary>
        public Guid DepartmentID { get; set; }
        /// <summary>
        /// Departman Adı
        /// </summary>
        public string DepartmentName { get; set; }
        /// <summary>
        /// Bağlı Olduğu Kıdem ID
        /// </summary>
        public Guid OfficialSeniorityID { get; set; }
        /// <summary>
        /// Bağlı Olduğu Kıdem Adı
        /// </summary>
        public string OfficialSeniorityName { get; set; }
        /// <summary>
        /// Kıdem ID
        /// </summary>
        public Guid SeniorityID { get; set; }
        /// <summary>
        /// Kıdem Adı
        /// </summary>
        public string SeniorityName { get; set; }
        /// <summary>
        /// Pozisyonun Başlangıç Maaşı
        /// </summary>
        public decimal StartingSalaryofPosition { get; set; }
        /// <summary>
        /// ,Bugünün Tarihi
        /// </summary>
        public DateTime TodaysDate { get; set; }
        /// <summary>
        /// Kıdem Değeri
        /// </summary>
        public decimal SeniorityValue { get; set; }
        /// <summary>
        /// Eğitim Seviyesi ID
        /// </summary>
        public Guid EducationLevelID { get; set; }
        /// <summary>
        /// Eğitim Seviyesi Adı
        /// </summary>
        public string EducationLevelName { get; set; }
        /// <summary>
        /// Eğitim Seviyesi Oranı
        /// </summary>
        public decimal EducationLevelScore { get; set; }
        /// <summary>
        /// Devamsızlık Süresi
        /// </summary>
        public decimal AbsencePeriod { get; set; }
        /// <summary>
        /// Vardiya Değeri
        /// </summary>
        public int ShiftValue { get; set; }
        /// <summary>
        /// Görev Yetkinliği Puanı
        /// </summary>
        public decimal TaskCompetenceScore { get; set; }
        /// <summary>
        /// Görev Kabiliyet Oranı
        /// </summary>
        public decimal TaskCapabilityRatio { get; set; }
        /// <summary>
        /// İşe Devam Oranı
        /// </summary>
        public decimal AttendanceRatio { get; set; }
        /// <summary>
        /// Genel Beceri Oranı
        /// </summary>
        public decimal GeneralSkillRatio { get; set; }
        /// <summary>
        /// Yönetim İyileştirme Oranı
        /// </summary>
        public decimal ManagementImprovementRatio { get; set; }
        /// <summary>
        /// Değerlendirme Sonrası Maaş
        /// </summary>
        public decimal AfterEvaluationSalary { get; set; }
        /// <summary>
        /// Yeniden Değerlendirme Oranı
        /// </summary>
        public decimal ReevaluationRatio { get; set; }
        /// <summary>
        /// Pozisyonun Değerleme Zam Oranı
        /// </summary>
        public decimal PositionValuationRaiseRatio { get; set; }
        /// <summary>
        /// Zam Ayı
        /// </summary>
        public int RaiseMonth { get; set; }

        [NoDatabaseAction]
        public List<SelectEmployeeOperationsDto> SelectEmployeeOperations { get; set; }
    }
}
