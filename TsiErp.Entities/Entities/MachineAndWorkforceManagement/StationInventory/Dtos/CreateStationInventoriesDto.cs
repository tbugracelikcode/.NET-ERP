using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tsi.Core.Entities.Auditing;

namespace TsiErp.Entities.Entities.MachineAndWorkforceManagement.StationInventory.Dtos
{
    public class CreateStationInventoriesDto : FullAuditedEntityDto
    {
        /// <summary>
        /// İş İstasyonu ID
        /// </summary>
        public Guid? StationID { get; set; }
        /// <summary>
        /// Stok ID
        /// </summary>
        public Guid? ProductID { get; set; }
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
