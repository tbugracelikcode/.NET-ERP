using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TsiErp.Entities.Entities.ProductionManagement.OperationStockMovement.Dtos
{
    public class SelectOperationStockMovementsDto
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
        /// Operasyon Kodu
        /// </summary>
        public string OperationCode { get; set; }

        /// <summary>
        /// Operasyon Adı
        /// </summary>
        public string OperationName { get; set; }

        /// <summary>
        ///  İş Emri ID
        /// </summary>
        public Guid WorkorderID { get; set; }

        /// <summary>
        /// İş Emri No
        /// </summary>
        public string WorkorderFicheNo { get; set; }

        /// <summary>
        ///  Üretim Emri ID
        /// </summary>
        public Guid ProductionorderID { get; set; }

        /// <summary>
        /// Üretim Emri No
        /// </summary>
        public string ProductionorderCode { get; set; }

        /// <summary>
        ///  Sipariş ID
        /// </summary>
        public Guid OrderID { get; set; }

        /// <summary>
        /// Satış Sipariş No
        /// </summary>
        public string OrderFicheNo { get; set; }

        /// <summary>
        ///  Toplam Miktar
        /// </summary>
        public decimal TotalAmount { get; set; }
    }
}
