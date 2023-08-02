using Tsi.Core.Entities;



namespace TsiErp.Entities.Entities.GeneralSystemIdentifications.SalesManagementParameter.Dtos
{
    public class UpdateSalesManagementParametersDto : IEntityDto
    {
        /// <summary>
        /// Sipariş İleri Zamanlı Tarih Parametresi
        /// </summary>
        public bool OrderFutureDateParameter { get; set; }
        /// <summary>
        /// Teklif İleri Zamanlı Tarih Parametresi
        /// </summary>
        public bool PropositionFutureDateParameter { get; set; }
        /// <summary>
        /// Id
        /// </summary>
        public Guid Id { get; set; }
    }
}
