using Tsi.Core.Entities.Auditing;
using TSI.QueryBuilder.MappingAttributes;
using TsiErp.Entities.Entities.MachineAndWorkforceManagement.EmployeeOperation.Dtos;

namespace TsiErp.Entities.Entities.MachineAndWorkforceManagement.EmployeeScoringLine.Dtos
{
    public class CreateEmployeeScoringLinesDto : FullAuditedEntityDto
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
        /// Departman ID
        /// </summary>
        public Guid DepartmentID { get; set; }
        /// <summary>
        /// Bağlı Olduğu Kıdem ID
        /// </summary>
        public Guid OfficialSeniorityID { get; set; }
        /// <summary>
        /// Üretim Performans Oranı
        /// </summary>
        public decimal ProductionPerformanceRatio { get; set; }
        /// <summary>
        /// Kıdem Oranı
        /// </summary>
        public decimal SeniorityRatio { get; set; }
        /// <summary>
        /// Kıdem ID
        /// </summary>
        public Guid SeniorityID { get; set; }
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
        /// Devamsızlık Süresi
        /// </summary>
        public decimal AbsencePeriod { get; set; }
        /// <summary>
        /// Pozisyonun Başlangıç Maaşı
        /// </summary>
        public decimal StartingSalaryofPosition { get; set; }
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
