using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tsi.Core.Entities.Auditing;
using Tsi.Core.Utilities.SqlDataTypeMappingUtilities;

namespace TsiErp.Entities.Entities.QualityControl.OperationalSPCLine
{
    public class OperationalSPCLines : FullAuditedEntity
    {
        [SqlColumnType(Nullable = false, SqlDbType = SqlDataType.UniqueIdentifier)]
        /// <summary>
        /// SPC ID
        /// </summary>
        public Guid OperationalSPCID { get; set; }
        [SqlColumnType(Nullable = false, SqlDbType = SqlDataType.UniqueIdentifier)]
        /// <summary>
        /// İş Merkezi ID
        /// </summary>
        public Guid WorkCenterID { get; set; }
        [SqlColumnType(Nullable = false, SqlDbType = SqlDataType.UniqueIdentifier)]
        /// <summary>
        /// Operasyon ID
        /// </summary>
        public Guid OperationID { get; set; }
        [SqlColumnType(Nullable = false, SqlDbType = SqlDataType.Int)]
        /// <summary>
        /// Toplam Üretilen Komponent
        /// </summary>
        public int TotalComponent { get; set; }
        [SqlColumnType(Nullable = false, SqlDbType = SqlDataType.Int)]
        /// <summary>
        /// Satır No
        /// </summary>
        public int LineNr { get; set; }
        [SqlColumnType(Nullable = false, SqlDbType = SqlDataType.Int)]
        /// <summary>
        /// Toplam Uygunsuz Komponent
        /// </summary>
        public int TotalUnsuitableComponent { get; set; }
        [SqlColumnType(Nullable = false, SqlDbType = SqlDataType.Int)]
        /// <summary>
        /// Uygunsuz Komponent Oranı
        /// </summary>
        public int UnsuitableComponentRate { get; set; }

        [SqlColumnType(Nullable = false, SqlDbType = SqlDataType.Int)]
        /// <summary>
        /// Toplam Gerçekleşen Operasyon
        /// </summary>
        public int TotalOccuredOperation { get; set; }
        [SqlColumnType(Nullable = false, SqlDbType = SqlDataType.Int)]
        /// <summary>
        /// Toplam Uygunsuz Operasyon
        /// </summary>
        public int TotalUnsuitableOperation { get; set; }
        [SqlColumnType(Nullable = false, SqlDbType = SqlDataType.Int)]
        /// <summary>
        /// Uygunsuz Operasyon Oranı
        /// </summary>
        public int UnsuitableOperationRate { get; set; }
        [SqlColumnType(Nullable = false, SqlDbType = SqlDataType.Int)]
        /// <summary>
        /// Operasyon Başına Uygunsuz Komponent Sayısı
        /// </summary>
        public int UnsuitableComponentPerOperation { get; set; }
        [SqlColumnType(Nullable = false, SqlDbType = SqlDataType.Int)]
        /// <summary>
        /// Şiddet
        /// </summary>
        public int Severity { get; set; }
        [SqlColumnType(Nullable = false, SqlDbType = SqlDataType.Int)]
        /// <summary>
        /// Sıklık
        /// </summary>
        public int Frequency { get; set; }
        [SqlColumnType(Nullable = false, SqlDbType = SqlDataType.Int)]
        /// <summary>
        /// Farkedilebilirlik
        /// </summary>
        public int Detectability { get; set; }
        [SqlColumnType(Nullable = false, SqlDbType = SqlDataType.Int)]
        /// <summary>
        /// RPN
        /// </summary>
        public int RPN { get; set; }
        [SqlColumnType(Nullable = false, SqlDbType = SqlDataType.Int)]
        /// <summary>
        /// Opersayon Bazlı Ara Kontrol Sıklıkları
        /// </summary>
        public int OperationBasedMidControlFrequency { get; set; }
    }
}
