using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TsiErp.Entities.Entities.PurchaseManagement.PurchaseRequest.ReportDtos.PurchaseRequestListReportDtos
{
    public class PurchaseRequestListReportDto
    {
        public string ProductCode { get; set; }

        public string ProductName { get; set; }

        public string UnitSetCode { get; set; }

        public string ProductTypeName { get; set; }

        public List<PurchaseRequestLines> PurchaseRequestLines { get; set; }

        public PurchaseRequestListReportDto()
        {
            PurchaseRequestLines = new List<PurchaseRequestLines>();
        }
    }

    public class PurchaseRequestLines
    {
        public int LineNr { get; set; }

        public DateTime FicheDate { get; set; }

        public string FicheNumber { get; set; }

        public string CurrentAcountCardCode { get; set; }

        public string CurrentAcountCardName { get; set; }

        public decimal ExchangeRate { get; set; }

        public decimal Quantity { get; set; }

        public decimal UnitPrice { get; set; }

        public decimal DiscountRate { get; set; }

        public decimal DiscountAmount { get; set; }

        public decimal LineAmount { get; set; }

        public decimal LineTotalAmount { get; set; }

        public int VATrate { get; set; }

        public decimal VATamount { get; set; }
    }
}
