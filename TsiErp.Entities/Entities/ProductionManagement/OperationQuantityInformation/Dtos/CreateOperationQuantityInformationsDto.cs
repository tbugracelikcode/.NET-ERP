using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tsi.Core.Entities;
using Tsi.Core.Utilities.SqlDataTypeMappingUtilities;
using TsiErp.Entities.Enums;

namespace TsiErp.Entities.Entities.ProductionManagement.OperationQuantityInformation.Dtos
{
    public class CreateOperationQuantityInformationsDto : IEntityDto
    {
        /// <summary>
        /// Id
        /// </summary>
        public Guid Id { get; set; }
        /// <summary>
        /// İş Emri ID
        /// </summary>
        public Guid WorkOrderID { get; set; }
        /// <summary>
        /// Üretim Emri ID
        /// </summary>
        public Guid ProductionOrderID { get; set; }
        /// <summary>
        /// Operatör ID
        /// </summary>
        public Guid OperatorID { get; set; }
        /// <summary>
        /// Bağlama Süresi
        /// </summary>
        public decimal AttachmentTime { get; set; }
        /// <summary>
        /// Operasyon Süresi
        /// </summary>
        public decimal OperationTime { get; set; }
        /// <summary>
        /// Üretim Takip ID
        /// </summary>
        public Guid ProductionTrackingID { get; set; }
        /// <summary>
        /// İstasyon ID
        /// </summary>
        public Guid StationID { get; set; }
        /// <summary>
        /// Operasyon ID
        /// </summary>
        public Guid ProductsOperationID { get; set; }
        /// <summary>
        /// Tür
        /// </summary>
        public int OperationQuantityInformationsType { get; set; }
    }
}
