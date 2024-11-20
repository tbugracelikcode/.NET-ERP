using Tsi.Core.Entities.Auditing;
using TSI.QueryBuilder.MappingAttributes;
using TsiErp.Entities.Entities.FinanceManagement.BankBalanceCashFlowLine.Dtos;

namespace TsiErp.Entities.Entities.FinanceManagement.BankBalanceCashFlow.Dtos
{
    public class SelectBankBalanceCashFlowsDto : FullAuditedEntityDto
    {
        /// <summary>
        /// Yıl
        /// </summary>
        public int Year_ { get; set; }

        [NoDatabaseAction]
        public List<SelectBankBalanceCashFlowLinesDto> SelectBankBalanceCashFlowLinesDto { get; set; }
    }
}
