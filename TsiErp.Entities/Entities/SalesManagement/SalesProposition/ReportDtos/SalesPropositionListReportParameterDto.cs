using TsiErp.Entities.Enums;

namespace TsiErp.Entities.Entities.SalesManagement.SalesProposition.ReportDtos
{
    public class SalesPropositionListReportParameterDto
    {
        public List<Guid> Products { get; set; }

        public List<Guid> CurrentAccounts { get; set; }

        public List<SalesPropositionLineStateEnum> SalesPropositionLineState { get; set; }

        public DateTime? StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        public SalesPropositionListReportParameterDto()
        {
            Products = new List<Guid>();
            CurrentAccounts = new List<Guid>();
            SalesPropositionLineState = new List<SalesPropositionLineStateEnum>();
        }
    }
}
