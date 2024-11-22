using Tsi.Core.Entities.Auditing;
using TSI.QueryBuilder.MappingAttributes;
using TsiErp.Entities.Entities.FinanceManagement.BankBalanceCashFlowLine.Dtos;

namespace TsiErp.Entities.Entities.FinanceManagement.BankBalanceCashFlow.Dtos
{
    public class UpdateBankBalanceCashFlowsDto : FullAuditedEntityDto
    {
        /// <summary>
        /// Yıl
        /// </summary>
        public int Year_ { get; set; }
        /// <summary>
        /// Açıklama
        /// </summary>
        public string _Description { get; set; }

        [NoDatabaseAction]
        public List<SelectBankBalanceCashFlowLinesDto> SelectBankBalanceCashFlowLinesDto { get; set; }
    }
}
