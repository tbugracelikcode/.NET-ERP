using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tsi.Core.Utilities.SqlDataTypeMappingUtilities;

namespace TsiErp.Entities.Entities.ProductionManagement.OperationStockMovement.Dtos
{
    public class CreateOperationStockMovementsDto
    {
        /// <summary>
        /// Id
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        ///  Operasyon ID
        /// </summary>
        public Guid OperationID { get; set; }

        /// <summary>
        ///  Üretim Emri ID
        /// </summary>
        public Guid ProductionorderID { get; set; }

        /// <summary>
        ///  Toplam Miktar
        /// </summary>
        public decimal TotalAmount { get; set; }
    }
}
