using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tsi.Core.Entities.Auditing;

namespace TsiErp.Entities.Entities.StockManagement.StockAddressLine.Dtos
{
    public class ListStockAddressLinesParameterDto : FullAuditedEntityDto
    {
        /// <summary>
        /// Stok Adres ID
        /// </summary>
        public Guid StockAdressID { get; set; }
    }
}
