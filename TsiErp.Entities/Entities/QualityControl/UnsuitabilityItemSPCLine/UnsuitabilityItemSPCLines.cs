using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tsi.Core.Entities.Auditing;
using Tsi.Core.Utilities.SqlDataTypeMappingUtilities;

namespace TsiErp.Entities.Entities.QualityControl.UnsuitabilityItemSPCLine
{
    public class UnsuitabilityItemSPCLines : FullAuditedEntity
    {
        [SqlColumnType( Nullable = false, SqlDbType = SqlDataType.UniqueIdentifier)]
        /// <summary>
        /// SPC ID
        /// </summary>
        public Guid UnsuitabilitySPCID { get; set; }
        [SqlColumnType(Nullable = false, SqlDbType = SqlDataType.UniqueIdentifier)]
        /// <summary>
        /// İş Merkezi ID
        /// </summary>
        public Guid WorkCenterID { get; set; }
        [SqlColumnType(Nullable = false, SqlDbType = SqlDataType.UniqueIdentifier)]
        /// <summary>
        /// Uygunsuzluk Türü ID
        /// </summary>
        public Guid UnsuitabilityTypeID { get; set; }
        [SqlColumnType(Nullable = false, SqlDbType = SqlDataType.UniqueIdentifier)]
        /// <summary>
        /// Uygunsuzluk Başlığı ID
        /// </summary>
        public Guid UnsuitabilityItemID { get; set; }
        [SqlColumnType(Nullable = false, SqlDbType = SqlDataType.Int)]
        /// <summary>
        /// Satır No
        /// </summary>
        public int LineNr { get; set; }
        [SqlColumnType(Nullable = false, SqlDbType = SqlDataType.Int)]
        /// <summary>
        /// Toplam Uygunsuz Komponent Sayısı
        /// </summary>
        public int TotalUnsuitableComponent { get; set; }
        [SqlColumnType(Nullable = false, SqlDbType = SqlDataType.Int)]
        /// <summary>
        /// Toplam Uygunsuz Rapor Sayısı
        /// </summary>
        public int TotalUnsuitableReport { get; set; }
        [SqlColumnType(Nullable = false, SqlDbType = SqlDataType.Int)]
        /// <summary>
        /// Rapor Başına Uygunsuz Komponent 
        /// </summary>
        public int UnsuitableComponentPerReport { get; set; }
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
    }
}
