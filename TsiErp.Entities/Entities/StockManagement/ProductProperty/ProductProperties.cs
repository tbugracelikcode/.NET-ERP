using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tsi.Core.Entities.Auditing;
using Tsi.Core.Utilities.SqlDataTypeMappingUtilities;

namespace TsiErp.Entities.Entities.StockManagement.ProductProperty
{
    public class ProductProperties : FullAuditedEntity
    {
        [SqlColumnType(Nullable = false, SqlDbType = SqlDataType.UniqueIdentifier)]
        /// <summary>
        /// Ürün Grubu ID
        /// </summary>
        public Guid ProductGroupID { get; set; }

        [SqlColumnType(MaxLength = 17, Nullable = false, SqlDbType = SqlDataType.NVarChar)]
        /// <summary>
        /// Ürün Özellikleri Kodu
        /// </summary>
        public string Code { get; set; }

        [SqlColumnType(MaxLength = 200, Nullable = false, SqlDbType = SqlDataType.NVarChar)]
        /// <summary>
        /// Ürün Özellikleri Açıklaması
        /// </summary>
        public string Name { get; set; }

        [SqlColumnType( Nullable = false, SqlDbType = SqlDataType.Bit)]
        /// <summary>
        /// Ürün Özellikleri Açıklaması
        /// </summary>
        public bool isChooseFromList { get; set; }
    }
}
