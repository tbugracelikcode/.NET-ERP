using TsiErp.Entities.Enums;

namespace TsiErp.Entities.Entities.PurchaseManagement.PurchaseOrder.ReportDtos.PurchaseOrderListReportDtos
{
    public class PurchaseOrderListReportParameterDto
    {
        public List<Guid> Products { get; set; }

        public List<Guid> CurrentAccounts { get; set; }

        public List<PurchaseOrderLineStateEnum> PurchaseOrderLineState { get; set; }

        public DateTime? StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        public PurchaseOrderListReportParameterDto()
        {
            Products = new List<Guid>();
            CurrentAccounts = new List<Guid>();
            PurchaseOrderLineState = new List<PurchaseOrderLineStateEnum>();
        }
    }
}
