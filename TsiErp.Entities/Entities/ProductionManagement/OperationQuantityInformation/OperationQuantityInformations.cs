using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tsi.Core.Entities;
using Tsi.Core.Utilities.SqlDataTypeMappingUtilities;
using TsiErp.Entities.Enums;

namespace TsiErp.Entities.Entities.ProductionManagement.OperationQuantityInformation
{
    public class OperationQuantityInformations : IEntity
    {
        [SqlColumnType(Nullable = false, SqlDbType = SqlDataType.UniqueIdentifier)]
        /// <summary>
        /// Id
        /// </summary>
        public Guid Id { get; set; }
        [SqlColumnType(Nullable = false, SqlDbType = SqlDataType.UniqueIdentifier)]
        /// <summary>
        /// İş Emri ID
        /// </summary>
        public Guid WorkOrderID { get; set; }
        [SqlColumnType(Nullable = false, SqlDbType = SqlDataType.UniqueIdentifier)]
        /// <summary>
        /// Üretim Emri ID
        /// </summary>
        public Guid ProductionOrderID { get; set; }
        [SqlColumnType(Nullable = false, SqlDbType = SqlDataType.UniqueIdentifier)]
        /// <summary>
        /// Operatör ID
        /// </summary>
        public Guid OperatorID { get; set; }
        [SqlColumnType(SqlDbType = SqlDataType.Decimal, Precision = 6, Scale = 18)]
        /// <summary>
        /// Bağlama Süresi
        /// </summary>
        public decimal AttachmentTime { get; set; }
        [SqlColumnType(SqlDbType = SqlDataType.Decimal, Precision = 6, Scale = 18)]
        /// <summary>
        /// Operasyon Süresi
        /// </summary>
        public decimal OperationTime { get; set; }
        [SqlColumnType(Nullable = false, SqlDbType = SqlDataType.UniqueIdentifier)]
        /// <summary>
        /// Üretim Takip ID
        /// </summary>
        public Guid ProductionTrackingID { get; set; }
        [SqlColumnType(Nullable = false, SqlDbType = SqlDataType.UniqueIdentifier)]
        /// <summary>
        /// İstasyon ID
        /// </summary>
        public Guid StationID { get; set; }
        [SqlColumnType(Nullable = false, SqlDbType = SqlDataType.UniqueIdentifier)]
        /// <summary>
        /// Operasyon ID
        /// </summary>
        public Guid ProductsOperationID { get; set; }
        [SqlColumnType(Nullable = false, SqlDbType = SqlDataType.Int)]
        /// <summary>
        /// Tür
        /// </summary>
        public OperationQuantityInformationsTypeEnum OperationQuantityInformationsType { get; set; }
        [SqlColumnType(Nullable = false, SqlDbType = SqlDataType.DateTime)]
        /// <summary>
        /// Tarih
        /// </summary>
        public DateTime Date_ { get; set; }
        [SqlColumnType(Nullable = false, SqlDbType = SqlDataType.Time)]
        /// <summary>
        /// Saat
        /// </summary>
        public TimeSpan Hour_ { get; set; }
    }
}
