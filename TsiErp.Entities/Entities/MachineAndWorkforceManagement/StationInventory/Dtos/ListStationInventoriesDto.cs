using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tsi.Core.Entities.Auditing;

namespace TsiErp.Entities.Entities.MachineAndWorkforceManagement.StationInventory.Dtos
{
    public class ListStationInventoriesDto : FullAuditedEntityDto
    {
        public Guid? StationID { get; set; }
        public Guid? ProductID { get; set; }
        /// <summary>
        /// Stok Kodu
        /// </summary>
        public string ProductCode { get; set; }
        /// <summary>
        /// Stok Açıklaması
        /// </summary>
        public string ProductName { get; set; }
        /// <summary>
        /// Miktar
        /// </summary>
        public decimal Amount { get; set; }
        /// <summary>
        /// Açıklama
        /// </summary>
        public string Description_ { get; set; }
    }
}
