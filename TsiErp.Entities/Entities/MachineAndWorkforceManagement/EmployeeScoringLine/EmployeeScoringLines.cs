using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tsi.Core.Entities.Auditing;
using Tsi.Core.Utilities.SqlDataTypeMappingUtilities;

namespace TsiErp.Entities.Entities.MachineAndWorkforceManagement.EmployeeScoringLine
{
    public class EmployeeScoringLines : FullAuditedEntity
    {
        [SqlColumnType( Nullable = false, SqlDbType = SqlDataType.UniqueIdentifier)]
        /// <summary>
        /// Puantaj ID
        /// </summary>
        public Guid EmployeeScoringID { get; set; }
        [SqlColumnType(Nullable = false, SqlDbType = SqlDataType.Int)]
        /// <summary>
        /// Satır No
        /// </summary>
        public int LineNr { get; set; }
        [SqlColumnType(Nullable = false, SqlDbType = SqlDataType.UniqueIdentifier)]
        /// <summary>
        /// Personel ID
        /// </summary>
        public Guid EmployeeID { get; set; }
        [SqlColumnType(Nullable = false, SqlDbType = SqlDataType.UniqueIdentifier)]
        /// <summary>
        /// Departman ID
        /// </summary>
        public Guid DepartmentID { get; set; }
        [SqlColumnType(Nullable = false, SqlDbType = SqlDataType.UniqueIdentifier)]
        /// <summary>
        /// Bağlı Olduğu Kıdem ID
        /// </summary>
        public Guid OfficialSeniorityID { get; set; }
        [SqlColumnType(Nullable = false, SqlDbType = SqlDataType.UniqueIdentifier)]
        /// <summary>
        /// Kıdem ID
        /// </summary>
        public Guid SeniorityID { get; set; }
        [SqlColumnType(Nullable = false, SqlDbType = SqlDataType.DateTime)]
        /// <summary>
        /// ,Bugünün Tarihi
        /// </summary>
        public DateTime TodaysDate { get; set; }
        [SqlColumnType(SqlDbType = SqlDataType.Decimal, Precision = 6, Scale = 18)]
        /// <summary>
        /// Kıdem Değeri
        /// </summary>
        public decimal SeniorityValue { get; set; }
        [SqlColumnType(Nullable = false, SqlDbType = SqlDataType.UniqueIdentifier)]
        /// <summary>
        /// Eğitim Seviyesi ID
        /// </summary>
        public Guid EducationLevelID { get; set; }
        [SqlColumnType(SqlDbType = SqlDataType.Decimal, Precision = 6, Scale = 18)]
        /// <summary>
        /// Devamsızlık Süresi
        /// </summary>
        public decimal AbsencePeriod { get; set; }
        [SqlColumnType(Nullable = true, SqlDbType = SqlDataType.Int)]
        /// <summary>
        /// Vardiya Değeri
        /// </summary>
        public int ShiftValue { get; set; }
        [SqlColumnType(SqlDbType = SqlDataType.Decimal, Precision = 6, Scale = 18)]
        /// <summary>
        /// Görev Yetkinliği Puanı
        /// </summary>
        public decimal TaskCompetenceScore { get; set; }
        [SqlColumnType(SqlDbType = SqlDataType.Decimal, Precision = 6, Scale = 18)]
        /// <summary>
        /// Görev Kabiliyet Oranı
        /// </summary>
        public decimal TaskCapabilityRatio { get; set; }
        [SqlColumnType(SqlDbType = SqlDataType.Decimal, Precision = 6, Scale = 18)]
        /// <summary>
        /// İşe Devam Oranı
        /// </summary>
        public decimal AttendanceRatio { get; set; }
        [SqlColumnType(SqlDbType = SqlDataType.Decimal, Precision = 6, Scale = 18)]
        /// <summary>
        /// Genel Beceri Oranı
        /// </summary>
        public decimal GeneralSkillRatio { get; set; }
        [SqlColumnType(SqlDbType = SqlDataType.Decimal, Precision = 6, Scale = 18)]
        /// <summary>
        /// Yönetim İyileştirme Oranı
        /// </summary>
        public decimal ManagementImprovementRatio { get; set; }
        [SqlColumnType(SqlDbType = SqlDataType.Decimal, Precision = 6, Scale = 18)]
        /// <summary>
        /// Pozisyonun Başlangıç Maaşı
        /// </summary>
        public decimal StartingSalaryofPosition { get; set; }
        [SqlColumnType(SqlDbType = SqlDataType.Decimal, Precision = 6, Scale = 18)]
        /// <summary>
        /// Değerlendirme Sonrası Maaş
        /// </summary>
        public decimal AfterEvaluationSalary { get; set; }
        [SqlColumnType(SqlDbType = SqlDataType.Decimal, Precision = 6, Scale = 18)]
        /// <summary>
        /// Yeniden Değerlendirme Oranı
        /// </summary>
        public decimal ReevaluationRatio { get; set; }
        [SqlColumnType(SqlDbType = SqlDataType.Decimal, Precision = 6, Scale = 18)]
        /// <summary>
        /// Pozisyonun Değerleme Zam Oranı
        /// </summary>
        public decimal PositionValuationRaiseRatio { get; set; }
        [SqlColumnType(Nullable = true, SqlDbType = SqlDataType.Int)]
        /// <summary>
        /// Zam Ayı
        /// </summary>
        public int RaiseMonth { get; set; }
    }
}
