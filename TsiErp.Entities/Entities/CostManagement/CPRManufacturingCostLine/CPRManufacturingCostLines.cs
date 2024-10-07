using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tsi.Core.Entities.Auditing;
using Tsi.Core.Utilities.SqlDataTypeMappingUtilities;
using TsiErp.Entities.Enums;

namespace TsiErp.Entities.Entities.CostManagement.CPRManufacturingCostLine
{
    public class CPRManufacturingCostLines : FullAuditedEntity
    {
        [SqlColumnType(Nullable = false, SqlDbType = SqlDataType.UniqueIdentifier)]
        /// <summary>
        /// CPR ID
        /// </summary>
        public Guid CPRID { get; set; }
        [SqlColumnType(SqlDbType = SqlDataType.Int)]
        /// <summary>
        /// Satır No
        /// </summary>
        public int LineNr { get; set; }
        [SqlColumnType(SqlDbType = SqlDataType.Int)]
        /// <summary>
        /// Üretim Adımları
        /// </summary>
        public CPRsManufacturingStepsEnum ManufacturingSteps { get; set; }
        [SqlColumnType(MaxLength = 250, Nullable = true, SqlDbType = SqlDataType.NVarChar)]
        /// <summary>
        /// Malzeme
        /// </summary>
        public string Material_ { get; set; }
        [SqlColumnType(Nullable = false, SqlDbType = SqlDataType.UniqueIdentifier)]
        /// <summary>
        /// İstasyon ID
        /// </summary>
        public Guid StationID { get; set; }
        [SqlColumnType(SqlDbType = SqlDataType.Int)]
        /// <summary>
        /// Net Çıkış / OEE
        /// </summary>
        public int NetOutputDVOEE { get; set; }
        [SqlColumnType(SqlDbType = SqlDataType.Int)]
        /// <summary>
        /// Döngü Başına Parça
        /// </summary>
        public int PartsperCycle { get; set; }
        [SqlColumnType(SqlDbType = SqlDataType.Decimal, Precision = 6, Scale = 18)]
        /// <summary>
        /// Çalışma Sistemi Yatırım
        /// </summary>
        public decimal WorkingSystemInvest { get; set; }
        [SqlColumnType(SqlDbType = SqlDataType.Decimal, Precision = 6, Scale = 18)]
        /// <summary>
        /// Çalışma Sistemi Saatlik Oran
        /// </summary>
        public decimal WorkingSystemHourlyRate { get; set; }
        [SqlColumnType(SqlDbType = SqlDataType.Decimal, Precision = 6, Scale = 18)]
        /// <summary>
        /// Çalışma Sistemi Parça Başına Oran
        /// </summary>
        public decimal WorkingSystemCostperPart { get; set; }
        [SqlColumnType(SqlDbType = SqlDataType.Decimal, Precision = 6, Scale = 18)]
        /// <summary>
        /// Doğrudan İşçilik Saatlik Ücret
        /// </summary>
        public decimal DirectLaborHourlyRate { get; set; }
        [SqlColumnType(SqlDbType = SqlDataType.Int)]
        /// <summary>
        /// Çalışma Sistemindeki Kişi Sayısı
        /// </summary>
        public int HeadCountatWorkingSystem { get; set; }
        [SqlColumnType(SqlDbType = SqlDataType.Decimal, Precision = 6, Scale = 18)]
        /// <summary>
        /// Parça Başına İşçilik Maliyeti
        /// </summary>
        public decimal LaborCostperPart { get; set; }
        [SqlColumnType(SqlDbType = SqlDataType.Decimal, Precision = 6, Scale = 18)]
        /// <summary>
        /// Kalan Üretim Genel Giderleri
        /// </summary>
        public decimal ResidualManufacturingOverhead { get; set; }
        [SqlColumnType(SqlDbType = SqlDataType.Decimal, Precision = 6, Scale = 18)]
        /// <summary>
        /// Hurda Oranı
        /// </summary>
        public decimal ScrapRate { get; set; }
        [SqlColumnType(SqlDbType = SqlDataType.Decimal, Precision = 6, Scale = 18)]
        /// <summary>
        /// Hurda Maliyeti
        /// </summary>
        public decimal ScrapCost { get; set; }
        [SqlColumnType(SqlDbType = SqlDataType.Decimal, Precision = 6, Scale = 18)]
        /// <summary>
        /// Üretim Adımı Maliyeti
        /// </summary>
        public decimal ManufacuringStepCost { get; set; }
    }
}
