using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TsiErp.Entities.Enums;

namespace TsiErp.Entities.Entities.PurchaseManagement.PurchaseRequest.ReportDtos.PurchaseRequestListReportDtos
{
    public class PurchaseRequestListReportParameterDto
    {
        public List<Guid> Products { get; set; }

        public List<Guid> CurrentAccounts { get; set; }

        public List<PurchaseRequestLineStateEnum> PurchaseRequestLineState { get; set; }

        public DateTime? StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        public PurchaseRequestListReportParameterDto()
        {
            Products = new List<Guid>();
            CurrentAccounts = new List<Guid>();
            PurchaseRequestLineState = new List<PurchaseRequestLineStateEnum>();
        }
    }
}
