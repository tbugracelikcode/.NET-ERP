using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tsi.Core.Entities.Auditing;
using TSI.QueryBuilder.MappingAttributes;
using TsiErp.Entities.Entities.MaintenanceManagement.MaintenanceInstructionLine.Dtos;

namespace TsiErp.Entities.Entities.MaintenanceManagement.MaintenanceInstruction.Dtos
{
    public class SelectMaintenanceInstructionsDto : FullAuditedEntityDto
    {
        /// <summary>
        /// Kod
        /// </summary>
        public string Code { get; set; }
        /// <summary>
        /// Bakım Talimat Adı
        /// </summary>
        public string InstructionName { get; set; }
        /// <summary>
        /// İstasyon ID
        /// </summary>
        public Guid? StationID { get; set; }
        /// <summary>
        /// İstasyon Kodu
        /// </summary>
        public string StationCode { get; set; }
        /// <summary>
        /// Bakım Periyodu ID
        /// </summary>
        public Guid? PeriodID { get; set; }
        /// <summary>
        /// Periyot Adı
        /// </summary>
        public string PeriodName { get; set; }
        /// <summary>
        /// Periyot Süresi
        /// </summary>
        public decimal PeriodTime { get; set; }
        /// <summary>
        /// Planlanan Bakım Süresi
        /// </summary>
        public decimal PlannedMaintenanceTime { get; set; }
        /// <summary>
        /// Not
        /// </summary>
        public string Note_ { get; set; }
        [NoDatabaseAction]
        public List<SelectMaintenanceInstructionLinesDto> SelectMaintenanceInstructionLines { get; set; }
    }
}
