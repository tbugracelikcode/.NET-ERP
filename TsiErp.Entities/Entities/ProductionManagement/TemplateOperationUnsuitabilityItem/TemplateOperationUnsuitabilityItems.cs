using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tsi.Core.Entities.Auditing;
using Tsi.Core.Utilities.SqlDataTypeMappingUtilities;

namespace TsiErp.Entities.Entities.ProductionManagement.TemplateOperationUnsuitabilityItem
{
    /// <summary>
    /// Şablon Operasyon Uygunsuzluk Başlıkları
    /// </summary>
    public class TemplateOperationUnsuitabilityItems : FullAuditedEntity
    {

        [SqlColumnType(Nullable = true, SqlDbType = SqlDataType.UniqueIdentifier)]
        ///<summary>
        ///Şablon Operasyon ID
        /// </summary
        public Guid TemplateOperationId { get; set; }

        [SqlColumnType(Nullable = true, SqlDbType = SqlDataType.UniqueIdentifier)]
        ///<summary>
        ///Uygunsuzluk Başlığı ID
        /// </summary
        public Guid UnsuitabilityItemsId { get; set; }

        [SqlColumnType(Nullable = false, SqlDbType = SqlDataType.Bit)]
        /// <summary>
        /// Kullanılacak
        /// </summary>
        public bool ToBeUsed { get; set; }

        [SqlColumnType(Nullable = false, SqlDbType = SqlDataType.Int)]
        /// <summary>
        /// Satır Numarası
        /// </summary>
        public int LineNr { get; set; }
    }
}
