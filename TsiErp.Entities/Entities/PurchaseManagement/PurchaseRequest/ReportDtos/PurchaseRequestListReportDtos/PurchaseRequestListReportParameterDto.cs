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

        public List<Guid> CurrentAcounts { get; set; }

        public List<PurchaseRequestStateEnum> PurchaseRequestStates { get; set; }

        public DateTime? StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        public PurchaseRequestListReportParameterDto()
        {
            Products = new List<Guid>();
            CurrentAcounts = new List<Guid>();
            PurchaseRequestStates = new List<PurchaseRequestStateEnum>();
        }
    }
}
