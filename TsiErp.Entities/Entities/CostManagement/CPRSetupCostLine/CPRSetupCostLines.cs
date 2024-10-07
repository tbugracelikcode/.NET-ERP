using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tsi.Core.Entities.Auditing;
using Tsi.Core.Utilities.SqlDataTypeMappingUtilities;
using TsiErp.Entities.Enums;

namespace TsiErp.Entities.Entities.CostManagement.CPRSetupCostLine
{
    public class CPRSetupCostLines : FullAuditedEntity
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
        [SqlColumnType(SqlDbType = SqlDataType.Int)]
        /// <summary>
        /// Üretim Pay Boyutu
        /// </summary>
        public int ManufacturingLotSize { get; set; }
        [SqlColumnType(SqlDbType = SqlDataType.Decimal, Precision = 6, Scale = 18)]
        /// <summary>
        /// Kurulum Süresi
        /// </summary>
        public decimal SetupTime { get; set; }
        [SqlColumnType(SqlDbType = SqlDataType.Decimal, Precision = 6, Scale = 18)]
        /// <summary>
        /// Saatlik Kurulum İşçiliği Ücreti
        /// </summary>
        public decimal SetupLaborHourlyRate { get; set; }
        [SqlColumnType(SqlDbType = SqlDataType.Decimal, Precision = 6, Scale = 18)]
        /// <summary>
        /// Saatlik Çalışma Sistemi Ücreti
        /// </summary>
        public decimal WorkingSystemHourlyRate { get; set; }
        [SqlColumnType(SqlDbType = SqlDataType.Decimal, Precision = 6, Scale = 18)]
        /// <summary>
        /// Kurulum Maliyeti
        /// </summary>
        public decimal SetupCost { get; set; }
        [SqlColumnType(SqlDbType = SqlDataType.Decimal, Precision = 6, Scale = 18)]
        /// <summary>
        /// Kalan Üretim Genel Giderleri
        /// </summary>
        public decimal ResidualManufacturingOverhead { get; set; }
        [SqlColumnType(SqlDbType = SqlDataType.Decimal, Precision = 6, Scale = 18)]
        /// <summary>
        /// Birim Kurulum Maliyeti
        /// </summary>
        public decimal UnitSetupCost { get; set; }
    }
}
